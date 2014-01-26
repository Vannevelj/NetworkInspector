using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Transport
{
    public interface IHeader
    {
        Protocol ProtocolName { get; }
    }
}