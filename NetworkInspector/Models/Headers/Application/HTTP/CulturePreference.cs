using System;
using System.Globalization;

namespace NetworkInspector.Models.Headers.Application.HTTP
{
    [Obsolete("Culture preferences are handled as a string right now", true)]
    internal struct CulturePreference
    {
        public CultureInfo Culture { get; set; }
        public double Weight { get; set; }
    }
}