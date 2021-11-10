using MultiInstanceManager.Modules;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MultiInstanceManager.Helpers
{
    public static class AutomationHelper
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public static Bitmap GetScreenshot(Rect rect)
        {
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            // var CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
        public static Point FindUsernameBox()
        {
            var rect = GetForegroundWindowRect();

            var bitmap = GetScreenshot(rect);
            var delim = bitmap.Width / 3;
            var lineA = delim * 2;
            var lineMax = bitmap.Height / 2;
            var findColor = Constants.usernameBoxColor;

            for (var i = 0; i < lineMax; i++)
            {
                var color = bitmap.GetPixel(lineA, i);
                if (color == findColor)
                {
                    return new Point(rect.Left + lineA, rect.Top + i);
                }
            }

            return new Point(0, 0);
        }
        public static Point FindLoginButton(int xline, int yline)
        {
            var rect = GetForegroundWindowRect();

            var bitmap = GetScreenshot(rect);
            var delim = bitmap.Width / 3;
            var lineA = delim * 2;
            var lineMax = bitmap.Height;
            var findColor = Constants.loginButtonColor;

            for (var i = 0; i < lineMax; i++)
            {
                var color = bitmap.GetPixel(lineA, i);
                if (color == findColor)
                {
                    return new Point(rect.Left + lineA, rect.Top + i);
                }
            }

            return new Point(0, 0);
        }
        public static Rect GetForegroundWindowRect()
        {
            var foregroundWindowsHandle = WindowHelper.GetForegroundWindow();
            var rect = new Rect();
            WindowHelper.GetWindowRect(foregroundWindowsHandle, ref rect);
            return rect;
        }
        public static Point FindLauncherButton()
        {
            var rect = GetForegroundWindowRect();
            var bitmap = GetScreenshot(rect);
            var delim = bitmap.Width / 3;
            var lineA = 150;
            var lineMax = bitmap.Height - 1;
            var findColor = Constants.loginButtonColor;
            for (var i = lineMax; i > 0; i--)
            {
                var color = bitmap.GetPixel(lineA, i);
                if (color == findColor)
                {
                    return new Point(rect.Left + lineA, rect.Top + i);
                }
            }

            return new Point(0, 0);
        }

        public static void StartGameWithLauncherButton()
        {
            Thread.Sleep(100);
            var button = AutomationHelper.FindLauncherButton();
            while (button.X == 0)
            {
                Thread.Sleep(100);
                button = AutomationHelper.FindLauncherButton();
            }
            if (button.X > 0 && button.Y > 0)
            {
                LeftMouseClick(button.X, button.Y - 10);
            }
        }
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        public static void FillLauncherCredentials(Process launcher, string user, string pass)
        {
            var where = AutomationHelper.FindUsernameBox();
            if (where.X > 0 && where.Y > 0)
            {
                AutomationHelper.LeftMouseClick(where.X, where.Y + 5);

                // We should be in the text-box now, we hope
                SendKeys.SendWait("^(a)");
                foreach (char c in user)
                {
                    SendKeys.SendWait(c.ToString());
                    Thread.Sleep(2);
                }
                Thread.Sleep(100);
                SendKeys.SendWait("{TAB}");
                Debug.WriteLine("Filling out password");
                Thread.Sleep(5);
                foreach (char c in pass)
                {
                    SendKeys.SendWait(c.ToString());
                    Thread.Sleep(2);
                }
                Thread.Sleep(100);
                Debug.WriteLine("Finding login button");
                var button = AutomationHelper.FindLoginButton(where.X, where.Y);
                while (button.X == 0)
                {
                    Thread.Sleep(100);
                    button = AutomationHelper.FindLoginButton(where.X, where.Y);
                }
                // Sleep a little, then tab to it
                Debug.WriteLine("Tabbing to button!");
                Thread.Sleep(100);
                SendKeys.SendWait("{TAB}");
                Thread.Sleep(15);
                SendKeys.SendWait("{TAB}");
                Thread.Sleep(5);
                SendKeys.SendWait("{ENTER}");
                // We should be logging in now!
                Debug.WriteLine("Should be logging in now");
            }
        }
        public static Point GetRandomPointWithinRect(Rect rect)
        {
            var y = new System.Random().Next(rect.Top+5, rect.Bottom-5);
            var x = new System.Random().Next(rect.Left+5, rect.Right-5);
            return new Point(x, y);
        }
        public static void ClickFreneticallyInsideWindow(CancellationToken ct, Process process,int intervalSeconds=5)
        {
            ct.ThrowIfCancellationRequested();
            Thread.Sleep(10000);    // Sleep for an arbitrary, 10s to let window boot
            var keepGoing = true;
            while(keepGoing)
            {
                if (ct.IsCancellationRequested)
                {
                    // Clean up here, then...
                    keepGoing = false;
                }
                else
                {
                    Debug.WriteLine("Click...");
                    WindowHelper.forceForegroundWindow(process.MainWindowHandle);
                    var rect = GetForegroundWindowRect();
                    var randomLocation = GetRandomPointWithinRect(rect);
                    AutomationHelper.LeftMouseClick(randomLocation.X, randomLocation.Y);
                    Thread.Sleep(1000 * intervalSeconds);
                }
            }
        }
    }
}
