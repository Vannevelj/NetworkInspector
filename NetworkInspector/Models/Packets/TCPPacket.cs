using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using System;

namespace NetworkInspector.Models.Packets {
    public class TCPPacket : Packet {
        public TCPHeader TransportHeader { get; set; }

        public override string PacketType { get { return "TCP"; } }

        public TCPPacket(IPHeader ip, TCPHeader tcp) {
            NetworkHeader = ip;
            TransportHeader = tcp;
            Received = DateTime.Now;
            DetectApplicationHeader(tcp);
        }
    }
}