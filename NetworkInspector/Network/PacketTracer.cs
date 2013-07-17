using NetworkInspector.Models;
using NetworkInspector.Models.Packets;
using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkInspector.Network {
    public class PacketTracer {
        private bool running;
        private byte[] _data = new byte[4096];

        private Socket _mainSocket;

        public void Capture() {
            _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            Console.WriteLine("Socket created");
            _mainSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.121"), 0));
            Console.WriteLine("Socket bound to " + _mainSocket.LocalEndPoint);
            _mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            running = true;
            _mainSocket.BeginReceive(_data, 0, _data.Length, SocketFlags.None, OnReceive, null);

            var byTrue = new byte[] { 1, 0, 0, 0 };
            var byOut = new byte[] { 1, 0, 0, 0 };

            _mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);
        }

        public void Stop() {
            running = false;
            _mainSocket.Close();
        }

        private void OnReceive(IAsyncResult ar) {
            var received = _mainSocket.EndReceive((ar));
            Parse(_data, received);

            if (running) {
                _mainSocket.BeginReceive(_data, 0, _data.Length, SocketFlags.None, OnReceive, null);
            }
        }

        private void Parse(byte[] data, int size) {
            var packet = new IPHeader(data, size);

            Console.WriteLine("TTL:\t{0}", packet.TTL);
            Console.WriteLine("Length:\t{0}", packet.MessageLength);
            Console.WriteLine("Source IP:\t{0}", packet.SourceIP);
            Console.WriteLine("Destination IP:\t{0}", packet.DestIP);
            Console.WriteLine("Protocol:\t{0}", packet.ProtocolType);

            if (packet.ProtocolType == ProtocolType.Tcp) {
                var tcp = new TCPHeader(packet.Data, packet.MessageLength);
                Console.WriteLine("Source Port:\t{0}", tcp.SourcePort);
                Console.WriteLine("Destination Port:\t{0}", tcp.DestinationPort);
            }

            if (packet.ProtocolType == ProtocolType.Udp) {
                var udp = new UDPHeader(packet.Data, packet.MessageLength);
                Console.WriteLine("Source Port:\t{0}", udp.SourcePort);
                Console.WriteLine("Destination Port:\t{0}", udp.DestinationPort);
            }
            Console.WriteLine("---------\n");
        }
    }
}