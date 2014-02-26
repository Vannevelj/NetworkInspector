using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;
using NetworkInspector.Extensions;
using NetworkInspector.Models.Headers.Application.DNS;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;

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

    public abstract class Packet : IDisplayable
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public abstract Protocol PacketType { get; } // TODO: what protocol are we defining here?

        public IHeader ApplicationHeader { get; set; }

        public IHeader TransportHeader { get; set; }

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
                        _log.Fatal(NetworkHeader.SourceIP);
                    }

                    else
                    {
                        _log.Warn(
                            string.Format(
                                "Could not detect packet. Source port: {0}\tDestination port: {1} Source IP: {2}",
                                tcp.SourcePort, tcp.DestinationPort, NetworkHeader.SourceIP));
                    }
                }
                    break;

                case Protocol.UDP:
                {
                    var udp = (UDPHeader) header;
                    if (udp.SourcePort == 53 || udp.DestinationPort == 53)
                    {
                        ApplicationHeader = new DNSHeader(udp.Data, udp.MessageLength - 8);
                    }

                    else
                    {
                        _log.Warn(
                            string.Format(
                                "Could not detect packet. Source port: {0}\tDestination port: {1} Source IP: {2}",
                                udp.SourcePort, udp.DestinationPort, NetworkHeader.SourceIP));
                    }
                }
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(string.Format("Time received: {0}\n", Received));
            builder.Append(string.Format("{0}\n", NetworkHeader));
            builder.Append(string.Format("{0}\n", ApplicationHeader));
            return builder.ToString();
        }

        public Dictionary<string, string> GetFieldRepresentation()
        {
            var dic = new Dictionary<string, string>
            {
                {"Received", Received.ToString()},
            };

            dic.AddRange(NetworkHeader.GetFieldRepresentation());
            dic.AddRange(TransportHeader.GetFieldRepresentation());

            // Application protocol might not be supported
            if (ApplicationHeader != null)
            {
                dic.AddRange(ApplicationHeader.GetFieldRepresentation());
            }

            return dic;
        }
    }
}