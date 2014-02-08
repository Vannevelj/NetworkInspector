using System;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Network.PacketTracing
{
    public class PacketTracerEventArgs : EventArgs
    {
        public Packet Packet { get; set; }
    }
}