using NetworkInspector.Models.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkInspector.Models.Interfaces {
    public interface ISubject {
        void AddObserver(IObserver obs);

        void NotifyObservers(TCPPacket tcp);

        void NotifyObservers(UDPPacket udp);
    }
}