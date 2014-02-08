using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkInspector.Network;

namespace GUIApplication.Pages.Shared
{
    public class NetworkInterfaces : ObservableCollection<string>
    {
        public NetworkInterfaces()
        {
            Add("-- Select a network --");
            foreach (var networkInterface in Utilities.GetNetworkInterfaces())
            {
                Add(networkInterface);
            }
        }
    }
}
