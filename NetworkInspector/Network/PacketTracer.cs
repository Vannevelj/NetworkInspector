using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace NetworkInspector.Network {
    public class PacketTracer : ISubject {
        private ICollection<IObserver> _observers = new Collection<IObserver>();
        private bool _running;
        private byte[] _data = new byte[4096];

        private Socket _mainSocket;

        public void Capture() {
            _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            Console.WriteLine("Socket created");
            _mainSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.121"), 0));
            Console.WriteLine("Socket bound to " + _mainSocket.LocalEndPoint);
            _mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            _running = true;
            _mainSocket.BeginReceive(_data, 0, _data.Length, SocketFlags.None, OnReceive, null);

            var byTrue = new byte[] { 1, 0, 0, 0 };
            var byOut = new byte[] { 1, 0, 0, 0 };

            _mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);
        }

        public void Stop() {
            _running = false;
            _mainSocket.Close();
        }

        private void OnReceive(IAsyncResult ar) {
            var received = _mainSocket.EndReceive((ar));
            Parse(_data, received);

            if (_running) {
                _mainSocket.BeginReceive(_data, 0, _data.Length, SocketFlags.None, OnReceive, null);
            }
        }

        private void Parse(byte[] data, int size) {
            var packet = new IPHeader(data, size);

            switch (packet.ProtocolType) {
                case ProtocolType.Udp: {
                        var udp = new UDPHeader(packet.Data, packet.MessageLength);
                        NotifyObservers(new UDPPacket(packet, udp));
                    }
                    break;

                case ProtocolType.Tcp: {
                        var tcp = new TCPHeader(packet.Data, packet.MessageLength);
                        NotifyObservers(new TCPPacket(packet, tcp));
                    }
                    break;
            }
        }

        public void AddObserver(IObserver obs) {
            _observers.Add(obs);
        }

        public void NotifyObservers(TCPPacket tcp) {
            foreach (var observer in _observers) {
                observer.IncomingPacket(tcp);
            }
        }

        public void NotifyObservers(UDPPacket udp) {
            foreach (var observer in _observers) {
                observer.IncomingPacket(udp);
            }
        }
    }
}