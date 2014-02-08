using System;
using System.Diagnostics;
using System.Threading;
using NetworkInspector.Extensions;
using NetworkInspector.Models;

namespace NetworkInspector.Network.BandwidthMonitoring
{
    public class BandwidthMonitor
    {
        public event EventHandler<NetworkStatisticsEventArgs> OnUpdateNetworkStatistics;
        private bool _running = true;


        public INetworkStatistics GetNetworkStatistics(string networkName)
        {
            return new NetworkStatistics(networkName);
        }

        // <summary>
        // Starts the monitoring of network activity
        // </summary>
        public void UpdateNetworkStatistics(INetworkStatistics stats)
        {
            while (_running)
            {
                var dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec",
                    stats.NetworkInterface.Description.ReplaceOptionalBrackets());
                var dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec",
                    stats.NetworkInterface.Description.ReplaceOptionalBrackets());

                var initSent = dataSentCounter.NextValue();
                var initReceived = dataReceivedCounter.NextValue();
                Thread.Sleep(1000);

                var sentSum = dataSentCounter.NextValue() - initSent;
                var receiveSum = dataReceivedCounter.NextValue() - initReceived;

                stats.AddSentData(sentSum);
                stats.AddReceivedData(receiveSum);

                NotifyObservers(stats);
            }
        }

        private void NotifyObservers(INetworkStatistics stats)
        {
            var handler = OnUpdateNetworkStatistics;

            if (handler != null)
            {
                handler(this, new NetworkStatisticsEventArgs {NetworkStatistics = stats});
            }
        }

        // <summary>
        // Stops the monitoring of network activity
        // </summary>
        public void Stop()
        {
            _running = false;
        }
    }
}