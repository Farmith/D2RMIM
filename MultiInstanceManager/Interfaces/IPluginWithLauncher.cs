using MultiInstanceManager.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiInstanceManager.Interfaces
{
    public interface IPluginWithLauncher : IPlugin
    {
        public bool LaunchOverride { get; }
        public GameInstance LaunchGame(string accountName, string cmdArgs, string installPath, string gameExe);
    }
}
