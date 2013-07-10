using NetworkInspector.Network;
using System;
using System.Diagnostics;
using System.Threading;

namespace NetworkInspector {
    internal class Program {
        private static void Main(string[] args) {
            var category = new PerformanceCounterCategory("Network Interface");
            var instances = category.GetInstanceNames();

            Console.WriteLine(category.CategoryName);
            Console.WriteLine(category.CategoryHelp);
            Console.WriteLine(instances[3]);

            Console.WriteLine("\n------\n");
            Console.WriteLine("All available network interfaces:\n");

            foreach (var instance in instances) {
                Console.WriteLine(instance);
            }

            while (true) {
                var stats = Utilities.GetNetworkStatistics(instances[3]);
                Console.WriteLine("Download speed: " + stats.DownloadSpeed + " KBytes/s");
                Console.WriteLine("Upload speed: " + stats.UploadSpeed + " KBytes/s");
                Console.WriteLine("--------------------------------------------------------------\n\n");
                Thread.Sleep(1000);
            }
        }
    }
}