using NetworkInspector.Models.Packets;

namespace NetworkInspector.Network.PacketTracingUtilities {
    public interface IPacketTracerSubject {
        void AddObserver(IPacketTracerObserver obs);

        void NotifyObservers(TCPPacket tcp);

        void NotifyObservers(UDPPacket udp);
    }
}