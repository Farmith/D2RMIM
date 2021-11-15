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
        public static List<AccountBinary> GetAccountsByFolder(string extension="*.bin")
        {
            var ePath = Application.ExecutablePath;
            var path = Path.GetDirectoryName(ePath);
            var Accounts = new List<AccountBinary>();
            var accounts = Directory.GetFiles(path, "*.bin");
            foreach (var account in accounts)
            {
                var lastWrite = File.GetLastWriteTime(account);
                var fileName = Path.GetFileNameWithoutExtension(account);
                Accounts.Add(new AccountBinary { FullPath = account, AccountName = fileName, LastWriteTime = lastWrite });
            }
            return Accounts;
        }
        public static bool JSONSettingsExist(string name)
        {
            if(File.Exists(name))
            {
                return true;
            }
            return false;
        }
        public static void CreateJSONSettings(string name)
        {
            var filePath = Environment.ExpandEnvironmentVariables(Constants.clientSettingsJsonLocation);
            var inJson = filePath +  "Settings.json";
            var outJson = name + Constants.clientSettingsAppendName;
            if (!JSONSettingsExist(outJson))
            {
                if(!File.Exists(inJson))
                {
                    // There is no settings file, cant do this.
                    Log.Debug("No settings file in: " + inJson);
                    return;
                }
                File.Copy(inJson, outJson);
            }
        }
        public static void ReplaceJSONSettingsFile(string name)
        {
            var filePath = Environment.ExpandEnvironmentVariables(Constants.clientSettingsJsonLocation);
            var inJson = filePath + "Settings.json";
            var outJson = name + Constants.clientSettingsAppendName;
            var storeJson = "original" + Constants.clientSettingsAppendName;
            // For just being double sure, check that there IS a settings file:
            CreateJSONSettings(name);
            // Copy the "standard" or old file to temp:
            if (File.Exists(inJson))
            {
                File.Copy(inJson, storeJson);
                File.Copy(outJson, inJson);
            }
        }
        public static void SaveAccountConfiguration(Account account)
        {
            //            var model = RuntimeTypeModel.Create();
            Console.WriteLine("Displ: " + account.DisplayName);
            var filename = account.DisplayName + ".bincnf";

            using (var file = File.Create(filename))
            {
                Serializer.Serialize(file, account);
            }
        }
        public static Account? LoadAccountConfiguration(string DisplayName)
        {
            var filename = DisplayName + ".bincnf";
            var account = new Account();
            if (File.Exists(filename))
            {
                using (var file = File.OpenRead(filename))
                {
                    account = Serializer.Deserialize<Account>(file);
                    return account;
                }
            }
            return null;
        }
    }
}
