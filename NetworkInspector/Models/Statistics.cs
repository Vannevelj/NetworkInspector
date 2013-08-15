using System.Collections.Generic;
using System.Linq;

namespace NetworkInspector.Models {
    public class Statistics : IStatistics {
        public Statistics(string name) {
            NetworkInterface = name;
            LatestDownTransfers = new Queue<float>(new[] { 0f, 0f, 0f });
            LatestUpTransfers = new Queue<float>(new[] { 0f, 0f, 0f });
        }

        // <summary>
        // Holds the name of the selected network interface
        // </summary>
        public string NetworkInterface { get; set; }

        // <summary>
        // Contains the total data sent since the start of capturing in bytes
        // </summary>
        public float DataSent { get; set; }

        // <summary>
        // Contains the total data received since the start of capturing in bytes
        // </summary>
        public float DataReceived { get; set; }

        // <summary>
        // Returns the upload speed in Bytes / Second
        // </summary>
        public float UploadSpeed {
            get { return LatestUpTransfers.Sum() / LatestUpTransfers.Count; }
        }

        // <summary>
        // Returns the download speed in Bytes / Second
        // </summary>
        public float DownloadSpeed {
            get { return LatestDownTransfers.Sum() / LatestDownTransfers.Count; }
        }

        // <summary>
        // Contains the data received in the three most recent time intervals
        // </summary>
        public Queue<float> LatestDownTransfers { get; set; }

        // <summary>
        // Contains the data sent in the three most recent time intervals
        // </summary>
        public Queue<float> LatestUpTransfers { get; set; }

        // <summary>
        // Adds a value to the current running statistics summary's upload list
        // </summary>
        public void AddSentData(float d) {
            DataSent += d;

            LatestUpTransfers.Dequeue();
            LatestUpTransfers.Enqueue(d);
        }

        // <summary>
        // Adds a value to the current running statistics summary's download list
        // </summary>
        public void AddReceivedData(float d) {
            DataReceived += d;

            LatestDownTransfers.Dequeue();
            LatestDownTransfers.Enqueue(d);
        }
    }
}