using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PacketTracerPage.xaml
    /// </summary>
    public partial class PacketTracerPage : Page
    {
        public PacketTracerPage()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {
                PacketList.Items.Add("Item " + i);
                PacketList.SelectionChanged += Item_Selected;
            }
        }

        private void Item_Selected(object sender, SelectionChangedEventArgs e)
        {
            PacketDetails.Text = (string) PacketList.SelectedItem;
        }

    }
}
