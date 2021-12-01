using System;
using System.Runtime.InteropServices;

namespace MultiInstanceManager.Helpers
{
    public class ProcessHelper
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }
        [Flags]
        public enum ThreadAccess : uint
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [Flags]
        public enum SaferLevels : uint
        {
            Disallowed = 0,
            Untrusted = 0x1000,
            Constrained = 0x10000,
            NormalUser = 0x20000,
            FullyTrusted = 0x40000
        }

        [Flags]
        public enum SaferComputeTokenFlags : uint
        {
            None = 0x0,
            NullIfEqual = 0x1,
            CompareOnly = 0x2,
            MakeIntert = 0x4,
            WantFlags = 0x8
        }

        [Flags]
        public enum SaferScopes : uint
        {
            Machine = 1,
            User = 2
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessID;
            public Int32 dwThreadID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        public enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        public enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public uint Attributes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_GROUPS
        {
            public int GroupCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public SID_AND_ATTRIBUTES[] Groups;
        };
        public enum LogonFlags
        {
            /// <summary>
            /// Log on, then load the user's profile in the HKEY_USERS registry key. The function
            /// returns after the profile has been loaded. Loading the profile can be time-consuming,
            /// so it is best to use this value only if you must access the information in the
            /// HKEY_CURRENT_USER registry key.
            /// NOTE: Windows Server 2003: The profile is unloaded after the new process has been
            /// terminated, regardless of whether it has created child processes.
            /// </summary>
            /// <remarks>See LOGON_WITH_PROFILE</remarks>
            WithProfile = 1,
            /// <summary>
            /// Log on, but use the specified credentials on the network only. The new process uses the
            /// same token as the caller, but the system creates a new logon session within LSA, and
            /// the process uses the specified credentials as the default credentials.
            /// This value can be used to create a process that uses a different set of credentials
            /// locally than it does remotely. This is useful in inter-domain scenarios where there is
            /// no trust relationship.
            /// The system does not validate the specified credentials. Therefore, the process can start,
            /// but it may not have access to network resources.
            /// </summary>
            /// <remarks>See LOGON_NETCREDENTIALS_ONLY</remarks>
            NetCredentialsOnly
        }
        public enum CreationFlags

        {

            DefaultErrorMode = 0x04000000,
            NewConsole = 0x00000010,
            NewProcessGroup = 0x00000200,
            SeparateWOWVDM = 0x00000800,
            Suspended = 0x00000004,
            UnicodeEnvironment = 0x00000400,
            ExtendedStartupInfoPresent = 0x00080000
        }
        [
           DllImport("kernel32.dll",
              EntryPoint = "CloseHandle", SetLastError = true,
              CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)
        ]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessWithTokenW(UIntPtr hToken, LogonFlags dwLogonFlags, string? lpApplicationName, string? lpCommandLine, CreationFlags dwCreationFlags, IntPtr lpEnvironment, string? lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll",EntryPoint = "CreateProcessAsUser", SetLastError = true,CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern bool
           CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine,
                               ref SECURITY_ATTRIBUTES lpProcessAttributes, ref SECURITY_ATTRIBUTES lpThreadAttributes,
                               bool bInheritHandle, Int32 dwCreationFlags, IntPtr lpEnvrionment,
                               string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
                               ref PROCESS_INFORMATION lpProcessInformation);

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        public static extern bool DuplicateTokenEx(UIntPtr hExistingToken, Int32 dwDesiredAccess,
                            ref SECURITY_ATTRIBUTES lpThreadAttributes,
                            Int32 ImpersonationLevel, Int32 dwTokenType,
                            ref UIntPtr phNewToken);
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(UIntPtr ProcessHandle,
    UInt32 DesiredAccess, out UIntPtr TokenHandle);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SaferComputeTokenFromLevel(IntPtr LevelHandle, IntPtr InAccessToken, out IntPtr OutAccessToken, int dwFlags, IntPtr lpReserved);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SaferCreateLevel(
            SaferScopes dwScopeId,
            SaferLevels dwLevelId,
            int OpenFlags,
            out IntPtr pLevelHandle,
            IntPtr lpReserved);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SaferCloseLevel(
    IntPtr pLevelHandle);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SaferComputeTokenFromLevel(
          IntPtr levelHandle,
          IntPtr inAccessToken,
          out IntPtr outAccessToken,
          SaferComputeTokenFlags dwFlags,
          IntPtr lpReserved
        );
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool ConvertStringSidToSid(
                    string StringSid,
                    out IntPtr ptrSid
                    );
        [DllImport("kernel32.dll")]
        public static extern UIntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessID);
        
        [DllImport("kernel32.dll")]
        public static extern UIntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(UIntPtr hThread);
        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(UIntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(UIntPtr handle);
    }
}
