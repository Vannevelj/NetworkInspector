using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NetworkInspector.Network.PacketTracing;

namespace GUIApplication.Pages.PacketTracer
{
    /// <summary>
    ///     Interaction logic for PacketTracerPage.xaml
    /// </summary>
    public partial class PacketTracerPage : Page
    {
        private NetworkInspector.Network.PacketTracing.PacketTracer _tracer;
        private Thread _tracerThread;
        private int _packetsSent;

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
            PacketDetails.Text = (string) PacketList.SelectedItem;
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
                PacketList.Items.Add(string.Format("{0}: {1}", e.Packet.Received, e.Packet.PacketType));
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