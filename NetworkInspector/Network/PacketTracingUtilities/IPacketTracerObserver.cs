using NetworkInspector.Models.Packets;

namespace NetworkInspector.Network.PacketTracingUtilities {
    public interface IPacketTracerObserver {
        void IncomingPacket(TCPPacket tcp);

        void IncomingPacket(UDPPacket udp);
    }
}