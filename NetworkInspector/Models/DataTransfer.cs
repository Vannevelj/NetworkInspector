using System.Collections.Generic;
using System.Linq;

namespace NetworkInspector.Models
{
    internal class DataTransfer
    {
        private readonly Queue<float> _latestUpload = new Queue<float>(new[] {0f, 0f, 0f});
        private readonly Queue<float> _latestDownload = new Queue<float>(new[] {0f, 0f, 0f});

        public void Send(float f)
        {
            TotalUploaded += f;
            _latestUpload.Dequeue();
            _latestUpload.Enqueue(f);
        }

        public void Receive(float f)
        {
            TotalDownloaded += f;
            _latestDownload.Dequeue();
            _latestDownload.Enqueue(f);
        }

        public float TotalUploaded { get; set; }
        public float TotalDownloaded { get; set; }

        public float LatestUploaded
        {
            get { return _latestUpload.Sum()/_latestUpload.Count; }
        }

        public float LatestDownloaded
        {
            get { return _latestDownload.Sum()/_latestDownload.Count; }
        }
    }
}