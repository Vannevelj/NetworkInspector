using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Interfaces
{
    public interface IHeader : IDisplayable
    {
        Protocol ProtocolName { get; }
    }
}