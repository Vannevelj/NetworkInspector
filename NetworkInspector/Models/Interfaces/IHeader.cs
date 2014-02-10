using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Interfaces
{
    public interface IHeader
    {
        Protocol ProtocolName { get; }
    }
}