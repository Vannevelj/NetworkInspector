using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkInspector.Models.Headers.Application.HTTP
{
    public struct Cookie
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
