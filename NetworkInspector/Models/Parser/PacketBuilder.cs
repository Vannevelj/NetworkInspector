using System;
using NetworkInspector.Models.Headers.Application.DNS;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Parser
{
    internal class PacketBuilder
    {
        private IHeader _networkLayer;
        private IHeader _transportLayer;
        private IHeader _applicationLayer;

        internal Packet Parse(byte[] data, int length)
        {
            // Parse network layer
            var networkResult = ParseNetworkHeader(data, length);
            _networkLayer = networkResult.Header;

            // Parse transport layer
            var transportResult = ParseTransportHeader(networkResult.Data, networkResult.Length, networkResult.UnderlyingProtocol);
            _transportLayer = transportResult.Header;

            // Parse application layer
            var applicationResult = ParseApplicationHeader(transportResult.Data, transportResult.Length, transportResult.UnderlyingProtocol);
            _applicationLayer = applicationResult.Header;

            if (_transportLayer.ProtocolName == Protocol.TCP)
            {
                return new TCPPacket
                {
                    NetworkHeader = _networkLayer as IPHeader,
                    TransportHeader = _transportLayer,
                    ApplicationHeader = _applicationLayer,
                    Received = DateTime.Now
                };
            }

            if (_transportLayer.ProtocolName == Protocol.UDP)
            {
                return new UDPPacket
                {
                    NetworkHeader = _networkLayer as IPHeader,
                    TransportHeader = _transportLayer,
                    ApplicationHeader = _applicationLayer,
                    Received = DateTime.Now
                };
            }

            return null;
        }

        private ParseResult ParseNetworkHeader(byte[] data, int length)
        {
            var header  = new IPHeader(data, length);
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
                Protocol prot = Protocol.UNKNOWN;

                if (header.SourcePort == 53 || header.DestinationPort == 53)
                {
                    prot = Protocol.DNS;
                }

                if (header.SourcePort == 80 || header.DestinationPort == 80)
                {
                    prot = Protocol.HTTP;
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
                Protocol prot = Protocol.UNKNOWN;

                if (header.SourcePort == 53 || header.DestinationPort == 53)
                {
                    prot = Protocol.DNS;
                }

                if (header.SourcePort == 80 || header.DestinationPort == 80)
                {
                    prot = Protocol.HTTP;
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
                if (_transportLayer.ProtocolName == Protocol.HTTP)
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
