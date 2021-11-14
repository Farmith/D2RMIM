using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Structs
{
    public class Account
    {        
        public string DisplayName { get; set; }
      
        public bool SkipCinematics { get; set; }
        public bool UseKeybinds { get; set; }
        public bool ModifyWindowtitles { get; set; }

        public string InstallationPath { get; set; }
        public string GameExecutable { get; set; }

        public LaunchSettings LaunchOptions { get; set; }

        public string Region { get; set; }
        public HotKey WindowHotKey { get; set; }
        
    }
}
