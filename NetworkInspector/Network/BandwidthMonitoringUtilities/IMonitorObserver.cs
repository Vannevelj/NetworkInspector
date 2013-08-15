using NetworkInspector.Models;

namespace NetworkInspector.Network.BandwidthMonitoringUtilities {
    public interface IMonitorObserver {
        void TransferUpdate(IStatistics stats);
    }
}