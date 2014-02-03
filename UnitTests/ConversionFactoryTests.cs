using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Headers.Application.HTTP.HeaderFields;

namespace UnitTests
{
    [TestClass]
    public class ConversionFactoryTests
    {
        [TestMethod]
        public void HostAsObject_ShouldReturn_String()
        {
            const string host = "programmers.stackexchange.com";
            object obj = host;

            Assert.AreEqual(host, ConversionFactory.Convert<string>("Host", obj));
        }

        [TestMethod]
        public void ConnectionAsObject_ShouldReturn_String()
        {
            const string connection = "keep-alive";
            object obj = connection;

            Assert.AreEqual(connection, ConversionFactory.Convert<string>("Connection", obj));
        }

        [TestMethod]
        public void UserAgentAsObject_ShouldReturn_String()
        {
            const string userAgent =
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.76 Safari/537.36";
            object obj = userAgent;

            Assert.AreEqual(userAgent, ConversionFactory.Convert<string>("User-Agent", obj));
        }

        [TestMethod]
        public void AcceptAsObject_ShouldReturn_ListOfAcceptPreference()
        {
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            object obj = accept;

            var result = ConversionFactory.Convert<List<AcceptPreference>>("Accept", obj);

            Assert.AreEqual("text/html", result[0].Type);
            Assert.AreEqual(1, result[0].Weight);
            Assert.AreEqual(1, result[0].Order);

            Assert.AreEqual("application/xhtml+xml", result[1].Type);
            Assert.AreEqual(1, result[1].Weight);
            Assert.AreEqual(2, result[1].Order);

            Assert.AreEqual("application/xml", result[2].Type);
            Assert.AreEqual(0.9, result[2].Weight);
            Assert.AreEqual(3, result[2].Order);

            Assert.AreEqual("image/webp", result[3].Type);
            Assert.AreEqual(1, result[3].Weight);
            Assert.AreEqual(4, result[3].Order);

            Assert.AreEqual("*/*", result[4].Type);
            Assert.AreEqual(0.8, result[4].Weight);
            Assert.AreEqual(5, result[4].Order);
        }

        [TestMethod]
        public void AcceptEncodingAsObject_ShouldReturn_ListOfString()
        {
            const string acceptEncoding = "gzip, deflate, sdch";
            object obj = acceptEncoding;

            var result = ConversionFactory.Convert<List<string>>("Accept-Encoding", obj);

            Assert.IsTrue(result.Contains("gzip"));
            Assert.IsTrue(result.Contains("deflate"));
            Assert.IsTrue(result.Contains("sdch"));
        }

        [TestMethod]
        public void AcceptLanguageAsObject_ShouldReturn_String()
        {
            const string acceptLanguage = "nl-NL,nl;q=0.8,en-US;q=0.6,en;q=0.4,fr;q=0.2";
            object obj = acceptLanguage;

            Assert.AreEqual(acceptLanguage, ConversionFactory.Convert<string>("Accept-Language", obj));
        }

        [TestMethod]
        public void CacheControlAsObject_ShouldReturn_String()
        {
            const string cacheControl = "max-age=0";
            object obj = cacheControl;

            Assert.AreEqual(cacheControl, ConversionFactory.Convert<string>("Cache-Control", obj));
        }

        [TestMethod]
        public void IfModifiedSinceAsObject_ShouldReturn_DateTime()
        {
            const string ifModifiedSince = "Sun, 02 Feb 2014 09:46:17 GMT";
            const int year = 2014;
            const int month = 2;
            const int day = 2;
            const int hour = 9;
            const int minute = 46;
            const int second = 17;

            object obj = ifModifiedSince;

            var result = ConversionFactory.Convert<DateTime>("If-Modified-Since", obj);

            Assert.AreEqual(year, result.Year);
            Assert.AreEqual(month, result.Month);
            Assert.AreEqual(day, result.Day);
            Assert.AreEqual(hour, result.Hour);
            Assert.AreEqual(minute, result.Minute);
            Assert.AreEqual(second, result.Second);
        }

        [TestMethod]
        public void RefererAsObject_ShouldReturn_String()
        {
            const string referer = "http://www.chess.com/echess/game?id=841781";
            object obj = referer;

            Assert.AreEqual(referer, ConversionFactory.Convert<string>("Referer", obj));
        }

        [TestMethod]
        public void CookieAsObject_ShouldReturn_Cookie()
        {
            const string cookie =
                "guest_id=v1%3A139035819669988; __utma=43838368.646140943.1390426206.1391168239.1391343148.7; __utmz=43838368.1391343148.7.6.utmcsr=reddit.com|utmccn=(referral)|utmcmd=referral|utmcct=/r/belgium/comments/1wr3o9/anybody_any_idea_what_is_going_on_with_laurent/";
            object obj = cookie;

            var result = ConversionFactory.Convert<List<Cookie>>("Cookie", obj);

            Assert.AreEqual("guest_id", result[0].Key);
            Assert.AreEqual("v1%3A139035819669988", result[0].Value);

            Assert.AreEqual("__utma", result[1].Key);
            Assert.AreEqual("43838368.646140943.1390426206.1391168239.1391343148.7", result[1].Value);

            Assert.AreEqual("__utmz", result[2].Key);
            Assert.AreEqual(
                "43838368.1391343148.7.6.utmcsr=reddit.com|utmccn=(referral)|utmcmd=referral|utmcct=/r/belgium/comments/1wr3o9/anybody_any_idea_what_is_going_on_with_laurent/",
                result[2].Value);
        }

        [TestMethod]
        public void ContentLengthAsObject_ShouldReturn_Int()
        {
            const string contentLength = "398";
            object obj = contentLength;

            Assert.AreEqual(Int32.Parse(contentLength), ConversionFactory.Convert<int>("Content-Length", obj));
        }

        [TestMethod]
        public void MultipleCustomHeadersAsObjects_ShouldReturn_ListOfCustomHeaders()
        {
            const string key1 = "X-Requested-With";
            const string value1 = "XMLHttpRequest";

            const string key2 = "X-Chrome-Variations";
            const string value2 = "CNa1yQEIjrbJAQiYtskBCKK2yQEIp7bJAQiptskBCLmDygE=";

        }

        [TestMethod]
        public void ContentTypeAsObject_ShouldReturn_ListOfString()
        {
            const string contentType = "application/x-www-form-urlencoded";
            object obj = contentType;

            var result = ConversionFactory.Convert<List<string>>("Content-Type", obj);

            Assert.AreEqual("application/x-www-form-urlencoded", result[0]);
        }

        [TestMethod]
        public void OriginAsObject_ShouldReturn_String()
        {
            const string origin = "https://www.youtube.com";
            object obj = origin;

            Assert.AreEqual(origin, ConversionFactory.Convert<string>("Origin", obj));
        }

        [TestMethod]
        public void IfNoneMatchAsObject_ShouldReturn_String()
        {
            const string ifNoneMatch = "\"5086f4b5-2398\"";
            object obj = ifNoneMatch;

            Assert.AreEqual(ifNoneMatch, ConversionFactory.Convert<string>("If-None-Match", obj));
        }

        [TestMethod]
        public void AcceptCharsetAsObject_ShouldReturn_ListOfCharsetPreference()
        {
            const string charset = "ISO-8859-1,utf-8,*;q=0.7";
            object obj = charset;

            var result = ConversionFactory.Convert<List<CharsetPreference>>("Accept-Charset", obj);

            Assert.AreEqual("ISO-8859-1", result[0].Charset);
            Assert.AreEqual(1, result[0].Weight);
            Assert.AreEqual(1, result[0].Order);

            Assert.AreEqual("utf-8", result[1].Charset);
            Assert.AreEqual(1, result[1].Weight);
            Assert.AreEqual(2, result[1].Order);

            Assert.AreEqual("*", result[2].Charset);
            Assert.AreEqual(0.7, result[2].Weight);
            Assert.AreEqual(3, result[2].Order);
        }

        [TestMethod]
        public void RangeAsObject_ShouldReturn_String()
        {
            const string range = "bytes=0-10783";
            object obj = range;

            Assert.AreEqual(range, ConversionFactory.Convert<string>("Range", obj));
        }
    }
}