﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using log4net;
using NetworkInspector.Models.Headers.Application.HTTP.HeaderFields;
using NetworkInspector.Models.Headers.Transport;
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
        private List<string> _contentType;
        private List<Cookie> _cookieValues;
        private string _origin;
        private List<AcceptPreference> _accept;
        private DateTime _ifModifiedSince;
        private string _cacheControl;
        private string _ifNoneMatch;
        private readonly List<CustomField> _customHeaders = new List<CustomField>();
        private List<CharsetPreference> _acceptCharset; 

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
            else if (IsCustomHeader(key))
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
               
                _log.Debug(string.Format("{0}: {1}", key, value));
            }
        }

        private void AddCustomHeader(string key, string value)
        {
            _customHeaders.Add(ConversionFactory.Convert<CustomField>(key, value));
            _log.Debug(string.Format("Custom headers: [{0}]", string.Join(",", _customHeaders)));
        }

        private bool IsCustomHeader(string key)
        {
            return key.StartsWith("X-");
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
    }
}