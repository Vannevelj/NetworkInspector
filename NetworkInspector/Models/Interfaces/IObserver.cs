using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Interfaces {
    public interface IObserver {
        void IncomingPacket(TCPPacket tcp);

        void IncomingPacket(UDPPacket udp);
    }
}