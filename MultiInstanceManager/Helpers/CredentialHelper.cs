#nullable enable
using MultiInstanceManager.Modules;
using MultiInstanceManager.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Helpers
{
    public static class CredentialHelper
    {
        public static Credentials? GetVaultCredentials(string displayName)
        {
            var bnetCredentials = Vault.GetCredential("D2RMIM-" + displayName);
            var username = "";
            var password = "";
            Debug.WriteLine("Disp: " + displayName + " User: " + bnetCredentials?.User + " Pass: <youwish>");
            if (bnetCredentials == null || (bnetCredentials?.User?.Length < 3 || bnetCredentials?.Pass?.Length < 3))
            {
                // The user has input some crap into the credentials, just delete and ask again
                Vault.RemoveCredentials("D2RMIM-" + displayName);
                Debug.WriteLine("Requesting username");
                username = Prompt.ShowDialog("Enter your battle.net username: ", "Account Data Required");
                Debug.WriteLine("Requesting password");
                password = Prompt.ShowDialog("Enter your battle.net password: ", "Account Data Required",true);
                Debug.WriteLine("Saving credentials in store");
                Vault.SetCredentials("D2RMIM-" + displayName, username, password); // Save credentials in windows credential store on local computer
                bnetCredentials = Vault.GetCredential("D2RMIM-" + displayName);
            }
            return bnetCredentials;
        }
        public static bool RemoveVaultCredentials(string displayName)
        {
            var bnetCredentials = Vault.GetCredential("D2RMIM-" + displayName);
            Debug.WriteLine("Disp: " + displayName + " User: " + bnetCredentials?.User + " Pass: <youwish>");
            if (bnetCredentials != null)
            {
                Vault.RemoveCredentials("D2RMIM-" + displayName);
                return true;
            }
            return false;
        
        }
    }
}
