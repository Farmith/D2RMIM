using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Structs
{
    [ProtoContract]
    public class Profile
    {
        [ProtoMember(1)]
        public string? DisplayName { get; set; }
        [ProtoMember(2)]
        public bool SkipCinematics { get; set; }
        [ProtoMember(3)]
        public bool UseKeybinds { get; set; }
        [ProtoMember(4)]
        public bool ModifyWindowtitles { get; set; }
        [ProtoMember(5)]
        public string? InstallationPath { get; set; }
        [ProtoMember(6)]
        public string? GameExecutable { get; set; }
        [ProtoMember(7)]
        public LaunchSettings? LaunchOptions { get; set; }

        [ProtoMember(8)]
        public string? Region { get; set; }
        [ProtoMember(9)]
        public HotKey? WindowHotKey { get; set; }
        [ProtoMember(10)]
        public bool UseDefaultGameInstallation { get; set; }
        [ProtoMember(11)]
        public bool SeparateTaskbarIcons { get; set; }
        [ProtoMember(12)]
        public bool SeparateJsonSettings { get; set; }
        [ProtoMember(13)]
        public bool MuteWhenMinimized { get; set; }
    }
}
