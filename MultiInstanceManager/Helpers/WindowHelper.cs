using MultiInstanceManager.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MultiInstanceManager.Helpers
{
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint BringWindowToTop(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern int AttachThreadInput(uint Attach, uint AttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetCurrentThreadId();

        public static void ShowMessage(string message)
        {
            _ = MessageBox.Show(message);
        }
        public static void ModifyWindowTitleName(Process process, string displayName)
        {
            var newTitle = "[ " + displayName + " ] Diablo II: Resurrected";
            Debug.WriteLine("Changing window title to: " + newTitle);
            WindowHelper.SetWindowText(process.MainWindowHandle, newTitle);
        }
        public static bool PriorityWindowFocus()
        {
            var clients = Process.GetProcessesByName(Constants.clientExecutableName);
            var focusWindow = WindowHelper.GetForegroundWindow();
            foreach (var client in clients)
            {
                if (focusWindow == client.MainWindowHandle)
                {
                    return true;
                }
            }
            if (focusWindow == Process.GetCurrentProcess().MainWindowHandle)
            {
                return true;
            }
            return false;
        }
        public static void forceForegroundWindow(IntPtr hwnd)
        {
            uint processId = 0;
            uint windowThreadProcessId = GetWindowThreadProcessId(GetForegroundWindow(), out processId);
            uint currentThreadId = GetCurrentThreadId();
            int CONST_SW_SHOW = 5;
            AttachThreadInput(windowThreadProcessId, currentThreadId, true);
            BringWindowToTop(hwnd);
            ShowWindow(hwnd, CONST_SW_SHOW);
            AttachThreadInput(windowThreadProcessId, currentThreadId, false);
        }
        public static IntPtr WinWait(string windowClass)
        {
            IntPtr windowHandle = WindowHelper.GetForegroundWindow();
            int maxWait = 2000;
            int waitTime = 0;
            while (!HasClass(windowHandle, windowClass) && waitTime < maxWait)
            {
                Thread.Sleep(100);
                windowHandle = WindowHelper.GetForegroundWindow();
                waitTime++;
            }
            return windowHandle;
        }
        public static void WinWaitClose(string windowClass, IntPtr handle)
        {
            int maxWait = 2000;
            int waitTime = 0;
            while (HasClass(handle, windowClass) && waitTime < maxWait)
            {
                Thread.Sleep(100);
                waitTime++;
            }
        }
        public static bool HasClass(IntPtr windowHandle, string windowClass)
        {
            StringBuilder lpsClassName = new StringBuilder();
            var classes = GetClassName(windowHandle, lpsClassName, 30);
            if (lpsClassName.ToString() == windowClass)
                return true;
            return false;
        }
    }
}
