using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Structs
{
    public class HotKey
    {
        public char ModifierKey { get; set; }
        public char Key { get; set; }
        public bool Enabled { get; set; }
    }
}
