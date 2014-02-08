using System;
using NetworkInspector.Models;

namespace NetworkInspector.Network.BandwidthMonitoring
{
    public class NetworkStatisticsEventArgs : EventArgs
    {
        public INetworkStatistics NetworkStatistics { get; set; }
    }
}