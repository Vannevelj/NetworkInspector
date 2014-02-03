namespace NetworkInspector.Models.Headers.Application.HTTP.HeaderFields
{
    public struct CustomField
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("Key: {0}, Value: {1}", Key, Value);
        }
    }
}
