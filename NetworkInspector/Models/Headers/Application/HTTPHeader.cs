using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NetworkInspector.Models.Headers.Transport;
using NetworkInspector.Models.Packets;

namespace NetworkInspector.Models.Headers.Application
{
    public class HTTPHeader : IHeader
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Protocol ProtocolName { get { return Protocol.HTTP;  } }

        public HTTPHeader(byte[] data, int length)
        {
            var stream = new MemoryStream(data, 0, length);
            var reader = new StreamReader(stream);
            //_log.Warn(Encoding.Default.GetString(data));
            _log.Warn(reader.ReadToEnd());
        }
    }
}
