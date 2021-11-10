#nullable enable
using CredentialManagement;
using MultiInstanceManager.Structs;

namespace MultiInstanceManager.Modules
{
    public static class Vault
    {
        public static Credentials? GetCredential(string target)
        {
            var cm = new Credential { Target = target };
            if (!cm.Load())
            {
                return null;
            }

            return new Credentials(cm.Username, cm.Password);
        }

        public static bool SetCredentials(
             string target, string username, string password, PersistanceType persistenceType = PersistanceType.LocalComputer)
        {
            return new Credential
            {
                Target = target,
                Username = username,
                Password = password,
                PersistanceType = persistenceType
            }.Save();
        }

        public static bool RemoveCredentials(string target)
        {
            return new Credential { Target = target }.Delete();
        }
    }
}
