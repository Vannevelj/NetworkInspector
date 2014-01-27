using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
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

        private HTTPRequestType _requestType;
        private string _page;
        private HttpVersion _version;
        private Uri _host;
        private string _connection;
        private string _userAgent;
        private Uri _referer;
        private IEnumerable<string> _acceptEncoding;
        private IEnumerable<CulturePreference> _acceptLanguage;
        private int _contentLength;
        private string _requestedWith;
        private string _contentType;
        private IEnumerable<Tuple<string, string>> _cookieValues;
        private Uri _origin;
        private IEnumerable<string> _accept;
        private DateTime _ifModifiedSince;
        private string _cacheControl;

        public HTTPHeader(byte[] data, int length)
        {
            using (var stream = new MemoryStream(data, 0, length))
            {
                using (var reader = new StreamReader(stream))
                {
                    _log.Info(reader.ReadToEnd());
                    Parse(reader.ReadToEnd());
                }
            }
        }

        private void Parse(string data)
        {
        }

        public Protocol ProtocolName
        {
            get { return Protocol.HTTP; }
        }
    }
}