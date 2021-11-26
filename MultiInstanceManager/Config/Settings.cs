using MultiInstanceManager.Collections;
using MultiInstanceManager.Interfaces;
using MultiInstanceManager.Structs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MultiInstanceManager.Config
{
    public class Settings
    {
        /* Keybinds */
        public List<WindowKeyBindElement> KeyBinds = new List<WindowKeyBindElement>();
        public List<KeyToggle> KeyToggles = new List<KeyToggle>();

        public void LoadWindowKeys()
        {
            Reset();
            string configFile = $"{Assembly.GetExecutingAssembly().Location}.config";
            string outputConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            if (!File.Exists(outputConfigFile))
            {
                Debug.WriteLine("No config at: " + outputConfigFile);
            }
            else
            {
                KeyBindsSection? winKeyBinds = ConfigurationManager.GetSection("keyBinds") as KeyBindsSection;
                List<IKeyBind>? collection = winKeyBinds?.WindowKeyBinds?.All;

                int i = 0;
                if (collection == null)
                    return;
                foreach (WindowKeyBindElement keyToggle in collection)
                {
                    if (keyToggle.enabled)
                    {
                        int a = -1;
                        if (int.TryParse(keyToggle.ascii, out int x))
                        {
                            a = x;
                        }

                        var bind = new KeyToggle { CharCode = a, ClientIterator = i, WindowHandle = IntPtr.Zero, ProcessId = 0 };
                        KeyToggles.Add(bind);
                        i++;
                    }
                }
            }
        }
        public void Reset()
        {
            KeyBinds = new List<WindowKeyBindElement>();
            KeyToggles = new List<KeyToggle>();
        }
    }
}
