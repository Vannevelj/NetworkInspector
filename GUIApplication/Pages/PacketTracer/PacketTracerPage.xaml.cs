using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIApplication.Pages.PacketTracer
{
    /// <summary>
    ///     Interaction logic for PacketTracerPage.xaml
    /// </summary>
    public partial class PacketTracerPage : Page
    {
        public PacketTracerPage()
        {
            InitializeComponent();

            // Display the standard text in the combobox
            NetworkInterfaceComboBox.SelectedIndex = 0;

            for (var i = 0; i < 5; i++)
            {
                PacketList.Items.Add("Item " + i);
            }

            // Event handlers
            PacketList.SelectionChanged += PacketList_ItemSelected;
        }


        private void PacketList_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            PacketDetails.Text = (string) PacketList.SelectedItem;
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}