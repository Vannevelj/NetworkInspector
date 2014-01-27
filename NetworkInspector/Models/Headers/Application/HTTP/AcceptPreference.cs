namespace NetworkInspector.Models.Headers.Application.HTTP
{
    internal struct AcceptPreference
    {
        public string Type { get; set; }
        public int Weight { get; set; }
    }
}