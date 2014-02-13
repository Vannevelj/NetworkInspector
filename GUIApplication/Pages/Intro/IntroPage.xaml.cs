using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GUIApplication.Pages.PacketTracer;
using log4net;

namespace GUIApplication.Pages.Intro
{
    /// <summary>
    ///     Interaction logic for IntroPage.xaml
    /// </summary>
    public partial class IntroPage : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IntroPage()
        {
            _log.Info(string.Format("Session started at {0}", DateTime.Now));
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