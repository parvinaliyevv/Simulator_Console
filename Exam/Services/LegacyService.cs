using System.Runtime.InteropServices;

namespace Exam.Services
{
    public class LegacyService
    {
        [DllImport("User32.dll", EntryPoint = "MessageBoxA")]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
    }
}
