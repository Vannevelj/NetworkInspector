﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Network
{
    public class IPHeader : IHeader, IDisplayable
    {
        private readonly byte _byVersionAndHeaderLength; // 8 bits

        private readonly byte _byDiffServices; // 8 bits

        private readonly ushort _usTotalLength; // 16 bits

        private readonly ushort _usId; // 16 bits

        private readonly ushort _usFlagsAndOffset; // 16 bits

        private readonly byte _byTTL; // 8 bits

        private readonly byte _byProtocol; // 8 bits

        private readonly short _sChecksum; // 16 bits

        private readonly uint _uiSourceIP; // 32 bits

        private readonly uint _uiDestIP; // 32 bits

        private readonly byte _byHeaderLength;

        private readonly byte[] _byData = new byte[4096];

        public IPHeader(byte[] buffer, int size)
        {
            var stream = new MemoryStream(buffer, 0, size);
            var reader = new BinaryReader(stream);

            _byVersionAndHeaderLength = reader.ReadByte();
            _byDiffServices = reader.ReadByte();

            _usTotalLength = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usId = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usFlagsAndOffset = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());

            _byTTL = reader.ReadByte();
            _byProtocol = reader.ReadByte();
            _sChecksum = IPAddress.NetworkToHostOrder(reader.ReadInt16());

            _uiSourceIP = (uint) (reader.ReadInt32());
            _uiDestIP = (uint) (reader.ReadInt32());

            _byHeaderLength = _byVersionAndHeaderLength;

            _byHeaderLength <<= 4;
            _byHeaderLength >>= 4;
            _byHeaderLength *= 4;

            Array.Copy(buffer, _byHeaderLength, _byData, 0, _usTotalLength - _byHeaderLength);
        }

        public string Version
        {
            get
            {
                if ((_byVersionAndHeaderLength >> 4) == 4)
                {
                    return "IPv4";
                }
                return (_byVersionAndHeaderLength >> 4) == 6 ? "IPv6" : "Unknown";
            }
        }

        public string HeaderLength
        {
            get { return _byHeaderLength.ToString(); }
        }

        public ushort MessageLength
        {
            get { return (ushort) (_usTotalLength - _byHeaderLength); }
        }

        public string TTL
        {
            get { return _byTTL.ToString(); }
        }

        public string DifferentiatedServices
        {
            get { return string.Format("0x{0:x2} ({1})", _byDiffServices, _byDiffServices); }
        }

        public string Flags
        {
            get
            {
                var flags = _usFlagsAndOffset >> 13;
                if (flags == 2)
                {
                    return "Not fragmented";
                }
                return flags == 1 ? "Multiple fragments" : flags.ToString();
            }
        }

        public string FragmentationOffset
        {
            get
            {
                var offset = _usFlagsAndOffset << 3;
                offset >>= 3;

                return offset.ToString();
            }
        }

        public Protocol UnderlyingProtocol
        {
            get
            {
                if (_byProtocol == 6)
                {
                    return Protocol.TCP;
                }
                return _byProtocol == 17 ? Protocol.UDP : Protocol.UNKNOWN;
            }
        }

        public string Checksum
        {
            get { return string.Format("0x{0:x2}", _sChecksum); }
        }

        public IPAddress SourceIP
        {
            get { return new IPAddress(_uiSourceIP); }
        }

        public IPAddress DestinationIP
        {
            get { return new IPAddress(_uiDestIP); }
        }

        public string TotalLength
        {
            get { return _usTotalLength.ToString(); }
        }

        public string Identification
        {
            get { return _usId.ToString(); }
        }

        public byte[] Data
        {
            get { return _byData; }
        }

        public Protocol ProtocolName
        {
            get { return Protocol.IP; }
        }

        public Dictionary<string, string> GetFieldRepresentation()
        {
            return new Dictionary<string, string>
            {
                {"Network Layer Protocol", ProtocolName.ToString()},
                {"Version", Version},
                {"Source IP", SourceIP.ToString()},
                {"Destination IP", DestinationIP.ToString()},
                {"TTL", TTL},
                {"Checksum", Checksum},
                {"Differentiated Services", DifferentiatedServices},
                {"Flags", Flags},
                {"Fragmentation offset", FragmentationOffset},
                {"IP header length", HeaderLength},
                {"IP message length", MessageLength.ToString()},
                {"Total length", TotalLength},
                {"Identification", Identification},
            };
        }
    }
}