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
        [ProtoMember(1)]
        public string? PreLaunchCommands { get; set; }
        [ProtoMember(2)]
        public string? PostLaunchCommands { get; set; }
        [ProtoMember(3)]
        public string? LaunchArguments { get; set; }
        [ProtoMember(4)]
        public int WindowX { get; set; }
        [ProtoMember(5)]
        public int WindowY { get; set; }
    }
}
