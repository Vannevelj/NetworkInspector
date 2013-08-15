using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkInspector.Extensions {
    public static class Helpers {
        public static string ToTransferRate(this float input) {
            if (input < 1028) {
                return input.ToString("0.00") + " Bytes/Second";
            }

            if (input < (1028 * 1028)) {
                return (input / 1028).ToString("0.00") + " KBytes/Second";
            }

            if (input < (1028 * 1028)) {
                return (input / 1028 / 1028).ToString("0.00") + " MBytes/Second";
            }

            if (input < (1028 * 1028 * 1028)) {
                return (input / 1028 / 1028 / 1028).ToString("0.00") + " GBytes/Second";
            }

            throw new ArgumentOutOfRangeException("input");
        }
    }
}