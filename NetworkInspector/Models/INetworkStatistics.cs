using System.Net.NetworkInformation;

namespace NetworkInspector.Models
{
    public interface INetworkStatistics
    {
        NetworkInterface NetworkInterface { get; }

        TransferRate TotalDataSent { get; }

        TransferRate TotalDataReceived { get; }

        TransferRate UploadSpeed { get; }

        TransferRate DownloadSpeed { get; }

        void AddSentData(float d);

        void AddReceivedData(float d);
    }
}