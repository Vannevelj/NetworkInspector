namespace NetworkInspector.Models.Headers.Application.HTTP
{
    public struct AcceptPreference
    {
        public string Type { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            return string.Format("Type: {0}, Weight: {1}", Type, Weight);
        }
    }
}