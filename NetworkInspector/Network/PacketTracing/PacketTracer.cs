using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using log4net;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Headers.Network;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Network.PacketTracing
{
    public class PacketTracer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public event EventHandler<PacketTracerEventArgs> OnPacketReceived;

        private bool _running = true;
        private readonly byte[] _data = new byte[4096];

        private Socket _mainSocket;

        public void Capture()
        {
            _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            _log.Info("Socket created");
            _mainSocket.Bind(new IPEndPoint(Utilities.GetLocalIP(), 0));
            _log.Info(string.Format("Socket bound to {0}", _mainSocket.LocalEndPoint));

            _mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            _running = true;
            _log.Info("Packet tracing started");
            _mainSocket.BeginReceive(_data, 0, _data.Length, SocketFlags.None, OnReceive, null);

            //http://stackoverflow.com/questions/9440130/socket-iocontrol-ambiguous-documentation

            var byTrue = new byte[] {1, 0, 0, 0};
            var byOut = new byte[] {1, 0, 0, 0};

            _mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);
        }

        public void Stop()
        {
            _log.Info("Packet tracing stopping");
            _running = false;
            _mainSocket.Shutdown(SocketShutdown.Both);

            // Disposing of any remaining data that might've stayed behind
            // http://vadmyst.blogspot.be/2008/04/proper-way-to-close-tcp-socket.html
            try
            {
                var read = 0;
                while ((read = _mainSocket.Receive(_data)) > 0)
                {
                }
            }
            catch
            {
                //ignore
            }

            _mainSocket.Close();
            _mainSocket.Dispose();

            _log.Info("Packet tracing stopped");
        }

        private void OnReceive(IAsyncResult ar)
        {
            if (_running)
            {
                SocketError error;
                var received = _mainSocket.EndReceive(ar, out error);

                if (error != SocketError.Success)
                {
                    _log.Warn(string.Format("Socket Error:\t{0}", error));
                }
                else
                {
                    Parse(_data, received);
                }

                _mainSocket.BeginReceive(_data, 0, _data.Length, SocketFlags.None, OnReceive, null);
            }
        }

        private void Parse(byte[] data, int size)
        {
            var packet = new IPHeader(data, size);

            switch (packet.ProtocolType)
            {
                case Protocol.UDP:
                {
                    var udp = new UDPHeader(packet.Data, packet.MessageLength);
                    NotifyObservers(new UDPPacket(packet, udp));
                }
                    break;

                case Protocol.TCP:
                {
                    var tcp = new TCPHeader(packet.Data, packet.MessageLength);
                    NotifyObservers(new TCPPacket(packet, tcp));
                }
                    break;
            }
        }

        private void NotifyObservers(Packet p)
        {
            if (p.ApplicationHeader is HTTPHeader)
            {
                var http = p.ApplicationHeader as HTTPHeader;
                if (string.IsNullOrEmpty(http.Host))
                {
                    return;
                }
            }

            var handler = OnPacketReceived;

            if (handler != null)
            {
                handler.Invoke(this, new PacketTracerEventArgs {Packet = p});
            }
        }
    }
}