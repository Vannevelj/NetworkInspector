using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using System;

namespace NetworkInspector.Models.Packets {
    public class UDPPacket : Packet {
        public override string PacketType {
            get { return "UDP"; }
        }

        public UDPHeader TransportHeader { get; set; }

        public UDPPacket(IPHeader ip, UDPHeader udp) {
            NetworkHeader = ip;
            TransportHeader = udp;
            Received = DateTime.Now;
            DetectApplicationHeader(udp);
        }
    }
}