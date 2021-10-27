using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager
{
    public static class Constants
    {
        /* Executable names */
        public static string clientExecutableName = "D2R.exe";
        public static string launcherExecutableName = "Diablo II Resurrected Launcher.exe";
        public static string battleNetExecutableName = "Battle.net.exe";
        /* Account related registry keys */
        public static List<String> accountRegKey = new List<String> { "HKEY_CURRENT_USER\\SOFTWARE\\Blizzard Entertainment\\Battle.net\\Launch Options\\OSI", "WEB_TOKEN" };
        public static List<String> gameInstallRegKey = new List<string> { "HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Diablo II Resurrected", "InstallLocation" };
        /* Window classes */
        public static string gameClass = "OsWindow";
        public static string bnetLauncherClass = "Qt5QWindowIcon";
        public static string bnetClientClass = "Chrome_WidgetWin_0";
    }
}
