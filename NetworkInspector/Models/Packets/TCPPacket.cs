using System;
using System.Collections.Generic;
using NetworkInspector.Extensions;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;

namespace NetworkInspector.Models.Packets
{
    public class TCPPacket : Packet, IDisplayable
    {
        public TCPHeader TransportHeader { get; set; }

        public override Protocol PacketType
        {
            get { return Protocol.TCP; }
        }

        public override Dictionary<string, string> GetFieldRepresentation()
        {
            var dic = new Dictionary<string, string>()
            {
               {"Received", Received.ToString()},
               {"Transport Layer Protocol", "TCP"},  
            };  
      
            dic.AddRange(NetworkHeader.GetFieldRepresentation());

            return dic;
        }

        public TCPPacket(IPHeader ip, TCPHeader tcp)
        {
            NetworkHeader = ip;
            TransportHeader = tcp;
            Received = DateTime.Now;
            DetectApplicationHeader(tcp);
        }
    }
}