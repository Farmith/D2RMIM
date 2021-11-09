using MultiInstanceManager.Interfaces;
using MultiInstanceManager.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace MultiInstanceManager.Collections
{
    public class WindowKeyBindCollection : ConfigurationElementCollection
    {
        public List<IKeyBind> All { get { return this.Cast<IKeyBind>().ToList(); } }

        public WindowKeyBindCollection()
        {
            Debug.WriteLine("Instantiating a new collection");
        }
        public WindowKeyBindElement this[int index]
        {
            get
            {
                Debug.WriteLine("Fetching element index: " + index);
                return base.BaseGet(index) as WindowKeyBindElement;
            }
            set
            {
                Debug.WriteLine("Adding element to index: " + index);
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }
        public void add(WindowKeyBindElement keybind)
        {
            BaseAdd(keybind);
        }
        public void clear()
        {
            BaseClear();
        }
        public void remove(WindowKeyBindElement keybind)
        {
            BaseRemove(keybind);
        }
            
        protected override ConfigurationElement CreateNewElement()
        {
            return new WindowKeyBindElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WindowKeyBindElement)element).key;
        }
        public object GetElementEnabled(ConfigurationElement element)
        {
            return ((WindowKeyBindElement)element).enabled;
        }
        public object GetElementAscii(ConfigurationElement element)
        {
            return ((WindowKeyBindElement)element).ascii;
        }
    }
}