using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MultiInstanceManager.Modules
{
    public static class ProcessManager
    {
        #region Native (Extern) Structs
        [Flags]
        private enum ProcessAccessFlags : uint
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
        [DllImport("kernel32.dll")]
        private static extern UIntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessID);

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
        public static string PROCESS_NAME = "D2R";
        public static string MUTEX_STRING = "Instances";

        public static bool CloseExternalHandles()
        {
            bool success = false;

            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                if (i.ProcessName.Equals(PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
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
            UIntPtr hProcess = OpenProcess(ProcessAccessFlags.All, false, processID);

            UIntPtr x;
            bool success = DuplicateHandle(hProcess, handleToClose, IntPtr.Zero,
                out x, 0, false, DuplicateOptions.DUPLICATE_CLOSE_SOURCE);

            CloseHandle(hProcess);

            return success;
        }
        private static IntPtr GetAllHandles()
        {
            int bufferSize = 0x10000;  
            int actualSize;            
                        
            IntPtr pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize); 

            NTSTATUS queryResult = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation,
                pSysInfoBuffer, bufferSize, out actualSize);

            while (queryResult == NTSTATUS.STATUS_INFO_LENGTH_MISMATCH)
            {
                Marshal.FreeHGlobal(pSysInfoBuffer);

                bufferSize = bufferSize * 2;

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

            UIntPtr hProcess = OpenProcess(ProcessAccessFlags.DupHandle, false, (uint)process.Id);
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

            IntPtr pStringBuffer = Marshal.AllocHGlobal(bufferSize);

            NtQueryObject(handle, OBJECT_INFORMATION_CLASS.ObjectNameInformation, pStringBuffer, bufferSize, out bufferSize);

            CloseHandle(handle);    

            string handleName = ConvertToString(pStringBuffer);

            Marshal.FreeHGlobal(pStringBuffer);

            return handleName;
        }
        private static string ConvertToString(IntPtr pStringBuffer)
        {
            long baseAddress = pStringBuffer.ToInt64();

            int offset = IntPtr.Size * 2; // offset is based on 32/64bit OS

            string handleName = Marshal.PtrToStringUni(new IntPtr(baseAddress + offset));

            return handleName;
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
    }
}
