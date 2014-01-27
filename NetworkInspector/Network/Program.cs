using System;
using System.Net.NetworkInformation;
using NetworkInspector.Models.Packets;
using NetworkInspector.Network.BandwidthMonitoringUtilities;
using NetworkInspector.Network.PacketTracingUtilities;

namespace NetworkInspector.Network
{
    public class Program
    {
        public Program()
        {
            var instances = Utilities.GetNetworkInterfaces();
            Console.WriteLine("All available network interfaces:\n");

            for (var i = 1; i <= instances.Count; i++)
            {
                Console.WriteLine(i + ": " + instances[i - 1]);
            }

            var choice = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine("Selected network interface:\n" + instances[choice] + "\n\n");

            Console.WriteLine("Choose an action:\n");
            Console.WriteLine("1: Packet tracing");
            Console.WriteLine("2: Bandwidth monitor");
            Console.WriteLine("3: Network information");
            var action = Convert.ToInt32(Console.ReadLine());


            switch (action)
            {
                case 1:
                {
                    var tracer = new PacketTracer();
                    tracer.OnPacketReceived += IncomingPacket;
                    tracer.Capture();
                    Console.ReadKey();
                }
                    break;

                case 2:
                {
                    var monitor = new BandwithMonitor();
                    monitor.OnUpdateNetworkStatistics += TransferUpdate;
                    monitor.UpdateNetworkStatistics(monitor.GetNetworkStatistics(instances[choice]));
                }
                    break;

                case 3:
                {
                    var networkInterface = Utilities.GetInterfaceInformation(instances[choice]);
                    DisplayNetworkInformation(networkInterface);
                }

                    break;

                default:
                    throw new ArgumentException("Enter a valid number");
            }
        }

        private void IncomingPacket(object sender, PacketTracerEventArgs e)
        {
            if (e.Packet.PacketType == Protocol.UDP)
            {
                IncomingPacket(e.Packet as UDPPacket);
            }
            else if (e.Packet.PacketType == Protocol.TCP)
            {
                IncomingPacket(e.Packet as TCPPacket);
            }
        }

        private void DisplayNetworkInformation(NetworkInterface networkInterface)
        {
            Console.WriteLine("Description:" + networkInterface.Description);
            Console.WriteLine("Name:" + networkInterface.Name);
            Console.WriteLine("Operational:" + networkInterface.OperationalStatus);
            Console.WriteLine("Speed:" + networkInterface.Speed);
            Console.WriteLine("Type:" + networkInterface.NetworkInterfaceType);
            Console.WriteLine("Physical:" + networkInterface.GetPhysicalAddress());
            Console.WriteLine("Bytes Received:" + networkInterface.GetIPv4Statistics().BytesReceived);
            Console.WriteLine("Bytes Sent:" + networkInterface.GetIPv4Statistics().BytesSent);
            foreach (var y in networkInterface.GetIPProperties().DnsAddresses)
            {
                Console.WriteLine("DNS: " + y);
            }

            foreach (var y in networkInterface.GetIPProperties().DhcpServerAddresses)
            {
                Console.WriteLine("DHCP: " + y);
            }

            foreach (var y in networkInterface.GetIPProperties().GatewayAddresses)
            {
                Console.WriteLine("Gateway: " + y.Address);
            }

            Console.WriteLine("DNS:" + networkInterface.GetIPProperties().IsDnsEnabled);
            Console.WriteLine("\n");

            Console.ReadKey();
        }

        public void IncomingPacket(TCPPacket tcp)
        {
            Console.WriteLine("Time received:\t\t{0}", tcp.Received.ToLongTimeString());
            Console.WriteLine("Packet type:\t\t{0}", tcp.TransportHeader.ProtocolName);
            Console.WriteLine("TTL:\t\t\t{0}", tcp.NetworkHeader.TTL);
            Console.WriteLine("Length:\t\t\t{0}", tcp.NetworkHeader.MessageLength);
            Console.WriteLine("Source IP:\t\t{0}", tcp.NetworkHeader.SourceIP);
            Console.WriteLine("Destination IP:\t\t{0}", tcp.NetworkHeader.DestIP);
            Console.WriteLine("Protocol:\t\t{0}", tcp.NetworkHeader.ProtocolType);
            Console.WriteLine("Source Port:\t\t{0}", tcp.TransportHeader.SourcePort);
            Console.WriteLine("Destination Port:\t{0}", tcp.TransportHeader.DestinationPort);
            if (tcp.ApplicationHeader != null)
            {
                Console.WriteLine("Application type:\t{0}", tcp.ApplicationHeader.ProtocolName);
            }
            Console.WriteLine("\n\n");
        }

        public void IncomingPacket(UDPPacket udp)
        {
            Console.WriteLine("Time received:\t\t{0}", udp.Received.ToLongTimeString());
            Console.WriteLine("Packet type:\t\t{0}", udp.TransportHeader.ProtocolName);
            Console.WriteLine("TTL:\t\t\t{0}", udp.NetworkHeader.TTL);
            Console.WriteLine("Length:\t\t\t{0}", udp.NetworkHeader.MessageLength);
            Console.WriteLine("Source IP:\t\t{0}", udp.NetworkHeader.SourceIP);
            Console.WriteLine("Destination IP:\t\t{0}", udp.NetworkHeader.DestIP);
            Console.WriteLine("Protocol:\t\t{0}", udp.NetworkHeader.ProtocolType);
            Console.WriteLine("Source Port:\t\t{0}", udp.TransportHeader.SourcePort);
            Console.WriteLine("Destination Port:\t{0}", udp.TransportHeader.DestinationPort);
            if (udp.ApplicationHeader != null)
            {
                Console.WriteLine("Application type:\t{0}", udp.ApplicationHeader.ProtocolName);
            }

            Console.WriteLine("\n\n");
        }

        public void TransferUpdate(object sender, NetworkStatisticsEventArgs stats)
        {
            Console.WriteLine("Download speed:\t\t{0} KBytes/s",
                stats.NetworkStatistics.DownloadSpeed.InKiloBytes.ToString("0.000"));
            Console.WriteLine("Upload speed:\t\t{0} KBytes/s",
                stats.NetworkStatistics.UploadSpeed.InKiloBytes.ToString("0.000"));
            Console.WriteLine("Total downloaded:\t{0} MB",
                stats.NetworkStatistics.TotalDataReceived.InMegaBytes.ToString("0.000"));
            Console.WriteLine("Total uploaded:\t\t{0} MB",
                stats.NetworkStatistics.TotalDataSent.InMegaBytes.ToString("0.000"));
            Console.WriteLine("--------------------------------------------------------------\n\n");
        }
    }
}