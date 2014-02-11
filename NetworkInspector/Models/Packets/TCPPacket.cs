using System;
using System.Collections.Generic;
using NetworkInspector.Extensions;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;

namespace NetworkInspector.Models.Packets
{
    public class TCPPacket : Packet
    {
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