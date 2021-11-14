using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Structs
{
    public class LaunchSettings
    {
        public string PreLaunchCommands { get; set; }
        public string PostLaunchCommands { get; set; }
        public string LaunchArguments { get; set; }
        public int WindowX { get; set; }
        public int WindowY { get; set; }
    }
}
