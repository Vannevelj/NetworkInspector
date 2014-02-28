using System;
using System.Collections.Generic;
using System.Text;
using NetworkInspector.Extensions;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Interfaces;

namespace NetworkInspector.Models.Packets
{
    public enum Protocol
    {
        TCP,
        UDP,
        IP,
        DNS,
        HTTP,
        UNKNOWN
    }

    public class Packet : IDisplayable
    {
        public Protocol PacketType { get; set; }

        public IHeader ApplicationHeader { get; set; }

        public IHeader TransportHeader { get; set; }

        public IPHeader NetworkHeader { get; set; }

        public DateTime Received { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(string.Format("Time received: {0}\n", Received));
            builder.Append(string.Format("{0}\n", NetworkHeader));
            builder.Append(string.Format("{0}\n", ApplicationHeader));
            return builder.ToString();
        }

        public Dictionary<string, string> GetFieldRepresentation()
        {
            var dic = new Dictionary<string, string>
            {
                {"Received", Received.ToString()},
            };

            dic.AddRange(NetworkHeader.GetFieldRepresentation());

            // Transport protocol might not be supported
            if (TransportHeader != null)
            {
                dic.AddRange(TransportHeader.GetFieldRepresentation());
            }

            // Application protocol might not be supported
            if (ApplicationHeader != null)
            {
                dic.AddRange(ApplicationHeader.GetFieldRepresentation());
            }

            return dic;
        }
    }
}