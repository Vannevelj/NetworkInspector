using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Transport
{
    public class UDPHeader : IHeader
    {
        private readonly ushort _usSourcePort; // 16 bits

        private readonly ushort _usDestinationPort; // 16 bits

        private readonly ushort _usLength; // 16 bits

        private readonly short _sChecksum; // 16 bits

        private readonly byte[] _byData = new byte[4096];

        public UDPHeader(byte[] buffer, int size)
        {
            using (var stream = new MemoryStream(buffer, 0, size))
            {
                using (var reader = new BinaryReader(stream))
                {
                    _usSourcePort = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usDestinationPort = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usLength = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _sChecksum = IPAddress.NetworkToHostOrder(reader.ReadInt16());

                    Array.Copy(buffer, 8, _byData, 0, size - 8);
                }
            }
        }

        public int SourcePort
        {
            get { return Convert.ToInt32(_usSourcePort); }
        }

        public int DestinationPort
        {
            get { return Convert.ToInt32(_usDestinationPort); }
        }

        public int MessageLength
        {
            get { return Convert.ToInt32(_usLength); }
        }

        public string Checksum
        {
            get { return string.Format("0x{0:x2}", _sChecksum); }
        }

        public byte[] Data
        {
            get { return _byData; }
        }

        public Protocol ProtocolName
        {
            get { return Protocol.UDP; }
        }

        public override string ToString()
        {
            return string.Format("UDP - Source: {0} - Destination: {1} - MessageLength: {2}", SourcePort, DestinationPort,
                MessageLength);
        }

        public Dictionary<string, string> GetFieldRepresentation()
        {
            return new Dictionary<string, string>()
            {
                {"Transport Header Protocol ", ProtocolName.ToString()},
                {"Source port", SourcePort.ToString()},
                {"Destination port", DestinationPort.ToString()},
                {"Message length", MessageLength.ToString()},
                {"Checksum", Checksum},
                //{"TEST: DATA", System.Text.Encoding.Default.GetString(Data)}
            };
        }
    }
}