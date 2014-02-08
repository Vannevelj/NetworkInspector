using System;
using System.Windows;
using System.Windows.Controls;
using GUIApplication.Pages.PacketTracer;

namespace GUIApplication.Pages.Intro
{
    /// <summary>
    /// Interaction logic for IntroPage.xaml
    /// </summary>
    public partial class IntroPage : Page
    {
        public IntroPage()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {     
            var action = AppsListBox.SelectedIndex;

            switch (action)
            {
                case 0:
                    //NavigationService.Navigate(new BandwidthMonitorPage());
                    break;

                case 1:
                    NavigationService.Navigate(new PacketTracerPage());
                    break;

                    // Nothing is selected
                case -1:
                    break;

                default: 
                    throw new ArgumentException();
            }
        }
    }
}
