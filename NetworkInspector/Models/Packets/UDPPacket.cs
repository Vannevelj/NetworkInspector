using System;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;

namespace NetworkInspector.Models.Packets
{
    public class UDPPacket : Packet
    {
        public override Protocol PacketType
        {
            get { return Protocol.UDP; }
        }

        public UDPHeader TransportHeader { get; set; }

        public UDPPacket(IPHeader ip, UDPHeader udp)
        {
            NetworkHeader = ip;
            TransportHeader = udp;
            Received = DateTime.Now;
            DetectApplicationHeader(udp);
        }
    }
}