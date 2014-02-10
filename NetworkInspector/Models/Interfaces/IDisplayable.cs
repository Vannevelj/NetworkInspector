using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkInspector.Models.Interfaces
{
    public interface IDisplayable
    {
        Dictionary<string, string> GetFieldRepresentation();
    }
}
