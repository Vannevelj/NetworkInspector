using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkInspector.Models.Headers.Application.HTTP;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class ConversionFactoryTests
    {
        [TestMethod]
        public void HostAsObject_ShouldReturn_StringValue()
        {
            string host = "programmers.stackexchange.com";
            object obj = (object) host;

            Assert.AreEqual(host, ConversionFactory.Convert<string>("Host", obj));
        }

        [TestMethod]
        public void ConnectionAsObject_ShouldReturn_StringValue()
        {
            string connection = "keep-alive";
            object obj = (object) connection;

            Assert.AreEqual(connection, ConversionFactory.Convert<string>("Connection", obj));
        }

        [TestMethod]
        public void UserAgentAsObject_ShouldReturn_StringValue()
        {
            string userAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.76 Safari/537.36";
            object obj = (object) userAgent;

            Assert.AreEqual(userAgent, ConversionFactory.Convert<string>("User-Agent", obj));
        }

        [Ignore]
        [TestMethod]
        public void AcceptAsObject_ShouldReturn_ListOfAcceptPreferenceValue()
        {
            string accept = "";
            object obj = (object) accept;

            var result = ConversionFactory.Convert<List<AcceptPreference>>("Accept", obj);

            foreach(var acc in result){
                StringAssert.Contains(accept, string.Format("{0}={1}", acc.Type, acc.Weight));
            } //TODO: this isn't correct yet
        }

        [Ignore]
        [TestMethod]
        public void AcceptEncodingAsObject_ShouldReturn_ListOfStringValue()
        {
            string acceptEncoding = "gzip, deflate, sdch";
            object obj = (object) acceptEncoding;

            var result = ConversionFactory.Convert<List<string>>("Accept-Encoding", obj);

            // Foreach
        }

        [TestMethod]
        public void AcceptLanguageAsObject_ShouldReturn_StringValue()
        {
            string acceptLanguage = "nl-NL,nl;q=0.8,en-US;q=0.6,en;q=0.4,fr;q=0.2";
            object obj = (object) acceptLanguage;

            Assert.AreEqual(acceptLanguage, ConversionFactory.Convert<string>("Accept-Language", obj));
        }

        [TestMethod]
        public void CacheControlAsObject_ShouldReturn_StringValue()
        {
            string cacheControl = "max-age=0";
            object obj = (object) cacheControl;

            Assert.AreEqual(cacheControl, ConversionFactory.Convert<string>("Cache-Control", obj));
        }

        [Ignore]
        [TestMethod]
        public void IfModifiedSinceAsObject_ShouldReturn_DateTimeValue()
        {
            string ifModifiedSince = "Sun, 02 Feb 2014 09:46:17 GMT";
            int year = 2014;
            int month = 2;
            int day = 2;
            int hour = 9;
            int minute = 46;
            int second = 17;

            object obj = (object) ifModifiedSince;

            var result = ConversionFactory.Convert<DateTime>("If-Modified-Since", obj);

            Assert.AreEqual(year, result.Year);
            Assert.AreEqual(month, result.Month);
            Assert.AreEqual(day, result.Day);
            Assert.AreEqual(hour, result.Hour);
            Assert.AreEqual(minute, result.Minute);
            Assert.AreEqual(second, result.Second);
        }

        [TestMethod]
        public void RefererAsObject_ShouldReturn_StringValue()
        {
            string referer = "";
            object obj = (object) referer;

            Assert.AreEqual(referer, ConversionFactory.Convert<string>("Referer", obj));
        }

        [Ignore]
        [TestMethod]
        public void CookieAsObject_ShouldReturn_CookieValue()
        {
            string cookie = "";
            object obj = (object) cookie;

            var result = ConversionFactory.Convert<List<Cookie>>("Cookie", obj);
            //TODO: asserts
        }

        [TestMethod]
        public void ContentLengthAsObject_ShouldReturn_IntValue()
        {
            string contentLength = "";
            object obj = (object) contentLength;

            Assert.AreEqual(Int32.Parse(contentLength), ConversionFactory.Convert<int>("Content-Length", obj));
        }

        [TestMethod]
        public void XRequestedWithAsObject_ShouldReturn_StringValue()
        {
            string requestedWith = "";
            object obj = (object) requestedWith;

            Assert.AreEqual(requestedWith, ConversionFactory.Convert<string>("X-Requested-With", obj));
        }

        [Ignore]
        [TestMethod]
        public void ContentTypeAsObject_ShouldReturn_ListOfStringValue()
        {
            string contentType = "";
            object obj = (object)contentType;

            //TODO
        }

        [TestMethod]
        public void OriginAsObject_ShouldReturn_StringValue()
        {
            string origin = "";
            object obj = (object) origin;

            Assert.AreEqual(origin, ConversionFactory.Convert<string>("Origin", obj));
        }

        [TestMethod]
        public void IfNoneMatchAsObject_ShouldReturn_StringValue()
        {
            string ifNoneMatch = "";
            object obj = (object)ifNoneMatch;

            Assert.AreEqual(ifNoneMatch, ConversionFactory.Convert<string>("If-None-Match", obj));
        }
    }
}
