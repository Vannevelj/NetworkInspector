using NetworkInspector.Models;
using NetworkInspector.Models.Packets;
using NetworkInspector.Network.BandwidthMonitoringUtilities;
using NetworkInspector.Network.PacketTracingUtilities;
using System;
using System.Net.NetworkInformation;

namespace NetworkInspector.Network {
    public class Program : IPacketTracerObserver, IMonitorObserver {
        public Program() {
            var instances = Utilities.GetNetworkInterfaces();
            Console.WriteLine("All available network interfaces:\n");

            for (var i = 0; i < instances.Count; i++) {
                Console.WriteLine(i + ": " + instances[i]);
            }

            var choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Selected network interface:\n" + instances[choice] + "\n\n");

            Console.WriteLine("Choose an action:\n");
            Console.WriteLine("1: Packet tracing");
            Console.WriteLine("2: Bandwidth monitor");
            var action = Convert.ToInt32(Console.ReadLine());

            //var networkInterface = Utilities.GetInterfaceInformation(instances[choice]);

            //Console.WriteLine("Desc:" + networkInterface.Description);
            //Console.WriteLine("Name:" + networkInterface.Name);
            //Console.WriteLine("Operational:" + networkInterface.OperationalStatus);
            //Console.WriteLine("Speed:" + networkInterface.Speed);
            //Console.WriteLine("Type:" + networkInterface.NetworkInterfaceType);
            //Console.WriteLine("Physical:" + networkInterface.GetPhysicalAddress());
            //Console.WriteLine("B Received:" + networkInterface.GetIPv4Statistics().BytesReceived);
            //Console.WriteLine("B Sent:" + networkInterface.GetIPv4Statistics().BytesSent);
            //foreach (var y in networkInterface.GetIPProperties().DnsAddresses) {
            //    Console.WriteLine("DNS: " + y);
            //}

            //foreach (var y in networkInterface.GetIPProperties().DhcpServerAddresses) {
            //    Console.WriteLine("DHCP: " + y);
            //}

            //foreach (var y in networkInterface.GetIPProperties().GatewayAddresses) {
            //    Console.WriteLine("Gateway: " + y.Address);
            //}

            //Console.WriteLine("DNS:" + networkInterface.GetIPProperties().IsDnsEnabled);
            //Console.WriteLine("\n");

            //Console.ReadKey();

            switch (action) {
                case 1: {
                        var tracer = new PacketTracer();
                        tracer.AddObserver(this);
                        tracer.Capture();
                        Console.ReadKey();
                    }
                    break;

                case 2: {
                        var monitor = new BandwithMonitor();
                        monitor.AddObserver(this);
                        monitor.GetNetworkStatistics(instances[choice]);
                    }
                    break;

                default:
                    throw new ArgumentException("Enter a valid number");
            }
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
            if (tcp.ApplicationHeader != null) {
                Console.WriteLine("Application type:\t{0}", tcp.ApplicationHeader.ProtocolName);
            }
            Console.WriteLine("\n\n");
        }

        public void IncomingPacket(UDPPacket udp) {
            Console.WriteLine("Time received:\t\t{0}", udp.Received.ToLongTimeString());
            Console.WriteLine("Packet type:\t\t{0}", udp.TransportHeader.ProtocolName);
            Console.WriteLine("TTL:\t\t\t{0}", udp.NetworkHeader.TTL);
            Console.WriteLine("Length:\t\t\t{0}", udp.NetworkHeader.MessageLength);
            Console.WriteLine("Source IP:\t\t{0}", udp.NetworkHeader.SourceIP);
            Console.WriteLine("Destination IP:\t\t{0}", udp.NetworkHeader.DestIP);
            Console.WriteLine("Protocol:\t\t{0}", udp.NetworkHeader.ProtocolType);
            Console.WriteLine("Source Port:\t\t{0}", udp.TransportHeader.SourcePort);
            Console.WriteLine("Destination Port:\t{0}", udp.TransportHeader.DestinationPort);
            if (udp.ApplicationHeader != null) {
                Console.WriteLine("Application type:\t{0}", udp.ApplicationHeader.ProtocolName);
            }

            Console.WriteLine("\n\n");
        }

        public void TransferUpdate(IStatistics stats) {
            Console.WriteLine("Download speed:\t\t{0} KBytes/s", (stats.DownloadSpeed / 1024).ToString("0.000"));
            Console.WriteLine("Upload speed:\t\t{0} KBytes/s", (stats.UploadSpeed / 1024).ToString("0.000"));
            Console.WriteLine("Total downloaded:\t{0} Kbytes", (stats.DataReceived / 1024).ToString("0.000"));
            Console.WriteLine("Total uploaded:\t\t{0} Kbytes", (stats.DataSent / 1024).ToString("0.000"));
            Console.WriteLine("--------------------------------------------------------------\n\n");
        }
    }
}