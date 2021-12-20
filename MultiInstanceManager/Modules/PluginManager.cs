using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Interfaces;
using MultiInstanceManager.Structs;
using static System.Windows.Forms.ListView;

namespace MultiInstanceManager.Modules
{
    public class PluginManager
    {
        public static List<IPlugin>? Plugins { get; set; }
        private Dictionary<String,CancellationTokenSource>? PluginTokenSources { get; set; }

        public void MessageHandler(CancellationToken ct)
        {
            // Handle sending commands to plugins here? if any
            ct.ThrowIfCancellationRequested();
            var keepGoing = true;
            while (keepGoing)
            {
                if (ct.IsCancellationRequested)
                {
                    keepGoing = false;
                }
                // Log.Debug("So far we have: " + Plugins?.Count + " plugins");

                if (Plugins?.Count > 0)
                {
                    foreach (IPlugin plugin in Plugins)
                    {
                        // Log.Debug("Name: " + plugin.Name);
                    }
                }
                Thread.Sleep(10000); // Just since we aren't actually doing anything here, yet
            }
        }
        public bool PluginBasedLaunchHandler(CheckedListViewItemCollection profiles)
        {
            if ( Plugins != null )
            {
                foreach(IPluginWithLauncher plugin in Plugins)
                {
                    if ( plugin.LaunchHandlerOverride == true)
                    {
                        plugin.LaunchHandler(profiles);
                        return true;
                    }
                }
            }
            return false;
        }
        public List<string>? PluginBasedTokenLocation(Process process)
        {
            List<string>? tokenLocation = null;
            if ( Plugins != null )
            {
                Log.Debug("We have some plugins to check for an altered token location");
                foreach (IPluginWithLauncher plugin in Plugins)
                {
                    Log.Debug("Checking if plugin: " + plugin.Name + " has a token location override");
                    if (plugin.TokenOverride == true)
                    {
                        Log.Debug("It does, repsonding with the new location:");
                        tokenLocation = plugin.TokenLocation(process);
                        Log.Debug("Location: " + tokenLocation[0] + " Name: " + tokenLocation[1]);
                        break; // Only one plugin may supply a tokenlocation at a time.
                    }
                }
            }
            return tokenLocation;
        }
        /*
         * PluginHandledLaunch Returns null if no plugin handled the launch
         * 
         */
        public GameInstance? PluginHandledLaunch(string accountName, string cmdArgs, string installPath, string gameExe)
        {
            GameInstance? thisInstance = null;
            if (Plugins != null)
            {
                Log.Debug("We have atleast some plugins");
                foreach (IPluginWithLauncher plugin in Plugins)
                {
                    Log.Debug("Checking if plugin has override");
                    if (plugin.LaunchOverride == true)
                    {
                        Log.Debug("Plugin has override, attempting to launch");
                        thisInstance = plugin.LaunchGame(accountName, cmdArgs, installPath, gameExe);
                        break;  // We only allow ONE plugin at a time to override the launch sequence.
                    }
                }
            }
            Log.Debug("Returning instance");
            return thisInstance;
        }
        public void SetMultiHandler(MultiHandler mh)
        {
            if (Plugins != null)
            {
                foreach (IPluginWithLauncher plugin in Plugins)
                {
                    if (plugin.LaunchOverride == true)
                    {
                        Log.Debug("Setting  multihandler of: " + plugin.Name);
                        plugin.SetMultiHandler(mh);
                    }
                }
            }
        }
        public void LoadPlugins()
        {
            Log.Debug("PluginManager: Loading plugins");
            Plugins = new List<IPlugin>();
            PluginTokenSources = new Dictionary<String, CancellationTokenSource>();

            if (Directory.Exists(Constants.PluginFolderName))
            {
                Log.Debug("PluginManager: Directory is fine, references should be loaded, loading plugins");
                String[] plugins = Directory.GetFiles(Constants.PluginFolderName);
                foreach (string plugin in plugins)
                {
                    if (Path.GetExtension(plugin).CompareTo(".dll") == 0){
                        Log.Debug("PluginManager: Loading => " + plugin);
                        Assembly.LoadFile(Path.GetFullPath(plugin));
                    }
                }
            }
            Log.Debug("PluginManager: Plugins loaded in fileformat, instantiating");
            Type interfaceType = typeof(IPlugin);
            try
            {
                Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass).ToArray();
                Log.Debug("PluginManager: Found " + types.Length + " plugins to iterate");
                foreach (Type type in types)
                {
                    // Initiate new instance of all plugins (types)
                    Log.Debug("Adding plugin of type: " + type.ToString());
                    var ip = (IPlugin?)Activator.CreateInstance(type);
                    if(ip != null)
                        Plugins.Add((IPlugin)ip);
                }
                Thread.Sleep(5000);
                // Start all plugins main-loop
                Log.Debug("Loading all plugins main-loop");
                foreach (IPlugin plugin in Plugins)
                {
                    Log.Debug("Starting mainloop of: " + plugin.Name);
                    plugin.StartMainLoop();
                    Log.Debug("Started..");
                }
            } catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception? exSub in ex.LoaderExceptions)
                {
                    if (exSub == null)
                        continue;
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException? exFileNotFound = exSub as FileNotFoundException;
                    if(exFileNotFound != null)
                    {
                        if(!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log: ");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                Log.Debug("PluginManager: Major error: " + sb);
            }
            Log.Debug("Done with starting plugin mainloops");
        }
    }
}
