using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Parser
{
    internal class ParseResult
    {
        public IHeader Header { get; set; }
        public byte[] Data { get; set; }
        public int Length { get; set; }
        public Protocol UnderlyingProtocol { get; set; }
    }
}