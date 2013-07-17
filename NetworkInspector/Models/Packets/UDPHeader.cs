using System;
using System.IO;
using System.Net;

namespace NetworkInspector.Models.Packets {
    public class UDPHeader {
        private ushort _usSourcePort; // 16 bits

        private ushort _usDestinationPort; // 16 bits

        private ushort _usLength; // 16 bits

        private short _sChecksum; // 16 bits

        private byte[] _byData = new byte[4096];

        public UDPHeader(byte[] buffer, int size) {
            var stream = new MemoryStream(buffer, 0, size);
            var reader = new BinaryReader(stream);

            _usSourcePort = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usDestinationPort = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usLength = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _sChecksum = IPAddress.NetworkToHostOrder(reader.ReadInt16());

            Array.Copy(buffer, 8, _byData, 0, size - 8);
        }

        public int SourcePort {
            get {
                return Convert.ToInt32(_usSourcePort);
            }
        }

        public int DestinationPort {
            get {
                return Convert.ToInt32(_usDestinationPort);
            }
        }

        public int Length {
            get {
                return Convert.ToInt32(_usLength);
            }
        }

        public string Checksum {
            get {
                return string.Format("0x{0:x2}", _sChecksum);
            }
        }

        public byte[] Data {
            get {
                return _byData;
            }
        }
    }
}