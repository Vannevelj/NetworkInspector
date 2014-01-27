using System.Net.NetworkInformation;
using NetworkInspector.Network;

namespace NetworkInspector.Models
{
    public class NetworkStatistics : INetworkStatistics
    {
        private DataTransfer _data = new DataTransfer();

        public NetworkStatistics(string name)
        {
            NetworkInterface = Utilities.GetInterfaceInformation(name);
        }

        public NetworkInterface NetworkInterface { get; set; }

        public TransferRate TotalDataSent
        {
            get { return new TransferRate(_data.TotalUploaded); }
        }


        public TransferRate TotalDataReceived
        {
            get { return new TransferRate(_data.TotalDownloaded); }
        }

        public TransferRate UploadSpeed
        {
            get { return new TransferRate(_data.LatestUploaded); }
        }

        public TransferRate DownloadSpeed
        {
            get { return new TransferRate(_data.LatestDownloaded); }
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