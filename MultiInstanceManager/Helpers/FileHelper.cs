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
            var clone = new Account();
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, account);
                ms.Position = 0;
                Console.WriteLine("Bytes: " + ms.Length);
                clone = (Account)Serializer.Deserialize<Account>(ms);
                Console.WriteLine("Test: " + clone.DisplayName);
                using (FileStream file = new FileStream(account.DisplayName + ".bincnf", FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, (int)ms.Length);
                    file.Write(bytes, 0, bytes.Length);
                    ms.Close();
                }
            }

        }
        public static Account? LoadAccountConfiguration(string DisplayName)
        {
            var filename = DisplayName + ".bincnf";
            if (File.Exists(filename))
            {
                using (FileStream file = new FileStream(filename, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        try
                        {
                            file.CopyTo(ms);
                            ms.Position = 0;
                            Console.WriteLine("Bytes: " + ms.Length);
                            Console.WriteLine("Found an account");
                            return (Account)Serializer.Deserialize<Account>(ms);
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("Can not read previous config for: " + DisplayName);
                            Log.Debug(ex.ToString());
                        }
                    }
                }
            }
            return null;
        }
    }
}
