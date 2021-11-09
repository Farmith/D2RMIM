using MultiInstanceManager.Collections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Config
{
    public class KeyBindsSection : ConfigurationSection
    {
        // public static KeyBindsSection KeyBind => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("keyBinds") as KeyBinds;

        [ConfigurationProperty("windowKeyBinds", IsRequired = true, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(WindowKeyBindCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public WindowKeyBindCollection WindowKeyBinds
        {
            get {
                Debug.WriteLine("Inside the config section");
                Debug.WriteLine(this["windowKeyBinds"].ToString());
                return (WindowKeyBindCollection)base["windowKeyBinds"]; 
            }
            set {
                Debug.WriteLine("Setting windowKeyBinds to: " + value);
                this["windowKeyBinds"] = value; 
            }
        }
    }
}
