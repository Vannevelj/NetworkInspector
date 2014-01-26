using NetworkInspector.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NetworkInspector.Network.BandwidthMonitoringUtilities {
    public class BandwithMonitor : IMonitorSubject {
        private readonly List<IMonitorObserver> _observers = new List<IMonitorObserver>();
        private bool _running = true;


        public INetworkStatistics GetNetworkStatistics(string networkName)
        {
            return new NetworkStatistics(networkName);
        }

        // <summary>
        // Starts the monitoring of network activity
        // </summary>
        public void UpdateNetworkStatistics(INetworkStatistics stats) {
            while (_running) {
                var dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", stats.NetworkInterface.Description.Replace('(', '[').Replace(')', ']'));
                var dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", stats.NetworkInterface.Description.Replace('(', '[').Replace(')', ']'));

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

        // <summary>
        // Stops the monitoring of network activity
        // </summary>
        public void Stop() {
            _running = false;
        }

        public void AddObserver(IMonitorObserver obs) {
            _observers.Add(obs);
        }

        public void NotifyObservers(INetworkStatistics stats) {
            foreach (var obs in _observers) {
                obs.TransferUpdate(stats);
            }
        }
    }
}