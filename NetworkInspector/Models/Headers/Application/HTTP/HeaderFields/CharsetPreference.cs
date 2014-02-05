namespace NetworkInspector.Models.Headers.Application.HTTP.HeaderFields
{
    public struct CharsetPreference
    {
        public string Charset { get; set; }
        public double Weight { get; set; }
        public int Order { get; set; }

        public override string ToString()
        {
            return string.Format("Charset: {0}, Weight: {1}, Order: {2}", Charset, Weight, Order);
        }
    }
}