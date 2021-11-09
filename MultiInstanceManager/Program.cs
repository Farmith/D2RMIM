using System;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace MultiInstanceManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (IKeyboardMouseEvents globalHook = Hook.GlobalEvents())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MultiInstanceManager(globalHook));
            }
        }
    }
}
