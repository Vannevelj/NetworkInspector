using System;
using System.Reflection;
using log4net;
using NetworkInspector.Models.Headers.Application.DNS;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Parser
{
    public class PacketBuilder
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IHeader _networkLayer;
        private IHeader _transportLayer;
        private IHeader _applicationLayer;

        public Packet Parse(byte[] data, int length)
        {
            // Parse network layer
            var networkResult = ParseNetworkHeader(data, length);
            _networkLayer = networkResult.Header;

            // Parse transport layer
            var transportResult = ParseTransportHeader(networkResult.Data, networkResult.Length,
                networkResult.UnderlyingProtocol);

            // Do not continue when we don't know the packet type
            if (transportResult == null)
            {
                return null;
            }
            _transportLayer = transportResult.Header;

            // Parse application layer
            var applicationResult = ParseApplicationHeader(transportResult.Data, transportResult.Length,
                transportResult.UnderlyingProtocol);
            if (applicationResult != null)
            {
                _applicationLayer = applicationResult.Header;
            }

            return new Packet
            {
                PacketType = _transportLayer.ProtocolName,
                NetworkHeader = _networkLayer as IPHeader,
                TransportHeader = _transportLayer,
                ApplicationHeader = _applicationLayer,
                Received = DateTime.Now
            };
        }

        private ParseResult ParseNetworkHeader(byte[] data, int length)
        {
            var header = new IPHeader(data, length);
            return new ParseResult
            {
                Header = header,
                Data = header.Data,
                Length = header.MessageLength,
                UnderlyingProtocol = header.UnderlyingProtocol
            };
        }

        private ParseResult ParseTransportHeader(byte[] data, int length, Protocol protocol)
        {
            if (protocol == Protocol.TCP)
            {
                var header = new TCPHeader(data, length);
                var prot = Protocol.UNKNOWN;

                if (header.SourcePort == 80 || header.DestinationPort == 80)
                {
                    prot = Protocol.HTTP;
                }

                else
                {
                    _log.Warn(
                        string.Format(
                            "Could not detect application header. Source port: {0}\tDestination port: {1}",
                            header.SourcePort, header.DestinationPort));
                }

                return new ParseResult
                {
                    Header = header,
                    Data = header.Data,
                    Length = header.MessageLength,
                    UnderlyingProtocol = prot
                };
            }

            if (protocol == Protocol.UDP)
            {
                var header = new UDPHeader(data, length);
                var prot = Protocol.UNKNOWN;

                _log.Debug(string.Format("UDP PACKETS SP: {0}\tDP: {1}", header.SourcePort, header.DestinationPort));

                if (header.SourcePort == 53 || header.DestinationPort == 53)
                {
                    prot = Protocol.DNS;
                }
                else
                {
                    _log.Warn(
                        string.Format(
                            "Could not detect application header. Source port: {0}\tDestination port: {1}",
                            header.SourcePort, header.DestinationPort));
                }

                return new ParseResult
                {
                    Header = header,
                    Data = header.Data,
                    Length = header.MessageLength,
                    UnderlyingProtocol = prot
                };
            }

            return null;
        }

        private ParseResult ParseApplicationHeader(byte[] data, int length, Protocol protocol)
        {
            if (protocol == Protocol.UNKNOWN)
            {
                return null;
            }

            if (protocol == Protocol.DNS)
            {
                IHeader header = null;
                if (_transportLayer.ProtocolName == Protocol.TCP)
                {
                    header = new DNSHeader(data, length);
                }

                if (_transportLayer.ProtocolName == Protocol.UDP)
                {
                    header = new DNSHeader(data, length - 8);
                }

                return new ParseResult
                {
                    Header = header
                };
            }

            if (protocol == Protocol.HTTP)
            {
                if (_transportLayer.ProtocolName == Protocol.TCP)
                {
                    var header = new HTTPHeader(data, length);
                    return new ParseResult
                    {
                        Header = header
                    };
                }
            }

            return null;
        }
    }
}