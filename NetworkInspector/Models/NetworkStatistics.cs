using System.Net.NetworkInformation;
using NetworkInspector.Network;

namespace NetworkInspector.Models
{
    public class NetworkStatistics : INetworkStatistics
    {
        private readonly TransferRate _downTransferRate;
        private readonly TransferRate _upTransferRate;
        private readonly TransferRate _totalDownTransferRate;
        private readonly TransferRate _totalUpTransferRate;

        public NetworkStatistics(string name)
        {
            NetworkInterface = Utilities.GetInterfaceInformation(name);
            _downTransferRate = new TransferRate();
            _upTransferRate = new TransferRate();
            _totalDownTransferRate = new TransferRate();
            _totalUpTransferRate = new TransferRate();
        }

        public NetworkInterface NetworkInterface { get; set; }

        public TransferRate TotalDataSent
        {
            get { return _totalUpTransferRate; }
        }


        public TransferRate TotalDataReceived
        {
            get { return _totalDownTransferRate; }
        }

        public TransferRate UploadSpeed
        {
            get { return _upTransferRate; }
        }

        public TransferRate DownloadSpeed
        {
            get { return _downTransferRate; }
        }

        // <summary>
        // Adds a value to the current running statistics summary's upload list
        // </summary>
        public void AddSentData(float d)
        {
            _upTransferRate.AddDataPoint(d);
        }

        // <summary>
        // Adds a value to the current running statistics summary's download list
        // </summary>
        public void AddReceivedData(float d)
        {
            _downTransferRate.AddDataPoint(d);
        }
    }
}