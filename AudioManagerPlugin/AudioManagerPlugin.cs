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
using AudioManagerPlugin.Helpers;
using System.Diagnostics;

namespace AudioManagerPlugin
{
    public class AudioManagerPlugin : IPlugin
    {
        private CancellationToken ct;
        private CancellationTokenSource cts;
        private Task mainLoop;
        private Dictionary<ActiveWindow, AudioHelper.ISimpleAudioVolume> volumeList;
        public void StartMainLoop()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
            volumeList = new Dictionary<ActiveWindow, AudioHelper.ISimpleAudioVolume>();
            mainLoop = Task.Factory.StartNew(() => this.MainLoop(ct), cts.Token);
        }
        public void Stop()
        {
            cts.Cancel();
        }
        private AudioHelper.ISimpleAudioVolume? GetVolumeControlForWindow(ActiveWindow window)
        {
            AudioHelper.ISimpleAudioVolume? currentAudio = null;

            if(volumeList.TryGetValue(window,out currentAudio)){
                if (currentAudio != null)
                {
                    return currentAudio;
                }
            }
            currentAudio = AudioHelper.GetWindowAudioControl(window.Process);
            volumeList.Add(window, currentAudio);
            return currentAudio;
        }
        public void MainLoop(CancellationToken? ct)
        {
            ct?.ThrowIfCancellationRequested();
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
                if (ct?.IsCancellationRequested == true)
                {
                    keepGoing = false;
                }
                else
                {
                    try
                    {
                        if (MultiHandler.activeWindows != null)
                        {
                            foreach (var activeWindow in MultiHandler.activeWindows)
                            {
                                if (activeWindow == null || activeWindow.Profile?.MuteWhenMinimized == false)
                                    continue;
                                var control = GetVolumeControlForWindow(activeWindow);
                                if (activeWindow.Process != null && WindowHelper.IsMinimized(activeWindow.Process) && control != null)
                                {
                                    bool isMuted = false;
                                    try
                                    {
                                        Guid bs = Guid.Empty;
                                        control.GetMute(out isMuted);
                                        if (!isMuted)
                                        {
                                            Log.Debug("Muting window for: " + activeWindow.Profile?.DisplayName);
                                            control.SetMute(true, bs);
                                        }
                                    }
                                    catch (Exception me)
                                    {
                                        Log.Debug("Could not mute window for: " + activeWindow.Profile?.DisplayName);
                                        Log.Debug(me.ToString());
                                    }
                                }
                                else
                                {
                                    bool isMuted = false;
                                    try
                                    {
                                        Guid bs = Guid.Empty;
                                        control?.GetMute(out isMuted);
                                        if (isMuted)
                                        {
                                            Log.Debug("UnMuting window for: " + activeWindow.Profile?.DisplayName);
                                            control?.SetMute(false, bs);
                                        }
                                    }
                                    catch (Exception me)
                                    {
                                        Log.Debug("Could not UnMute window for: " + activeWindow.Profile?.DisplayName);
                                        Log.Debug(me.ToString());
                                    }
                                }
                                Thread.Sleep(50); // Small delay here too
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Debug("Ammount of windows changed: " + e.ToString());
                    }
                }
                Thread.Sleep(1000);
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
