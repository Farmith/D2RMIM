using MultiInstanceManager.Modules;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

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
            var foregroundWindowsHandle = WindowHelper.GetForegroundWindow();
            var rect = new Rect();
            WindowHelper.GetWindowRect(foregroundWindowsHandle, ref rect);

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
            var foregroundWindowsHandle = WindowHelper.GetForegroundWindow();
            var rect = new Rect();
            WindowHelper.GetWindowRect(foregroundWindowsHandle, ref rect);

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
        public static Point FindLauncherButton()
        {
            var foregroundWindowsHandle = WindowHelper.GetForegroundWindow();
            var rect = new Rect();
            WindowHelper.GetWindowRect(foregroundWindowsHandle, ref rect);

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
    }
}
