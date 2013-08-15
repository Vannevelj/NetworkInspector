using System.Collections.Generic;

namespace NetworkInspector.Models {
    public interface IStatistics {
        string NetworkInterface { get; }

        float DataSent { get; }

        float DataReceived { get; }

        float UploadSpeed { get; }

        float DownloadSpeed { get; }

        Queue<float> LatestDownTransfers { get; }

        Queue<float> LatestUpTransfers { get; }

        void AddSentData(float d);

        void AddReceivedData(float d);
    }
}