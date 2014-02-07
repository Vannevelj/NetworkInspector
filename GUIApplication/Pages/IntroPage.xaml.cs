using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUIApplication.Pages
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
            var action = this.AppsListBox.SelectedIndex;

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
