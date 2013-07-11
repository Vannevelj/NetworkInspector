using NetworkInspector.Models.Interfaces;
using NetworkInspector.Network;
using System;
using System.Diagnostics;
using System.Threading;

namespace NetworkInspector {
    public class Program {
        private static void Main(string[] args) {
            var category = new PerformanceCounterCategory("Network Interface");
            var instances = category.GetInstanceNames();
            Console.WriteLine("All available network interfaces:\n");

            for (var i = 0; i < instances.Length; i++) {
                Console.WriteLine(i + ": " + instances[i]);
            }

            var choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Selected network interface:\n" + instances[choice]);

            while (true) {
                IStatistics stats = Utilities.GetNetworkStatistics(instances[choice]);
                Console.WriteLine("Download speed: " + stats.DownloadSpeed + " KBytes/s");
                Console.WriteLine("Upload speed: " + stats.UploadSpeed + " KBytes/s");
                Console.WriteLine("--------------------------------------------------------------\n\n");
                Thread.Sleep(1000);
            }
        }
    }
}