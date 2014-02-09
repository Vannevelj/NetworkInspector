﻿using System;
using System.Collections.Specialized;
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
        private readonly Packet[] _packets = new Packet[4096];
        private int _selectedPacket;
 

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
            _tracer.OnPacketReceived += IncrementPacketsSent;
            _tracer.OnPacketReceived += PacketSent;

            _tracerThread = new Thread(_tracer.Capture);
            _tracerThread.Start();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _tracer.Stop();
            _tracerThread.Abort();
        }

        private void PacketSent(object sender, PacketTracerEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var item = string.Format("{0}: {1}", e.Packet.PacketType, e.Packet.NetworkHeader.DestIP);

                if (_packetsSent >= MaxPackets)
                {
                    Array.Copy(_packets, 1, _packets, 0, _packets.Length - 1);
                    PacketList.Items.RemoveAt(0);
                }

                _packets[_packetsSent % MaxPackets] = e.Packet;
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