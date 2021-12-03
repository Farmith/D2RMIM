using System.IO;
using System.Windows.Forms;

namespace MultiInstanceManager.Helpers
{
    public static class CustomCursor
    {
        public static Cursor FromByteArray(byte[] array)
        {
            using (MemoryStream memoryStream = new MemoryStream(array))
            {
                return new Cursor(memoryStream);
            }
        }
    }
}
