using MultiInstanceManager.Structs;
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
    }
}
