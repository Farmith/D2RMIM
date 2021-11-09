using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Structs
{
    public class KeyToggle
    {
        public int ClientIterator { get; set; }
        public string Key { get; set; }
        public int CharCode { get; set; }
        public IntPtr WindowHandle { get; set; }
        public int ProcessId { get; set; }
    }
}
