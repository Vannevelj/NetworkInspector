namespace NetworkInspector.Models
{
    public class TransferRate
    {
        private readonly float _data;

        public TransferRate(float f)
        {
            _data = f;
        }

        public float BytesPerSecond
        {
            get { return _data; }
        }

        public float KiloBytesPerSecond
        {
            get { return BytesPerSecond/1028; }
        }

        public float MegaBytesPerSecond
        {
            get { return KiloBytesPerSecond/1028; }
        }
    }
}