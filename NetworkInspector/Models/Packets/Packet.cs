using NetworkInspector.Models.Headers.Application;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using System;

namespace NetworkInspector.Models.Packets {
    public abstract class Packet {
        public abstract string PacketType { get; }

        public IHeader ApplicationHeader { get; set; }

        public IPHeader NetworkHeader { get; set; }

        public DateTime Received { get; set; }

        protected void DetectApplicationHeader(IHeader header) {
            switch (header.ProtocolName) {
                case "TCP": {
                        var tcp = (TCPHeader) header;
                        if (tcp.SourcePort == 53 || tcp.DestinationPort == 53) {
                            ApplicationHeader = new DNSHeader(tcp.Data, tcp.MessageLength);
                        }
                    }
                    break;

                case "UDP": {
                        var udp = (UDPHeader) header;
                        if (udp.SourcePort == 53 || udp.DestinationPort == 53) {
                            ApplicationHeader = new DNSHeader(udp.Data, udp.Length - 8);
                        }
                    }
                    break;

                default:
                    throw new ArgumentException();
            }
        }
    }
}