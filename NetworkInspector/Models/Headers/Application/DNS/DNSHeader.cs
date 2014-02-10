using System.IO;
using System.Net;
using NetworkInspector.Models.Headers.Interfaces;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Application.DNS
{
    public class DNSHeader : IHeader
    {
        private readonly ushort _usIdentification; // 16 bits
        private readonly ushort _usFlags; // 16 bits
        private readonly ushort _usTotalQuestions; // 16 bits
        private readonly ushort _usTotalAnswerResourceRecords; // 16 bits
        private readonly ushort _usTotalAuthorityResourceRecords; // 16 bits
        private readonly ushort _usTotalAdditionalResourceRecords; // 16 bits

        public DNSHeader(byte[] buffer, int size)
        {
            using (var stream = new MemoryStream(buffer, 0, size))
            {
                using (var reader = new BinaryReader(stream))
                {
                    _usIdentification = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usFlags = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usTotalQuestions = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usTotalAnswerResourceRecords = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usTotalAuthorityResourceRecords = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _usTotalAdditionalResourceRecords = (ushort) IPAddress.NetworkToHostOrder(reader.ReadInt16());
                }
            }
        }

        public string Identification
        {
            get { return string.Format("0x{0:x2}", _usIdentification); }
        }

        public string Flags
        {
            get { return string.Format("0x{0:x2}", _usFlags); }
        }

        public string TotalQuestions
        {
            get { return _usTotalQuestions.ToString(); }
        }

        public string TotalAnswerResourceRecords
        {
            get { return _usTotalAnswerResourceRecords.ToString(); }
        }

        public string TotalAuthorityResourceRecords
        {
            get { return _usTotalAuthorityResourceRecords.ToString(); }
        }

        public string TotalAdditionalResourceRecords
        {
            get { return _usTotalAdditionalResourceRecords.ToString(); }
        }

        public Protocol ProtocolName
        {
            get { return Protocol.DNS; }
        }
    }
}