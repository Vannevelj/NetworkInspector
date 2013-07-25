using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;
using System;

namespace NetworkInspector.Models.Packets {
    public class UDPPacket {
        public IHeader ApplicationHeader { get; set; }

        public UDPHeader TransportHeader { get; set; }

        public IPHeader NetworkHeader { get; set; }

        public DateTime Received { get; set; }

        public UDPPacket(IPHeader ip, UDPHeader udp) {
            NetworkHeader = ip;
            TransportHeader = udp;
            Received = DateTime.Now;
        }
    }
}