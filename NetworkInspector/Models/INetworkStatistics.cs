using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NetworkInspector.Models {
    public interface INetworkStatistics {
        NetworkInterface NetworkInterface { get; }

        TransferRate DataSent { get; }

        TransferRate DataReceived { get; }

        TransferRate UploadSpeed { get; }

        TransferRate DownloadSpeed { get; }

        void AddSentData(float d);

        void AddReceivedData(float d);
    }
}