using NetworkInspector.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NetworkInspector.Network.BandwidthMonitoringUtilities {
    public class BandwithMonitor : IMonitorSubject {
        private List<IMonitorObserver> _observers = new List<IMonitorObserver>();
        private IDictionary<string, Statistics> _interfaces = new Dictionary<string, Statistics>();
        private bool running = true;

        public BandwithMonitor() {
            foreach (var _interface in Utilities.GetNetworkInterfaces()) {
                _interfaces.Add(_interface, new Statistics(_interface));
            }
        }

        // <summary>
        // Starts the monitoring of network activity
        // </summary>
        public void GetNetworkStatistics(string interfaceName) {
            while (running) {
                var networkStats = _interfaces[interfaceName];

                var dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", interfaceName);
                var dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", interfaceName);

                var initSent = dataSentCounter.NextValue();
                var initReceived = dataReceivedCounter.NextValue();
                Thread.Sleep(1000);

                var sentSum = dataSentCounter.NextValue() - initSent;
                var receiveSum = dataReceivedCounter.NextValue() - initReceived;

                networkStats.AddSentData(sentSum);
                networkStats.AddReceivedData(receiveSum);

                NotifyObservers(networkStats);
            }
        }

        // <summary>
        // Stops the monitoring of network activity
        // </summary>
        public bool Stop() {
            return running = true;
        }

        public void AddObserver(IMonitorObserver obs) {
            _observers.Add(obs);
        }

        public void NotifyObservers(IStatistics stats) {
            foreach (var obs in _observers) {
                obs.TransferUpdate(stats);
            }
        }
    }
}