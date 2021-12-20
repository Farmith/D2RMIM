using MultiInstanceManager.Modules;
using MultiInstanceManager.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.ListView;

namespace MultiInstanceManager.Interfaces
{
    public interface IPluginWithLauncher : IPlugin
    {
        void SetMultiHandler(MultiHandler mh);

        public bool LaunchOverride { get; }
        public bool LaunchHandlerOverride { get; }

        public bool TokenOverride { get; }
        public GameInstance LaunchGame(string accountName, string cmdArgs, string installPath, string gameExe);

        public void LaunchHandler(CheckedListViewItemCollection accounts);
        public List<string> TokenLocation(Process process);
    }
}
