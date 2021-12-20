using MultiInstanceManager.Modules;
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
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        // Add plugin methods here that correspond to what is expsed later..
        void StartMainLoop();
        void Stop();
        void MainLoop(CancellationToken? ct);
    }
}
