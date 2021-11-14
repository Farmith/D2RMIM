using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Structs
{
    [ProtoContract]
    public class LaunchSettings
    {
        [ProtoMember(30)]
        public string PreLaunchCommands { get; set; }
        [ProtoMember(31)]
        public string PostLaunchCommands { get; set; }
        [ProtoMember(32)]
        public string LaunchArguments { get; set; }
        [ProtoMember(33)]
        public int WindowX { get; set; }
        [ProtoMember(34)]
        public int WindowY { get; set; }
    }
}
