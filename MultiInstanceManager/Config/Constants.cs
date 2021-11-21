using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager
{
    public static class Constants
    {
        /* Executable names */
        public static string clientExecutableName = "D2R";
        public static string executableFileExt = ".exe";
        public static string launcherExecutableName = "Diablo II Resurrected Launcher.exe";
        public static string battleNetExecutableName = "Battle.net.exe";
        /* Account related registry keys */
        public static List<String> accountRegKey = new List<String> { "HKEY_CURRENT_USER\\SOFTWARE\\Blizzard Entertainment\\Battle.net\\Launch Options\\OSI", "WEB_TOKEN" };
        public static List<String> gameInstallRegKey = new List<String> { "HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Diablo II Resurrected", "InstallLocation" };
        public static List<String> clientRegionKey = new List<String> { "HKEY_CURRENT_USER\\SOFTWARE\\Blizzard Entertainment\\Battle.net\\Launch Options\\OSI", "REGION" };
        /* Account Related Statics */
        public static string clientSettingsAppendName = " - clientSettings.json";
        public static string clientSettingsJsonLocation = @"%UserProfile%\Saved Games\Diablo II Resurrected\";
        /* Plugin stuff */
        public static string PluginFolderName = "plugins";
        /* Window classes */
        public static string gameClass = "OsWindow";
        public static string bnetLauncherClass = "Qt5QWindowIcon";
        public static string bnetClientClass = "Chrome_WidgetWin_0";
        public static string windowKeys = "windowKeys";
        public static string windowKey = "keyBind";
        /* Colors */
        public static Color loginButtonColor = Color.FromArgb(255, 0, 116, 224);
        public static Color usernameBoxColor = Color.FromArgb(255, 16, 17, 23);

    }
}
