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
