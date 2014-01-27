namespace NetworkInspector.Models
{
    public class TransferRate
    {
        private float _data;

        public TransferRate()
        {
        }

        public TransferRate(float f)
        {
            _data = f;
        }

        public float InBytes
        {
            get { return _data; }
        }

        public float InKiloBytes
        {
            get { return InBytes/1028; }
        }

        public float InMegaBytes
        {
            get { return InKiloBytes/1028; }
        }

        public TransferRate Update(float f)
        {
            _data = f;
            return this;
        }
    }
}