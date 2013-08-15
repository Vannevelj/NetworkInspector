using NetworkInspector.Models;

namespace NetworkInspector.Network.BandwidthMonitoringUtilities {
    public interface IMonitorSubject {
        void AddObserver(IMonitorObserver obs);

        void NotifyObservers(IStatistics stats);
    }
}