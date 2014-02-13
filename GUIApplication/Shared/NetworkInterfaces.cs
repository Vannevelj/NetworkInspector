using System.Collections.ObjectModel;
using NetworkInspector.Network;

namespace GUIApplication.Shared
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