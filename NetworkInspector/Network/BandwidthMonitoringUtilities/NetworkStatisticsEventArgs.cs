using System;
using NetworkInspector.Models;

namespace NetworkInspector.Network.BandwidthMonitoringUtilities
{
    public class NetworkStatisticsEventArgs : EventArgs
    {
        public INetworkStatistics NetworkStatistics { get; set; }
    }
}