using MultiInstanceManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Config
{
    public class WindowKeyBindElement : ConfigurationElement, IKeyBind
    {
        [ConfigurationProperty("ascii", IsRequired = true, DefaultValue = "", IsKey = true)]
        public string ascii
        {
            get {
                Debug.WriteLine("Fetching ascii: " + (string)this["ascii"]);
                return (string)this["ascii"]; 
            }
            set {
                Debug.WriteLine("Setting ascii to: " + value);
                this["ascii"] = value; 
            }
        }
        [ConfigurationProperty("key", IsRequired = true, DefaultValue = "", IsKey = true)]
        public string key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }
        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false, IsKey = true)]
        public bool enabled
        {
            get {
                return (bool)this["enabled"];
            }
            set { this["enabled"] = value; }
        }
    }
}
