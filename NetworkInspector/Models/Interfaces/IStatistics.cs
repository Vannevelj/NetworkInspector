using System.Collections.Generic;

namespace NetworkInspector.Models.Interfaces {
    public interface IStatistics {
        string NetworkInterface { get; }

        float DataSent { get; }

        float DataReceived { get; }

        float UploadSpeed { get; }

        float DownloadSpeed { get; }

        Queue<float> LatestDownTransfers { get; }

        Queue<float> LatestUpTransfers { get; }
    }
}