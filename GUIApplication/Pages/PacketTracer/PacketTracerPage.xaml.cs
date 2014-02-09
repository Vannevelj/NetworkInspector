using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NetworkInspector.Models.Packets;
using NetworkInspector.Network.PacketTracing;

namespace GUIApplication.Pages.PacketTracer
{
    /// <summary>
    /// Interaction logic for PacketTracerPage.xaml
    /// </summary>
    public partial class PacketTracerPage : Page
    {
        private NetworkInspector.Network.PacketTracing.PacketTracer _tracer;
        private Thread _tracerThread;
        private int _packetsSent;

        private const int MaxPackets = 4096;
        private readonly Packet[] _packets = new Packet[MaxPackets]; // Most recent packets
        private int _selectedPacket, _amountOfPackets;
 

        public PacketTracerPage()
        {
            InitializeComponent();

            // Display the standard text in the combobox
            NetworkInterfaceComboBox.SelectedIndex = 0;

            // UI Event handlers
            PacketList.SelectionChanged += PacketList_ItemSelected;
        }


        private void PacketList_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedPacket = PacketList.SelectedIndex;
            PacketDetails.Text = _packets[_selectedPacket].ToString();
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _tracer = new NetworkInspector.Network.PacketTracing.PacketTracer();
            _tracer.OnPacketReceived += PacketReceived;
            _tracer.OnPacketReceived += IncrementPacketsSent;

            _tracerThread = new Thread(_tracer.Capture);
            _tracerThread.Start();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _tracer.Stop();
            _tracerThread.Abort();
        }

        private void PacketReceived(object sender, PacketTracerEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var item = string.Format("{0}: {1}", e.Packet.PacketType, e.Packet.NetworkHeader.DestIP);

                ++_amountOfPackets;
                if (_amountOfPackets >= MaxPackets)
                {
                    PacketList.Items.RemoveAt(0);
                }

                _packets[_amountOfPackets] = e.Packet;
                PacketList.Items.Add(item);
                PacketList.ScrollIntoView(item);
            });

        }

        private void IncrementPacketsSent(object sender, PacketTracerEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                AmountOfPacketsSentLabel.Content = ++_packetsSent;
            });
        }
    }
}