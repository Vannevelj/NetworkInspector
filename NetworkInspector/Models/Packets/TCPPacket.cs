using System;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;

namespace NetworkInspector.Models.Packets
{
    public class TCPPacket : Packet
    {
        public TCPHeader TransportHeader { get; set; }

        public override Protocol PacketType
        {
            get { return Protocol.TCP; }
        }

        public TCPPacket(IPHeader ip, TCPHeader tcp)
        {
            NetworkHeader = ip;
            TransportHeader = tcp;
            Received = DateTime.Now;
            DetectApplicationHeader(tcp);
        }
    }
}