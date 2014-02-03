using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NetworkInspector.Models.Headers.Application.HTTP
{
    public class ConversionFactory
    {
        private static string GetStringValue(object o)
        {
            return (string) o;
        }

        private static int GetIntValue(object o)
        {
            return System.Convert.ToInt32(o);
        }

        private static IEnumerable<string> GetStringEnumerableValue(object o, string delimiter)
        {
            return
                ((string) o).Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
        }

        private static IEnumerable<AcceptPreference> GetPreferenceEnumerableValue(object o, string delimiter)
        {
            var entries = ((string) o).Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries);

            return
                entries.Select(entry => entry.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
                    .Select((pairs, x) => new AcceptPreference
                    {
                        Type = pairs[0].Trim(),
                        Weight =
                            pairs.Length > 1
                                ? Double.Parse(pairs[1].Trim().Substring(2), CultureInfo.InvariantCulture)
                                : 1, // Substring the 'q=<value>' from the weight
                        Order = ++x
                    }).ToList();
        }

        private static IEnumerable<Cookie> GetCookieEnumerableValue(object o, string delimiter)
        {
            var entries = ((string) o).Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries);

            return
                entries.Select(
                    x => x.Split(new[] {"="}, 2, StringSplitOptions.RemoveEmptyEntries))
                    .Select(pairs => new Cookie
                    {
                        Key = pairs[0].Trim(),
                        Value = pairs[1].Trim()
                    }).ToList();
        }

        private static DateTime GetDateTimeValue(object o)
        {
            const string format = "ddd, dd MMM yyyy HH:mm:ss Z";
            return DateTime.ParseExact((string) o, format, CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public static T Convert<T>(string key, object value)
        {
            switch (key)
            {
                case "Connection":
                case "User-Agent":
                case "X-Requested-With":
                case "Cache-Control":
                case "If-None-Match":
                case "Referer":
                case "Origin":
                case "Host":
                case "Accept-Language":
                    return (T) System.Convert.ChangeType(GetStringValue(value), typeof (T));

                case "Accept-Encoding":
                    return (T) GetStringEnumerableValue(value, ",");

                case "Content-Type":
                    return (T) GetStringEnumerableValue(value, ";");

                case "Accept":
                    return (T) GetPreferenceEnumerableValue(value, ",");

                case "Content-Length":
                    return (T) System.Convert.ChangeType(GetIntValue(value), typeof (T));

                case "Cookie":
                    return (T) GetCookieEnumerableValue(value, ";");

                case "If-Modified-Since":
                    return (T) System.Convert.ChangeType(GetDateTimeValue(value), typeof (T));

                default:
                    throw new ArgumentException();
            }
        }
    }
}