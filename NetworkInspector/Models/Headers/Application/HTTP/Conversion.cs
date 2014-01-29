namespace NetworkInspector.Models.Headers.Application.HTTP
{
    internal class Conversion
    {
        public string HTTPValue { get; private set; }
        public string ObjectValue { get; private set; }

        public Conversion(string http, string field)
        {
            HTTPValue = http;
            ObjectValue = field;
        }
    }
}