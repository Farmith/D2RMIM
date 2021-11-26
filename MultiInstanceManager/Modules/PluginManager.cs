using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiInstanceManager.Helpers;
using MultiInstanceManager.Interfaces;

namespace MultiInstanceManager.Modules
{
    public class PluginManager
    {
        public static List<IPlugin> Plugins { get; set; }
        private Dictionary<String,CancellationTokenSource> PluginTokenSources { get; set; }

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
                Log.Debug("So far we have: " + Plugins.Count + " plugins");
                Thread.Sleep(10000); // Just since we aren't actually doing anything here, yet
            }
        }
        public void LoadPlugins()
        {
            Log.Debug("PluginManager: Loading plugins");
            Plugins = new List<IPlugin>();
            PluginTokenSources = new Dictionary<String, CancellationTokenSource>();

            if (Directory.Exists(Constants.PluginFolderName))
            {
                if (Directory.Exists(Constants.PluginFolderName + "\\" + Constants.PluginLibReferenceFolder))
                {
                    Log.Debug("Loading all referenced libraries first");
                    String[] references = Directory.GetFiles(Constants.PluginFolderName + "\\" + Constants.PluginLibReferenceFolder);
                    foreach (string reference in references)
                    {
                        if (Path.GetExtension(reference).CompareTo(".dll") == 0)
                        {
                            Log.Debug("PluginManager: Loading ref => " + reference);
                            Assembly.LoadFile(Path.GetFullPath(reference));
                        }
                    }
                    Log.Debug("Done loading references");
                }
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
                    Plugins.Add((IPlugin)Activator.CreateInstance(type));
                }
                Thread.Sleep(5000);
                // Start all plugins main-loop
                Log.Debug("Loading all plugins main-loop");
                foreach (IPlugin plugin in Plugins)
                {
                    plugin.StartMainLoop();
                }
            } catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
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
