using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiInstanceManager.Structs
{
    [ProtoContract]
    public class HotKey
    {
        [ProtoMember(1)]
        public Keys? ModifierKey { get; set; }
        [ProtoMember(2)]
        public Keys? Key { get; set; }
        [ProtoMember(3)]
        public bool Enabled { get; set; }
    }
}
