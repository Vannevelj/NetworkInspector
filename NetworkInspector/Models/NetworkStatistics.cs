using System.Net.NetworkInformation;
using NetworkInspector.Network;

namespace NetworkInspector.Models
{
    public class NetworkStatistics : INetworkStatistics
    {
        private readonly DataTransfer _data = new DataTransfer();
        private readonly TransferRate _latestUpload = new TransferRate();
        private readonly TransferRate _latestDownload = new TransferRate();
        private readonly TransferRate _totalUpload = new TransferRate();
        private readonly TransferRate _totalDownload = new TransferRate();


        public NetworkStatistics(string name)
        {
            NetworkInterface = Utilities.GetInterfaceInformation(name);
        }

        public NetworkInterface NetworkInterface { get; set; }

        public TransferRate TotalDataSent
        {
            get { return _totalUpload.Update(_data.TotalUploaded); }
        }


        public TransferRate TotalDataReceived
        {
            get { return _totalDownload.Update(_data.TotalDownloaded); }
        }

        public TransferRate UploadSpeed
        {
            get { return _latestUpload.Update(_data.LatestUploaded); }
        }

        public TransferRate DownloadSpeed
        {
            get { return _latestDownload.Update(_data.LatestDownloaded); }
        }

        // <summary>
        // Adds a value to the current running statistics summary's upload list
        // </summary>
        public void AddSentData(float d)
        {
            _data.Send(d);
        }

        // <summary>
        // Adds a value to the current running statistics summary's download list
        // </summary>
        public void AddReceivedData(float d)
        {
            _data.Receive(d);
        }
    }
}