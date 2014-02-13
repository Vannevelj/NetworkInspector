using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkInspector.Models.Headers.Application.HTTP;

namespace UnitTests
{
    [TestClass]
    public class HTTPHeaderTests
    {
        private static string _httpRequestType = "GET";
        private static string _page = "/tutorials/other/top-20-mysql-best-practices/";
        private static string _httpVersion = "HTTP/1.1";
        private static string _host = "net.tutsplus.com";

        private static string _userAgent =
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)";

        private static string _accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private static string _acceptLanguage = "en-us,en;q=0.5";
        private static string _acceptEncoding = "gzip,deflate";
        private static string _acceptCharset = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
        private static string _connection = "keep-alive";
        private static string _cookie = "PHPSESSID=r2t5uvjq435r4q7ib3vtdjq120; somekey=somevalue";
        private static string _cacheControl = "no-cache";


        private static byte[] GetHeader()
        {
            var builder = new StringBuilder();
            builder.Append(string.Format("{0} {1} {2}", _httpRequestType, _page, _httpVersion)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Host", _host)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "User-Agent", _userAgent)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Accept", _accept)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Accept-Language", _acceptLanguage)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Accept-Encoding", _acceptEncoding)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Accept-Charset", _acceptCharset)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Connection", _connection)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Cookie", _cookie)).Append("\n");
            builder.Append(string.Format("{0}: {1}", "Cache-Control", _cacheControl)).Append("\n");

            return Encoding.ASCII.GetBytes(builder.ToString());
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_HTTPRequestType()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_httpRequestType, parsedHeader.RequestType.ToString());
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_Page()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_page, parsedHeader.Page);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_HttpVersion()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(new Version(1, 1), parsedHeader.HTTPVersion);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_Host()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_host, parsedHeader.Host);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_UserAgent()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_userAgent, parsedHeader.UserAgent);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_AcceptPreferences()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            var preferences = parsedHeader.Accept.ToArray();

            Assert.AreEqual("text/html", preferences[0].Type);
            Assert.AreEqual(1, preferences[0].Weight);
            Assert.AreEqual(1, preferences[0].Order);

            Assert.AreEqual("application/xhtml+xml", preferences[1].Type);
            Assert.AreEqual(1, preferences[1].Weight);
            Assert.AreEqual(2, preferences[1].Order);

            Assert.AreEqual("application/xml", preferences[2].Type);
            Assert.AreEqual(0.9, preferences[2].Weight);
            Assert.AreEqual(3, preferences[2].Order);

            Assert.AreEqual("*/*", preferences[3].Type);
            Assert.AreEqual(0.8, preferences[3].Weight);
            Assert.AreEqual(4, preferences[3].Order);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_Language()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_acceptLanguage, parsedHeader.AcceptLanguage);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_Encoding()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            var encodings = parsedHeader.AcceptEncoding.ToArray();

            Assert.AreEqual("gzip", encodings[0]);
            Assert.AreEqual("deflate", encodings[1]);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_CharsetPreferences()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            var charsets = parsedHeader.AcceptCharsets.ToArray();

            Assert.AreEqual("ISO-8859-1", charsets[0].Charset);
            Assert.AreEqual(1, charsets[0].Weight);
            Assert.AreEqual(1, charsets[0].Order);

            Assert.AreEqual("utf-8", charsets[1].Charset);
            Assert.AreEqual(0.7, charsets[1].Weight);
            Assert.AreEqual(2, charsets[1].Order);

            Assert.AreEqual("*", charsets[2].Charset);
            Assert.AreEqual(0.7, charsets[2].Weight);
            Assert.AreEqual(3, charsets[2].Order);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_Connection()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_connection, parsedHeader.Connection);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_Cookie()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            var cookies = parsedHeader.Cookies.ToArray();

            Assert.AreEqual("PHPSESSID", cookies[0].Key);
            Assert.AreEqual("r2t5uvjq435r4q7ib3vtdjq120", cookies[0].Value);

            Assert.AreEqual("somekey", cookies[1].Key);
            Assert.AreEqual("somevalue", cookies[1].Value);
        }

        [TestMethod]
        public void HttpHeader_ShouldReturn_CacheControl()
        {
            var header = GetHeader();
            var length = header.Length;

            var parsedHeader = new HTTPHeader(header, length);

            Assert.AreEqual(_cacheControl, parsedHeader.CacheControl);
        }
    }
}