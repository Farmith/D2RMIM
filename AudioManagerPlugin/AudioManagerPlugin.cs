using MultiInstanceManager.Helpers;
using MultiInstanceManager.Interfaces;
using MultiInstanceManager.Modules;
using MultiInstanceManager.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AudioManagerPlugin
{
    public class AudioManagerPlugin : IPlugin
    {
        private CancellationToken ct;
        private CancellationTokenSource cts;
        private Task mainLoop;

        public void StartMainLoop()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
            mainLoop = Task.Factory.StartNew(() => this.MainLoop(ct), cts.Token);
        }
        public void Stop()
        {
            cts.Cancel();
        }
        public void MainLoop(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var keepGoing = true;
            // Things outside of any infinite loop, can just be executed as-is
            foreach (IPlugin plugin in PluginManager.Plugins)
            {
                Log.Debug(plugin.Name + ": " + plugin.Description);
            }

            /*
             * The Main Loop (of the Main Loop, how meta) needs to adhere to threading principals
             * I.E quit looping if a cancellation is requested
             */
            while (keepGoing)
            {
                if (ct.IsCancellationRequested)
                {
                    keepGoing = false;
                }
                else
                {
                    if (MultiHandler.activeWindows != null)
                    {
                        foreach (ActiveWindow window in MultiHandler.activeWindows)
                        {
                            Log.Debug("ExamplePlugin => Found an active window for: " + window.Profile.DisplayName);
                        }
                    }
                    Thread.Sleep(5000);
                }
            }
        }
        public string Name
        {
            get
            {
                return "ExamplePlugin";
            }
        }
        public string Description
        {
            get
            {
                return "This is an example plugin for D2RMIM";
            }
        }
    }
}
