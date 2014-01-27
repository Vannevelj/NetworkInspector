using System;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Network.PacketTracingUtilities
{
    public class PacketTracerEventArgs : EventArgs
    {
        public Packet Packet { get; set; }
    }
}