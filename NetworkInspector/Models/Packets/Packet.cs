using System;
using NetworkInspector.Models.Headers.Application.DNS;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;

namespace NetworkInspector.Models.Packets
{
    public enum Protocol
    {
        TCP,
        UDP,
        IP,
        DNS,
        HTTP,
        UNKNOWN
    }

    public abstract class Packet
    {
        public abstract Protocol PacketType { get; }

        public IHeader ApplicationHeader { get; set; }

        public IPHeader NetworkHeader { get; set; }

        public DateTime Received { get; set; }

        protected void DetectApplicationHeader(IHeader header)
        {
            switch (header.ProtocolName)
            {
                case Protocol.TCP:
                {
                    var tcp = (TCPHeader) header;
                    if (tcp.SourcePort == 53 || tcp.DestinationPort == 53)
                    {
                        ApplicationHeader = new DNSHeader(tcp.Data, tcp.MessageLength);
                    }
                    else if (tcp.SourcePort == 80 || tcp.DestinationPort == 80)
                    {
                        ApplicationHeader = new HTTPHeader(tcp.Data, tcp.MessageLength);
                    }
                }
                    break;

                case Protocol.UDP:
                {
                    var udp = (UDPHeader) header;
                    if (udp.SourcePort == 53 || udp.DestinationPort == 53)
                    {
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