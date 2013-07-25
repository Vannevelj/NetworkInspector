using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;
using System;

namespace NetworkInspector.Models.Packets {
    public class TCPPacket {
        public IHeader ApplicationHeader { get; set; }

        public TCPHeader TransportHeader { get; set; }

        public IPHeader NetworkHeader { get; set; }

        public DateTime Received { get; set; }

        public TCPPacket(IPHeader ip, TCPHeader tcp) {
            NetworkHeader = ip;
            TransportHeader = tcp;
            Received = DateTime.Now;
        }
    }
}