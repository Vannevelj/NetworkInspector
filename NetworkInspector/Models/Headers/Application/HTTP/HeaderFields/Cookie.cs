namespace NetworkInspector.Models.Headers.Application.HTTP.HeaderFields
{
    public struct Cookie
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} = {1}", Key, Value);
        }
    }
}