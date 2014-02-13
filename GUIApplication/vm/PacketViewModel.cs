using System.ComponentModel;

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