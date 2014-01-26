using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Network.PacketTracingUtilities
{
    public class PacketTracerEventArgs : EventArgs
    {
        public Packet Packet { get; set; }
    }
}
