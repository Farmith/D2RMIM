#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using MultiInstanceManager.Helpers;
using System.Security.Principal;

namespace MultiInstanceManager.Modules
{
    public static class ProcessManager
    {
        #region Native (Extern) Structs
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public UIntPtr lpReserved2;
            public UIntPtr hStdInput;
            public UIntPtr hStdOutput;
            public UIntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public UIntPtr hProcess;
            public UIntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }
        [Flags]
        private enum NTSTATUS : uint
        {
            STATUS_SUCCESS = 0x00000000,
            STATUS_INFO_LENGTH_MISMATCH = 0xC0000004
        }
        [Flags]
        private enum OBJECT_INFORMATION_CLASS : uint
        {
            ObjectBasicInformation = 0,
            ObjectNameInformation = 1,
            ObjectTypeInformation = 2,
            ObjectAllTypesInformation = 3,
            ObjectHandleInformation = 4
        }
        [Flags]
        private enum SYSTEM_INFORMATION_CLASS : uint
        {
            SystemHandleInformation = 16
        } 

        [Flags]
        private enum FILE_INFORMATION_CLASS
        {
            FileNameInformation = 9
        }
        [Flags]
        private enum DuplicateOptions : uint
        {
            DUPLICATE_CLOSE_SOURCE = 0x00000001,
            DUPLICATE_SAME_ACCESS = 0x00000002
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SYSTEM_HANDLE_INFORMATION
        {
            public UInt32 OwnerPID;
            public Byte ObjectType;
            public Byte HandleFlags;
            public UInt16 HandleValue;
            public UIntPtr ObjectPointer;
            public IntPtr AccessMask;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        private struct OBJECT_BASIC_INFORMATION
        {
            public UInt32 Attributes;
            public UInt32 GrantedAccess;
            public UInt32 HandleCount;
            public UInt32 PointerCount;
            public UInt32 PagedPoolUsage;
            public UInt32 NonPagedPoolUsage;
            public UInt32 Reserved1;
            public UInt32 Reserved2;
            public UInt32 Reserved3;
            public UInt32 NameInformationLength;
            public UInt32 TypeInformationLength;
            public UInt32 SecurityDescriptorLength;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
        }
        #endregion
        #region Native (Extern) Methods

        [DllImport("ntdll.dll")]
        private static extern NTSTATUS NtQueryObject(UIntPtr ObjectHandle, OBJECT_INFORMATION_CLASS ObjectInformationClass,
            IntPtr ObjectInformation, int ObjectInformationLength, out int ReturnLength);
        
        [DllImport("ntdll.dll")]
        private static extern NTSTATUS NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass,
            IntPtr SystemInformation, int SystemInformationLength, out int ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(UIntPtr hObject);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DuplicateHandle(UIntPtr hSourceProcessHandle,
            IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out UIntPtr lpTargetHandle,
            uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, DuplicateOptions dwOptions);
        #endregion

        public static uint NAMEDPIPEMASK = 0x0012019F;
        public static string MUTEX_STRING = "Instances";

        public static bool CloseExternalHandles(string name)
        {
            bool success = false;

            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                if (i.ProcessName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (KillHandle(i, MUTEX_STRING))
                    {
                        success = true;
                    }
                }
            }

            return success;
        }
        private static bool CloseHandleEx(uint processID, IntPtr handleToClose)
        {
            UIntPtr hProcess = ProcessHelper.OpenProcess(ProcessHelper.ProcessAccessFlags.All, false, processID);

            UIntPtr x;
            bool success = DuplicateHandle(hProcess, handleToClose, IntPtr.Zero,
                out x, 0, false, DuplicateOptions.DUPLICATE_CLOSE_SOURCE);

            CloseHandle(hProcess);

            return success;
        }
        private static IntPtr GetAllHandles()
        {
            int initialSize = 0x10000;
            int bufferSize = 0x10000;  
            int actualSize;            
                        
            IntPtr pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize); 

            NTSTATUS queryResult = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation,
                pSysInfoBuffer, bufferSize, out actualSize);

            while (queryResult == NTSTATUS.STATUS_INFO_LENGTH_MISMATCH)
            {
                Marshal.FreeHGlobal(pSysInfoBuffer);

                bufferSize = bufferSize + initialSize; // * 2;

                pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize);

                queryResult = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation,
                    pSysInfoBuffer, bufferSize, out actualSize);
            }

            if (queryResult == NTSTATUS.STATUS_SUCCESS)
            {
                return pSysInfoBuffer; 
            }
            else
            {
                Marshal.FreeHGlobal(pSysInfoBuffer);
                return IntPtr.Zero;
            }
        }
        private static List<SYSTEM_HANDLE_INFORMATION> GetHandles(Process targetProcess, IntPtr pSysHandles)
        {
            List<SYSTEM_HANDLE_INFORMATION> processHandles = new List<SYSTEM_HANDLE_INFORMATION>();

            Int64 pBaseAddr = pSysHandles.ToInt64();
            Int64 Offset;                           
            IntPtr pLocation;                              

            SYSTEM_HANDLE_INFORMATION handleInfo;

            int numHandles = Marshal.ReadInt32(pSysHandles);

            for (int i = 0; i < numHandles; i++)
            {
                Offset = IntPtr.Size + i * Marshal.SizeOf(typeof(SYSTEM_HANDLE_INFORMATION));

                pLocation = new IntPtr(pBaseAddr + Offset);

                handleInfo = (SYSTEM_HANDLE_INFORMATION)
                    Marshal.PtrToStructure(pLocation, typeof(SYSTEM_HANDLE_INFORMATION));

                if (handleInfo.OwnerPID == targetProcess.Id)
                {
                    processHandles.Add(handleInfo);
                }
            }

            return processHandles;
        }

        public static bool KillHandle(Process process, string handleName)
        {
            bool success = false;

            IntPtr pSysHandles = GetAllHandles();

            if (pSysHandles == IntPtr.Zero)
            {
                return success;
            }
            List<SYSTEM_HANDLE_INFORMATION> processHandles = GetHandles(process, pSysHandles);

            Marshal.FreeHGlobal(pSysHandles);

            UIntPtr hProcess = ProcessHelper.OpenProcess(ProcessHelper.ProcessAccessFlags.DupHandle, false, (uint)process.Id);
            foreach (SYSTEM_HANDLE_INFORMATION handleInfo in processHandles)
            {
                string name = GetHandleName(handleInfo, hProcess);
                if (name.Contains(handleName))
                {
                    if (CloseHandleEx(handleInfo.OwnerPID, new IntPtr(handleInfo.HandleValue)))
                    {
                        success = true;
                    }
                }
            }
            CloseHandle(hProcess);

            return success;
        }
        private static string GetHandleName(SYSTEM_HANDLE_INFORMATION targetHandleInfo, UIntPtr hProcess)
        {
            if (targetHandleInfo.AccessMask.ToInt64() == NAMEDPIPEMASK)
            {
                return String.Empty;
            }

            IntPtr process = Process.GetCurrentProcess().Handle;
            UIntPtr handle;

            DuplicateHandle(hProcess, new IntPtr(targetHandleInfo.HandleValue), process,
                out handle, 0, false, DuplicateOptions.DUPLICATE_SAME_ACCESS);

            int bufferSize = GetHandleNameLength(handle);

            Debug.WriteLine("Trying to allocate: " + bufferSize.ToString() + " bytes of memory");
            try
            {
                IntPtr pStringBuffer = Marshal.AllocHGlobal(bufferSize);

                NtQueryObject(handle, OBJECT_INFORMATION_CLASS.ObjectNameInformation, pStringBuffer, bufferSize, out bufferSize);

                CloseHandle(handle);

                string handleName = ConvertToString(pStringBuffer);

                Marshal.FreeHGlobal(pStringBuffer);

                return handleName;
            } catch (Exception e)
            {
                Console.WriteLine("Could not allocate memory: " + e.ToString());
            }
            return String.Empty;
        }
        private static string ConvertToString(IntPtr pStringBuffer)
        {
            long baseAddress = pStringBuffer.ToInt64();

            int offset = IntPtr.Size * 2; // offset is based on 32/64bit OS

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string handleName = Marshal.PtrToStringUni(new IntPtr(baseAddress + offset));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8603 // Possible null reference return.
            return handleName;
#pragma warning restore CS8603 // Possible null reference return.
        }
        private static int GetHandleNameLength(UIntPtr handle)
        {
            int OBISize = Marshal.SizeOf(typeof(OBJECT_BASIC_INFORMATION));  
            IntPtr OBIBuffer = Marshal.AllocHGlobal(OBISize);            

            NtQueryObject(handle, OBJECT_INFORMATION_CLASS.ObjectBasicInformation, OBIBuffer, OBISize, out OBISize);

            OBJECT_BASIC_INFORMATION objInfo =
                (OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(OBIBuffer, typeof(OBJECT_BASIC_INFORMATION));

            Marshal.FreeHGlobal(OBIBuffer);   

            if (objInfo.NameInformationLength == 0)
            {
                return 0x100;    //reserve 256 bytes, since nameinfolength = 0 for filenames
            }
            else
            {
                return (int)objInfo.NameInformationLength;
            }
        }
        public static bool blizzardProcessesExists()
        {
            var launchers = Process.GetProcessesByName(Constants.launcherExecutableName);
            var battlenets = Process.GetProcessesByName(Constants.battleNetExecutableName);
            
            if (launchers.Length > 0 || battlenets.Length > 0)
                return true;
            return false;
        }
        public static bool MatchProcess(Process process)
        {
            try
            {
                Process exists = Process.GetProcessById(process.Id);
                if (exists.Id == process.Id)
                {
                    return true;
                }
            } catch (Exception ex)
            {
                Log.Debug("Process has died prematurely");
                Log.Debug(ex.ToString());
            }
            return false;
        }
        public static Process? ProcessWait(string processName,int processCounter)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            int maxWait = 2000;
            int waitTime = 0;
            while (processes.Length < processCounter && waitTime < maxWait)
            {
                Thread.Sleep(100);
                processes = Process.GetProcessesByName(processName);
                waitTime++;
            }
            if (processes.Length == processCounter)
            {
                var process = processes[processCounter - 1];
                var keeptrying = true;
                while (!process.HasExited && keeptrying)
                {
                    process.Refresh();
                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        keeptrying = false;
                    }
                    else
                    {
                        // We sleep for 5 ms untill we find a MainWindowHandle
                        Thread.Sleep(5);
                    }
                }
                return process;
            }
            return null;
        }
        public static Process? GetProcessByHandle(IntPtr handle)
        {
            Process process;
            bool go = true;
            while (go)
            {
                try
                {
                    process = Process.GetProcesses().Single(p => p.Id != 0 && p.MainWindowHandle == handle);
                    if (process.Id > 0)
                    {
                        go = false;
                        return process;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Need to wait a bit, process not ready: " + e.Message.ToString());
                    Thread.Sleep(10);
                }
            }
            return null;
        }
        public static Process StartProcess(string cmd,bool wait = false)
        {
            var process = Process.Start("explorer",cmd);
            /*
            process.StartInfo.FileName = "Explorer.exe";
            process.StartInfo.Arguments = cmd;
            process.Start();
            Log.Debug("Command started: " + cmd);
            if (wait)
            {
                var name = process.ProcessName;
                var processes = Process.GetProcessesByName(name);
                var alive = false;
                // Sanity check to make sure the process is alive before we return it. trust me its required.
                while (!alive)
                {
                    foreach (var p in processes)
                    {
                        if (p.Id == process.Id && p.MainWindowHandle != IntPtr.Zero)
                        {
                            alive = true;
                        }
                    }
                    if (!alive)
                        processes = Process.GetProcessesByName(name);
                }
            }
            */
            return process;
        }
        public static Process DeElevatedProcess(string args)
        {
            var shellWnd = ProcessHelper.GetShellWindow();
            if (shellWnd == IntPtr.Zero)
            {
                Log.Debug("Could not get a shell window");
                return new Process();
            }

            uint shellProcessId;
            WindowHelper.GetWindowThreadProcessId(shellWnd, out shellProcessId);

            var hShellProcess = ProcessHelper.OpenProcess(ProcessHelper.ProcessAccessFlags.QueryInformation, false, shellProcessId);

            var hShellToken = UIntPtr.Zero;
            if (!ProcessHelper.OpenProcessToken(hShellProcess, 2 /* TOKEN_DUPLICATE */, out hShellToken))
            {
                Log.Debug("Can not open process token");
                return new Process();
            }

            int tokenAccess = 8 /*TOKEN_QUERY*/ | 1 /*TOKEN_ASSIGN_PRIMARY*/ | 2 /*TOKEN_DUPLICATE*/ | 0x80 /*TOKEN_ADJUST_DEFAULT*/ | 0x100 /*TOKEN_ADJUST_SESSIONID*/;
            var hToken = IntPtr.Zero;
            var dupTokenHandle = UIntPtr.Zero;

            ProcessHelper.SECURITY_ATTRIBUTES sa = new ProcessHelper.SECURITY_ATTRIBUTES();
            sa.bInheritHandle = true;
            sa.Length = Marshal.SizeOf(sa);
            sa.lpSecurityDescriptor = (IntPtr)0;

            ProcessHelper.DuplicateTokenEx(hShellToken, tokenAccess, ref sa, 2 /* SecurityImpersonation */, 1 /* TokenPrimary */, ref dupTokenHandle);

            var pi = new ProcessHelper.PROCESS_INFORMATION();
            var si = new ProcessHelper.STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            if (!ProcessHelper.CreateProcessWithTokenW(dupTokenHandle, 0, null, args, 0, IntPtr.Zero, null, ref si, out pi))
            {
                Log.Debug("Unable to create process with token");
                Log.Debug(Marshal.GetLastWin32Error().ToString());
                return new Process();
            }
            var proc = Process.GetProcessById(pi.dwProcessID);
            if (proc.Id == pi.dwProcessID)
            {
                Log.Debug("We probably started the process as a de-elevated one now: " + pi.dwProcessID);
                return proc;
            }
            return new Process();
        }
        
        public static Boolean IsProcessRunning(Process process)
        {
            try
            {
                var id = Process.GetProcessById(process.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }
    }
}
