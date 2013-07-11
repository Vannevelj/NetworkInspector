﻿using NetworkInspector.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetworkInspector.Network {
    public static class Utilities {
        static Utilities() {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }

        // <summary>
        // Returns statistics about the given network interface
        // </summary>
        public static Statistics GetNetworkStatistics(string interfaceName) {
            var stats = StatisticsFactory.CreateStatistics(interfaceName);

            var dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", interfaceName);
            var dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", interfaceName);

            float sendSum = 0;
            float receiveSum = 0;

            for (var index = 0; index < StatisticsFactory.MULTIPLIER; index++) {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }

            if (sendSum > 0 || receiveSum > 0) {
                StatisticsFactory.AddReceivedData(receiveSum);
                StatisticsFactory.AddSentData(sendSum);
            }

            return stats;
        }

        // <summary>
        // Returns a list of all available network interfaces
        // </summary>
        public static IList<string> GetNetworkInterfaces() {
            return new PerformanceCounterCategory("Network Interface").GetInstanceNames().ToList();
        }
    }
}