using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GUIApplication.vm
{
    public class PacketViewModel
    {
        [DisplayName("Key")]
        public string Key { get; set; }

        [DisplayName("Value")]
        public string Value { get; set; }
    }
}
