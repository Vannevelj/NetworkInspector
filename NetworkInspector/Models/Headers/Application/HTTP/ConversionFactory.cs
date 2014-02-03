using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NetworkInspector.Models.Headers.Application.HTTP.HeaderFields;

namespace NetworkInspector.Models.Headers.Application.HTTP
{
    public class ConversionFactory
    {
        private static string GetString(object o)
        {
            return (string) o;
        }

        private static int GetInt(object o)
        {
            return System.Convert.ToInt32(o);
        }

        private static IEnumerable<string> GetStringEnumerable(object o, string delimiter)
        {
            return
                ((string) o).Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
        }

        private static IEnumerable<AcceptPreference> GetAcceptPreferenceEnumerable(object o, string delimiter)
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

        private static IEnumerable<CharsetPreference> GetCharsetPreferenceEnumerable(object o, string delimiter)
        {
            var entries = ((string)o).Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            return
                entries.Select(entry => entry.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    .Select((pairs, x) => new CharsetPreference
                    {
                        Charset = pairs[0].Trim(),
                        Weight =
                            pairs.Length > 1
                                ? Double.Parse(pairs[1].Trim().Substring(2), CultureInfo.InvariantCulture)
                                : 1, // Substring the 'q=<value>' from the weight
                        Order = ++x
                    }).ToList();
        } 

        private static IEnumerable<Cookie> GetCookieEnumerable(object o, string delimiter)
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

        private static DateTime GetDateTime(object o)
        {
            const string format = "ddd, dd MMM yyyy HH:mm:ss Z";
            return DateTime.ParseExact((string) o, format, CultureInfo.InvariantCulture).ToUniversalTime();
        }

        private static CustomField GetCustomHeader(string key, object o)
        {
            return new CustomField
            {
                Key = key,
                Value = (string) o
            };
        } 

        public static T Convert<T>(string key, object value)
        {
            // Handle custom headers
            // http://stackoverflow.com/questions/3561381/custom-http-headers-naming-conventions/3561399#3561399
            if (key.StartsWith("X-"))
            {
                return (T) System.Convert.ChangeType(GetCustomHeader(key, value), typeof(T));
            }

            switch (key)
            {
                case "Connection":
                case "User-Agent":
                case "Cache-Control":
                case "If-None-Match":
                case "Referer":
                case "Origin":
                case "Host":
                case "Accept-Language":
                    return (T) System.Convert.ChangeType(GetString(value), typeof (T));

                case "Accept-Encoding":
                    return (T) GetStringEnumerable(value, ",");

                case "Content-Type":
                    return (T) GetStringEnumerable(value, ";");

                case "Accept-Charset":
                    return (T) GetCharsetPreferenceEnumerable(value, ",");

                case "Accept":
                    return (T) GetAcceptPreferenceEnumerable(value, ",");

                case "Content-Length":
                    return (T) System.Convert.ChangeType(GetInt(value), typeof (T));

                case "Cookie":
                    return (T) GetCookieEnumerable(value, ";");

                case "If-Modified-Since":
                    return (T) System.Convert.ChangeType(GetDateTime(value), typeof (T));

                default:
                    throw new ArgumentException();
            }
        }
    }
}