using System.Collections.Generic;

namespace NetworkInspector.Models.Interfaces
{
    public interface IDisplayable
    {
        Dictionary<string, string> GetFieldRepresentation();
    }
}