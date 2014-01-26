using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using NetworkInspector.Network;

namespace NetworkInspector.Models {
    public class NetworkStatistics : INetworkStatistics
    {
        private readonly TransferRate _downTransferRate;
        private readonly TransferRate _upTransferRate;
        private readonly TransferRate _totalDownTransferRate;
        private readonly TransferRate _totalUpTransferRate;

        public NetworkStatistics(string name) {
            NetworkInterface = Utilities.GetInterfaceInformation(name);
            _downTransferRate = new TransferRate();
            _upTransferRate = new TransferRate();
            _totalDownTransferRate = new TransferRate();
            _totalUpTransferRate = new TransferRate();
        }

        // <summary>
        // Holds the name of the selected network interface
        // </summary>
        public NetworkInterface NetworkInterface { get; set; }

        // <summary>
        // Contains the total data sent since the start of capturing in bytes
        // </summary>
        public TransferRate DataSent
        {
            get { return _totalUpTransferRate; }
        }

        // <summary>
        // Contains the total data received since the start of capturing in bytes
        // </summary>
        public TransferRate DataReceived
        {
            get { return _totalDownTransferRate; }
        }

        // <summary>
        // Returns the upload speed in Bytes / Second
        // </summary>
        public TransferRate UploadSpeed
        {
            get { return _upTransferRate; }
        }

        // <summary>
        // Returns the download speed in Bytes / Second
        // </summary>
        public TransferRate DownloadSpeed
        {
            get { return _downTransferRate; }
        }

        // <summary>
        // Adds a value to the current running statistics summary's upload list
        // </summary>
        public void AddSentData(float d) {
            _upTransferRate.AddDataPoint(d);
        }

        // <summary>
        // Adds a value to the current running statistics summary's download list
        // </summary>
        public void AddReceivedData(float d) {
            _downTransferRate.AddDataPoint(d);
        }
    }
}