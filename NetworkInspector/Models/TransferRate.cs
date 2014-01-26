using System.Collections.Generic;
using System.Linq;

namespace NetworkInspector.Models
{
    public class TransferRate
    {
        private readonly Queue<float> _latestTransfers = new Queue<float>(new[] { 0f, 0f, 0f });

        public float BytesPerSecond
        {
            get { return _latestTransfers.Sum() / _latestTransfers.Count; }
        }

        public float KiloBytesPerSecond
        {
            get { return BytesPerSecond / 1028; } 
        }

        public float MegaBytesPerSecond
        {
            get { return KiloBytesPerSecond / 1028; }
        }

        public void AddDataPoint(float f)
        {
            _latestTransfers.Dequeue();
            _latestTransfers.Enqueue(f);
        }
    }
}