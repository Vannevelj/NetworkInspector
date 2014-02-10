using System;
using System.Collections.Generic;

namespace NetworkInspector.Extensions
{
    public static class Helpers
    {
        /// <summary>
        ///     This extension replaces parantheses '(' and ')' with brackets '[' and ']'.
        ///     This is done because <code>NetworkInterface.GetAllNetworkInterfaces()</code>
        ///     returns a different version than
        ///     <code>new PerformanceCounterCategory("Network Interface").GetInstanceNames().ToList()</code>
        /// </summary>
        /// <param name="this">String to replace the brackets on</param>
        /// <returns></returns>
        public static string ReplaceOptionalBrackets(this string @this)
        {
            return @this.Replace('(', '[').Replace(')', ']');
        }

        /// <summary>
        /// Combines two dictionaries together.
        /// http://stackoverflow.com/questions/3982448/adding-a-dictionary-to-another
        /// </summary>
        /// <typeparam name="T"><code>Dictionary</code></typeparam>
        /// <typeparam name="S"><code>Dictionary</code></typeparam>
        /// <param name="source">Source dictionary</param>
        /// <param name="collection">Dictionary to add</param>
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }
    }
}