#nullable enable
using MultiInstanceManager.Structs;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiInstanceManager.Helpers
{
    public static class FileHelper
    {
        public static List<AccountBinary> GetProfilesByFolder(string extension = "*.bin")
        {
            var ePath = Application.ExecutablePath;
            var path = Path.GetDirectoryName(ePath);
            var Accounts = new List<AccountBinary>();
            if (path == null)
            {
                return Accounts;
            }
            var accounts = Directory.GetFiles(path: path, searchPattern: "*.bin");
            foreach (var account in accounts)
            {
                var lastWrite = File.GetLastWriteTime(account);
                var fileName = Path.GetFileNameWithoutExtension(account);
                Accounts.Add(new AccountBinary { FullPath = account, AccountName = fileName, LastWriteTime = lastWrite });
            }
            Log.Debug("Found: " + Accounts.Count + " accounts by .bin");
            return Accounts;
        }
        public static bool JSONSettingsExist(string name)
        {
            if (File.Exists(name))
            {
                return true;
            }
            return false;
        }
        public static void DeleteProfileConfiguration(string? name)
        {
            var file = name + ".cnf";
            Log.Debug("Deleting file: " + file);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        public static void DeleteProfileTokenStorage(string? name)
        {
            var file = name + ".bin";
            Log.Debug("Deleting file: " + file);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        public static void DeleteJSONSettings(string? name)
        {
            if (name == null)
                return;

            var outJson = name + Constants.clientSettingsAppendName;
            
            if (JSONSettingsExist(outJson))
            {
                File.Delete(outJson);
            }
        }
        public static void CreateJSONSettings(string? name)
        {
            if (name == null)
                return;
            var filePath = Environment.ExpandEnvironmentVariables(Constants.clientSettingsJsonLocation);
            var inJson = filePath + "Settings.json";
            var outJson = name + Constants.clientSettingsAppendName;
            if (!JSONSettingsExist(outJson))
            {
                if (!File.Exists(inJson))
                {
                    // There is no settings file, cant do this.
                    Log.Debug("No settings file in: " + inJson);
                    return;
                }
                File.Copy(inJson, outJson);
            }
        }
        public static void ReplaceJSONSettingsFile(string? name)
        {
            if (name == null)
                return;
            var filePath = Environment.ExpandEnvironmentVariables(Constants.clientSettingsJsonLocation);
            var inJson = filePath + "Settings.json";
            var outJson = name + Constants.clientSettingsAppendName;
            var storeJson = "original" + Constants.clientSettingsAppendName;
            // For just being double sure, check that there IS a settings file:
            CreateJSONSettings(name);
            // Copy the "standard" or old file to temp:
            if (File.Exists(inJson))
            {
                File.Copy(inJson, storeJson, true);
                File.Copy(outJson, inJson, true);
            }
        }
        public static void SaveAccountConfiguration(Profile profile)
        {
            //            var model = RuntimeTypeModel.Create();
            Log.Debug("Profile: " + profile.DisplayName);
            var filename = profile.DisplayName + ".cnf";

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, profile);
            }
        }
        public static Profile? LoadProfileConfiguration(string DisplayName)
        {
            var filename = DisplayName + ".cnf";
            var profile = new Profile();
            if (File.Exists(filename))
            {
                using (var file = File.OpenRead(filename))
                {
                    profile = Serializer.Deserialize<Profile>(file);
                }
                return profile;
            }
            return null;
        }
        public static bool CopyProfileConfigurationFrom(string fromName, string toName)
        {
            var fromfilename = fromName + ".cnf";
            var tofilename = toName + ".cnf";
            if (File.Exists(fromfilename))
            {
                File.Copy(fromfilename, tofilename);
                return true;
            }
            return false;
        }
        public static void MoveOldProfileConfigurations()
        {
            var ePath = Application.ExecutablePath;
            var path = Path.GetDirectoryName(ePath);
            if (path == null)
                return;
            var profiles = Directory.GetFiles(path, "*.bincnf");
            foreach (var profile in profiles)
            {
                var newname = profile.Substring(0, profile.Length - 7) + ".cnf";
                File.Move(profile, newname);
            }
            Log.Debug("Found: " + profiles.Count() + " profiles that needed cleaning.");
        }
    }
}
