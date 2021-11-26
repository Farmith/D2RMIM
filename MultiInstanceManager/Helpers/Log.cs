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
        public static void Empty()
        {
            var now = DateTime.Now.ToString("[yyyy-dd-M] HH-mm-ss: ");
            // File.WriteAllText("debug.log", now + "New session started");
        }
        public static void Clear()
        {
            var now = DateTime.Now.ToString("[yyyy-dd-M] HH-mm-ss: ");

            File.AppendAllText("debug.log", "\r\n\r\n\r\n\r\n============ CLEAR ===============\r\n\r\n\r\n");
            File.AppendAllText("debug.log", now);
        }
        public static void Debug(string text)
        {
            try
            {
                var now = DateTime.Now.ToString("[yyyy-dd-M] HH-mm-ss: ");
                File.AppendAllText("debug.log", now + text + "\r\n");
            } catch
            {
                // Could not write to debug log its being used by another thread at this very time.
                // Log.Debug(ex.ToString());

            }
        }
    }
}
