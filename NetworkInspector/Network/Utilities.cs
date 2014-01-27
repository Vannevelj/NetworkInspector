using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace NetworkInspector.Network
{
    public static class Utilities
    {
        static Utilities()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }

        // <summary>
        // Returns a list of all available network interfaces
        // </summary>
        public static IList<string> GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Select(x => x.Description).ToList();
        }

        // <summary>
        // Returns details about the given network interface
        // </summary>
        public static NetworkInterface GetInterfaceInformation(string name)
        {
            return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x.Description == name);
        }

        /// <summary>
        ///     Retrieves the IP of the current device in the local network
        /// </summary>
        /// <returns>The Local IP address</returns>
        public static IPAddress GetLocalIP()
        {
            return
                Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}