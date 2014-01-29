using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using log4net;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Packets;

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

        private HTTPRequestType _requestType;
        private string _page;
        private Version _version;
        private string _host;
        private string _connection;
        private string _userAgent;
        private string _referer;
        private List<string> _acceptEncoding;
        private string _acceptLanguage;
        private int _contentLength;
        private string _requestedWith;
        private List<string> _contentType;
        private List<Tuple<string, string>> _cookieValues;
        private string _origin;
        private List<AcceptPreference> _accept;
        private DateTime _ifModifiedSince;
        private string _cacheControl;
        private string _ifNoneMatch;

        public HTTPHeader(byte[] data, int length)
        {
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
            if (fields.Length <= 0) return;

            ParseTypePageVersion(fields[0]);

            for (var i = 1; i < fields.Length; i++)
            {
                ParseField(fields[i]);
            }
        }

        private void ParseTypePageVersion(string s)
        {
            var fields = s.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            var parsed = Enum.TryParse(fields[0], out _requestType);

            // Only continue parsing if the request type is valid
            if (!parsed) return;

            _page = fields[1];
            _version = fields[2] == "HTTP/1.1" ? HttpVersion.Version11 : HttpVersion.Version10;
        }

        private void ParseField(string field)
        {
            var pair = field.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);

            if (pair.Length != 2)
            {
                return;
            }

            var key = pair[0];
            var value = pair[1];

            var conversion = _conversions.FirstOrDefault(x => x.HTTPValue == key);

            if (conversion != null)
            {
                var obj =
                    GetType()
                        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                        .FirstOrDefault(x => x.Name == conversion.ObjectValue);

                if (obj != null)
                {
                    var type = obj.FieldType;
                    var factory = new ConversionFactory();

                    var method = factory.GetType()
                        .GetMethod("Convert")
                        .MakeGenericMethod(new[] {type});

                    obj.SetValue(this, method.Invoke(this, new object[] {conversion.HTTPValue, value.Trim()}));

                    _log.Info(obj.GetValue(this));
                }
            }
        }

        public Protocol ProtocolName
        {
            get { return Protocol.HTTP; }
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

        public DateTime IfModifiedSince
        {
            get { return _ifModifiedSince; }
        }

        public string Referer
        {
            get { return _referer; }
        }

        public IEnumerable<Tuple<string, string>> Cookies
        {
            get { return _cookieValues; }
        }

        public int ContentLength
        {
            get { return _contentLength; }
        }

        public string XRequestedWith
        {
            get { return _requestedWith; }
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
    }
}