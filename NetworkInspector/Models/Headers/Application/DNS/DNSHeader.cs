using System.Collections.Generic;
using System.IO;
using System.Net;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Application.DNS
{
    public class DNSHeader : IHeader
    {
        private readonly int _identification; // 16 bits
        private readonly int _flags; // 16 bits
        private readonly int _questionCount; // 16 bits
        private readonly int _answerRecordCount; // 16 bits
        private readonly int _authorityRecordCount; // 16 bits
        private readonly int _additionalRecordCount; // 16 bits
        private readonly string _data;

        public DNSHeader(byte[] buffer, int size)
        {
            _data = System.Text.Encoding.Default.GetString(buffer);

            using (var stream = new MemoryStream(buffer, 0, size))
            {
                using (var reader = new BinaryReader(stream))
                {
                    _identification = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _flags = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _questionCount = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _answerRecordCount = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _authorityRecordCount = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                    _additionalRecordCount = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                }
            }
        }

        public string Identification
        {
            get { return string.Format("0x{0:x2}", _identification); }
        }

        public string Flags
        {
            get { return string.Format("0x{0:x2}", _flags); }
        }

        public int QuestionCount
        {
            get { return _questionCount; }
        }

        public int AnswerRecordCount
        {
            get { return _answerRecordCount; }
        }

        public int AuthorityRecordCount
        {
            get { return _authorityRecordCount; }
        }

        public int AdditionalRecordCount
        {
            get { return _additionalRecordCount; }
        }

        public Protocol ProtocolName
        {
            get { return Protocol.DNS; }
        }

        public Dictionary<string, string> GetFieldRepresentation()
        {
            return new Dictionary<string, string>
            {
                {"Application Header Protocol", ProtocolName.ToString()},
                {"Identification", Identification},
                {"Flags", string.Join(",", Flags)},
                {"Total Questions", QuestionCount.ToString()},
                {"Total Answer Resource Records", AnswerRecordCount.ToString()},
                {"Total Additional Resource Records", AdditionalRecordCount.ToString()},
                {"Total Authority Resource Records", AuthorityRecordCount.ToString()},
                {"Raw data", _data}
            };
        }
    }
}