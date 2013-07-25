using NetworkInspector.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace NetworkInspector.Models.Headers.Transport {
    public class TCPHeader : IHeader {
        private ushort _usSourcePort;  // 16 bits

        private ushort _usDestinationPort; // 16 bits

        private uint _uiSequenceNumber = 555; // 32 bits

        private uint _uiAckNumber = 555; // 32 bits

        private ushort _usDataOffsetAndFlags = 555; // 16 bits

        private ushort _usWindow = 555; // 16 bits

        private short _sChecksum = 555; // 16 bits, can be negative

        private ushort _usUrgPointer; // 16 bits

        private byte _byHeaderLength;

        private ushort _usMessageLength;

        private byte[] _byTCPData = new byte[4096];

        public TCPHeader(byte[] buffer, int size) {
            var mem = new MemoryStream(buffer, 0, size);
            var reader = new BinaryReader(mem);

            _usSourcePort = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usDestinationPort = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _uiSequenceNumber = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt32());
            _uiAckNumber = (uint) IPAddress.NetworkToHostOrder(reader.ReadInt32());
            _usDataOffsetAndFlags = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usWindow = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _sChecksum = IPAddress.NetworkToHostOrder(reader.ReadInt16());
            _usUrgPointer = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());

            _byHeaderLength = (byte) (_usDataOffsetAndFlags >> 12);
            _byHeaderLength *= 4;

            _usMessageLength = (ushort) (size - _byHeaderLength);

            Array.Copy(buffer, _byHeaderLength, _byTCPData, 0, size - _byHeaderLength);
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

        public int SequenceNumber {
            get {
                return Convert.ToInt32(_uiSequenceNumber);
            }
        }

        public int AcknowledgementNumber {
            get {
                return (_usDataOffsetAndFlags & 0x10) != 0 ? Convert.ToInt32(_uiAckNumber) : 0;
            }
        }

        public int HeaderLength {
            get {
                return Convert.ToInt32(_byHeaderLength);
            }
        }

        public int WindowSize {
            get {
                return Convert.ToInt32(_usWindow);
            }
        }

        public int UrgentPointer {
            get {
                return (_usDataOffsetAndFlags & 0x20) != 0 ? Convert.ToInt32(_usUrgPointer) : 0;
            }
        }

        public ICollection<String> Flags {
            get {
                var flags = new List<string>();
                var nFlags = _usDataOffsetAndFlags & 0x3F;

                if ((nFlags & 0x01) != 0) {
                    flags.Add("FIN");
                }

                if ((nFlags & 0x02) != 0) {
                    flags.Add("SYN");
                }

                if ((nFlags & 0x04) != 0) {
                    flags.Add("RST");
                }

                if ((nFlags & 0x08) != 0) {
                    flags.Add("PSH");
                }

                if ((nFlags & 0x10) != 0) {
                    flags.Add("ACK");
                }

                if ((nFlags & 0x20) != 0) {
                    flags.Add("URG");
                }

                return flags;
            }
        }

        public string Checksum {
            get {
                return string.Format("0x{0:x2}", _sChecksum);
            }
        }

        public byte[] Data {
            get {
                return _byTCPData;
            }
        }

        public int MessageLength {
            get {
                return Convert.ToInt32(_usMessageLength);
            }
        }

        public string ProtocolName {
            get {
                return "TCP";
            }
        }
    }
}