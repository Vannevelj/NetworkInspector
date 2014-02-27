using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using log4net;
using NetworkInspector.Models.Headers.Application.HTTP.HeaderFields;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;
using Cookie = NetworkInspector.Models.Headers.Application.HTTP.HeaderFields.Cookie;

namespace NetworkInspector.Models.Headers.Application.HTTP
{
    public enum HTTPRequestType
    {
        GET,
        POST,
        PUT,
        HEAD,
        DELETE,
        TRACE,
        OPTIONS,
        CONNECT,
        PATCH
    }

    public class HTTPHeader : IHeader
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly List<Conversion> _conversions = new List<Conversion>();
        private static readonly ConversionFactory _factory = new ConversionFactory();

        #region Conversions

        static HTTPHeader()
        {
            var doc = XDocument.Load(@"Models\Headers\Application\HTTP\conversions.xml");
            foreach (var pair in doc.Descendants("PAIR"))
            {
                _conversions.Add(
                    new Conversion(
                        pair.Element("HTTP").Value,
                        pair.Element("FIELD").Value));
            }

            _log.Info(string.Format("{0} HTTP fields loaded", _conversions.Count));
        }

        #endregion Conversions

#pragma warning disable 649 // Disable the 'not assigned variable warning' -> reflection sets values
        private HTTPRequestType _requestType;
        private string _page;
        private Version _version;
        private string _host;
        private string _connection;
        private string _userAgent;
        private string _referer;
        private readonly List<string> _acceptEncoding = new List<string>();
        private string _acceptLanguage;
        private int _contentLength;
        private readonly List<string> _contentType = new List<string>();
        private readonly List<Cookie> _cookieValues = new List<Cookie>();
        private string _origin;
        private readonly List<AcceptPreference> _accept = new List<AcceptPreference>();
        private DateTime _ifModifiedSince;
        private string _cacheControl;
        private string _ifNoneMatch;
        private readonly List<CustomField> _customHeaders = new List<CustomField>();
        private readonly List<CharsetPreference> _acceptCharset = new List<CharsetPreference>();
        private string _range;
        private readonly string _data;

        // Mainly response fields
        private int _responseCode;
        private string _status;
#pragma warning restor e 649

        public HTTPHeader(byte[] data, int length)
        {
            _data = System.Text.Encoding.Default.GetString(data);
            using (var stream = new MemoryStream(data, 0, length))
            {
                using (var reader = new StreamReader(stream))
                {
                    var info = reader.ReadToEnd();
                    Parse(info);
                }
            }
        }

        private void Parse(string data)
        {
            data = data.Replace("\r", "");
            var fields = data.Trim().Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length <= 1) return; // Host is field[1]; I want at least a host entry.

            DetermineType(fields[0]);

            for (var i = 1; i < fields.Length; i++)
            {
                ParseField(fields[i]);
            }
        }

        // Checks whether it's a request or response
        private void DetermineType(string s)
        {
            var fields = s.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length < 3)
            {
                return;
            }

            // In case it's a request
            var parsed = Enum.TryParse(fields[0], out _requestType);
            if (parsed)
            {
                _page = fields[1];
                _version = fields[2] == "HTTP/1.1" ? HttpVersion.Version11 : HttpVersion.Version10;
            }

            var isResponse = Int32.TryParse(fields[1], out _responseCode);
            if (isResponse)
            {
                _version = fields[0] == "HTTP/1.1" ? HttpVersion.Version11 : HttpVersion.Version10;
                _status = fields[2];
                _log.Debug(s);
            }
        }

        private void ParseField(string field)
        {
            var pair = field.Split(new[] {":"}, 2, StringSplitOptions.RemoveEmptyEntries);

            if (pair.Length < 2)
            {
                return;
            }

            var key = pair[0].Trim();
            var value = pair[1].Trim();

            var conversion = _conversions.FirstOrDefault(x => x.HTTPValue == key);

            if (conversion != null)
            {
                ConvertData(conversion.ObjectValue, conversion.HTTPValue, value);
            }
            else if (IsCustomHeader(key) || IsDeprecatedCustomHeader(key, value))
            {
                AddCustomHeader(key, value);
            }
            else
            {
                _log.Warn(string.Format("Field \"{0}\" could not be found. Value is \"{1}\".", key, value));
            }
        }

        private void ConvertData(string localField, string key, string value)
        {
            var obj =
                GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .FirstOrDefault(x => x.Name == localField);

            if (obj != null)
            {
                var type = obj.FieldType;

                var method = _factory.GetType()
                    .GetMethod("Convert")
                    .MakeGenericMethod(new[] {type});

                var result = method.Invoke(this, new object[] {key, value});
                obj.SetValue(this, result);
            }
        }

        private void AddCustomHeader(string key, string value)
        {
            _customHeaders.Add(ConversionFactory.Convert<CustomField>(key, value));
            _log.Debug(string.Format("Custom headers: [{0}]", string.Join(",", _customHeaders)));
        }

        private bool IsCustomHeader(string key)
        {
            return key.StartsWith("X-") || key.StartsWith("x-");
        }

        // Custom headers should start with 'X-' but this wasn't always done so.
        private bool IsDeprecatedCustomHeader(string key, string value)
        {
            return key.Trim().Length >= 1 && value.Trim().Length >= 1;
        }

        public Protocol ProtocolName
        {
            get { return Protocol.HTTP; }
        }

        public HTTPRequestType RequestType
        {
            get { return _requestType; }
        }

        public string Page
        {
            get { return _page; }
        }

        public Version HTTPVersion
        {
            get { return _version ?? new Version(0, 0); }
        }

        public string Host
        {
            get { return _host; }
        }

        public string Connection
        {
            get { return _connection; }
        }

        public string UserAgent
        {
            get { return _userAgent; }
        }

        public IEnumerable<AcceptPreference> Accept
        {
            get { return _accept; }
        }

        public IEnumerable<string> AcceptEncoding
        {
            get { return _acceptEncoding; }
        }

        public string AcceptLanguage
        {
            get { return _acceptLanguage; }
        }

        public string CacheControl
        {
            get { return _cacheControl; }
        }

        /// <summary>
        ///     Returns <code>DateTime.MinValue</code> if no date could be parsed. Dates are returned in UTC time.
        /// </summary>
        public DateTime IfModifiedSince
        {
            get { return _ifModifiedSince; }
        }

        public string Referer
        {
            get { return _referer; }
        }

        public IEnumerable<Cookie> Cookies
        {
            get { return _cookieValues; }
        }

        public int ContentLength
        {
            get { return _contentLength; }
        }

        public IEnumerable<CustomField> CustomHeaders
        {
            get { return _customHeaders; }
        }

        public List<string> ContentType
        {
            get { return _contentType; }
        }

        public string Origin
        {
            get { return _origin; }
        }

        public string IfNoneMatch
        {
            get { return _ifNoneMatch; }
        }

        public IEnumerable<CharsetPreference> AcceptCharsets
        {
            get { return _acceptCharset; }
        }

        public string Range
        {
            get { return _range; }
        }

        public Dictionary<string, string> GetFieldRepresentation()
        {
            return new Dictionary<string, string>
            {
                {"Application Header Protocol", ProtocolName.ToString()},
                {"HTTP Version", string.Format("HTTP{0}/{1}", HTTPVersion.Major, HTTPVersion.Minor)},
                {"Request type", RequestType.ToString()},
                {"Host", Host},
                {"Page", Page},
                {"Referer", Referer},
                {"Origin", Origin},
                {"User-Agent", UserAgent},
                {"Content-Type", string.Join(",", ContentType)},
                {"Content-Length", ContentLength.ToString()},
                {"Range", Range},
                {"Accept", string.Join(",", Accept)},
                {"Accept-Charset", string.Join(",", AcceptCharsets)},
                {"Accept-Encoding", string.Join(",", AcceptEncoding)},
                {"Accept-Language", AcceptLanguage},
                {"Cache-Control", CacheControl},
                {"Connection", Connection},
                {"If-Modified-Since", IfModifiedSince.ToString()},
                {"If-None-Match", IfNoneMatch},
                {"Cookies", string.Join("\n", Cookies)},
                {"Custom headers", string.Join("\n", CustomHeaders)},
                {"Raw data", _data}
            };
        }
    }
}