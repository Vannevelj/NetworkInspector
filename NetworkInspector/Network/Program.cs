using NetworkInspector.Models.Interfaces;
using NetworkInspector.Models.Packets;
using System;
using System.Threading;

namespace NetworkInspector.Network {
    public class Program : IObserver {
        public Program() {
            var instances = Utilities.GetNetworkInterfaces();
            Console.WriteLine("All available network interfaces:\n");

            for (var i = 0; i < instances.Count; i++) {
                Console.WriteLine(i + ": " + instances[i]);
            }

            var choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Selected network interface:\n" + instances[choice] + "\n\n");

            while (true) {
                var stats = Utilities.GetNetworkStatistics(instances[choice]);
                Console.WriteLine("Download speed: " + stats.DownloadSpeed + " KBytes/s");
                Console.WriteLine("Upload speed: " + stats.UploadSpeed + " KBytes/s");
                Console.WriteLine("--------------------------------------------------------------\n\n");
                Thread.Sleep(1000);
            }

            //var tracer = new PacketTracer();
            //tracer.AddObserver(this);
            //tracer.Capture();

            //Console.ReadKey();
        }

        public void IncomingPacket(TCPPacket tcp) {
            Console.WriteLine("Time received:\t\t{0}", tcp.Received.ToLongTimeString());
            Console.WriteLine("Packet type:\t\t{0}", tcp.TransportHeader.ProtocolName);
            Console.WriteLine("TTL:\t\t\t{0}", tcp.NetworkHeader.TTL);
            Console.WriteLine("Length:\t\t\t{0}", tcp.NetworkHeader.MessageLength);
            Console.WriteLine("Source IP:\t\t{0}", tcp.NetworkHeader.SourceIP);
            Console.WriteLine("Destination IP:\t\t{0}", tcp.NetworkHeader.DestIP);
            Console.WriteLine("Protocol:\t\t{0}", tcp.NetworkHeader.ProtocolType);
            Console.WriteLine("Source Port:\t\t{0}", tcp.TransportHeader.SourcePort);
            Console.WriteLine("Destination Port:\t{0}", tcp.TransportHeader.DestinationPort);
            Console.WriteLine("\n\n");
        }

        public void IncomingPacket(UDPPacket udp) {
            Console.WriteLine("udp");
            Console.WriteLine("\n\n");
        }
    }
}