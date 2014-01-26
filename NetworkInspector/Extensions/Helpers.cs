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
    }
}