using System.Runtime.InteropServices;

namespace Exam.Services
{
    public static class LegacyService
    {
        [DllImport("User32.dll", EntryPoint = "MessageBoxA")]
        public static extern int MessageBox(System.IntPtr hWnd, string text, string caption, uint type);
    }
}
