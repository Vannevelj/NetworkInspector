using System.Globalization;

namespace NetworkInspector.Models.Headers.Application.HTTP
{
    internal struct CulturePreference
    {
        public CultureInfo Culture { get; set; }
        public int Weight { get; set; }
    }
}