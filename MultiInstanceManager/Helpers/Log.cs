using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Helpers
{
    public static class Log
    {
        public static void Clear()
        {
            File.WriteAllText("debug.log", "\r\n");
        }
        public static void Debug(string text)
        {
            File.AppendAllText("debug.log", text + "\r\n");
        }
    }
}
