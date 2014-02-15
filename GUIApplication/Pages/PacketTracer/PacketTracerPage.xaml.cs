using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GUIApplication.vm;
using log4net;
using NetworkInspector.Models.Headers.Application.HTTP;
using NetworkInspector.Models.Packets;
using NetworkInspector.Network.PacketTracing;

namespace GUIApplication.Pages.PacketTracer
{
    /// <summary>
    ///     Interaction logic for PacketTracerPage.xaml
    /// </summary>
    public partial class PacketTracerPage : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private NetworkInspector.Network.PacketTracing.PacketTracer _tracer;
        private Thread _tracerThread;
        private int _packetsSent;

        private const int MaxPackets = 4096;
        private readonly Packet[] _packets = new Packet[MaxPackets];
        private int _selectedPacket;

        public List<PacketViewModel> PacketDetails { get; set; }


        public PacketTracerPage()
        {
            InitializeComponent();
            PacketDetails = new List<PacketViewModel>();

            // UI Event handlers
            PacketList.SelectionChanged += PacketList_ItemSelected;
        }

        private void PacketList_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            PacketDetails = new List<PacketViewModel>();
            _selectedPacket = PacketList.SelectedIndex;
            var packet = _packets[_selectedPacket];

            foreach (var p in packet.GetFieldRepresentation())
            {
                PacketDetails.Add(new PacketViewModel {Key = p.Key, Value = p.Value});
            }

            FieldGrid.DataContext = PacketDetails;
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _tracer = new NetworkInspector.Network.PacketTracing.PacketTracer();
            _tracer.OnPacketReceived += PacketSent;

            _log.Info("Packet tracing activated in interface");
            _tracerThread = new Thread(_tracer.Capture);
            _tracerThread.Start();         

            StopButton.IsEnabled = true;
            StartButton.IsEnabled = false;
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _log.Info("Packet tracing deactivated in interface");
            _tracer.Stop();
            _tracerThread.Abort();

            StopButton.IsEnabled = false;
            StartButton.IsEnabled = true;
        }

        private void PacketSent(object sender, PacketTracerEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var item = string.Format("{0} - {1}: {2}", e.Packet.NetworkHeader.Identification, e.Packet.ApplicationHeader == null ? e.Packet.PacketType : e.Packet.ApplicationHeader.ProtocolName, e.Packet.NetworkHeader.DestinationIP);

                if (_packetsSent >= MaxPackets)
                {
                    Array.Copy(_packets, 1, _packets, 0, _packets.Length - 1);
                    PacketList.Items.RemoveAt(0);
                }

                _packets[_packetsSent%MaxPackets] = e.Packet;
                PacketList.Items.Add(item);
                PacketList.ScrollIntoView(item);
                AmountOfPacketsSentLabel.Content = _packetsSent++;
            });
        }
    }
}