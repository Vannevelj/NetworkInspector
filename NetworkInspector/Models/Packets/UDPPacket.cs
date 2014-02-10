using System;
using System.Collections.Generic;
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

        public override Dictionary<string, string> GetFieldRepresentation()
        {
            return new Dictionary<string, string>();
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