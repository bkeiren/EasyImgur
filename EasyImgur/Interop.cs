using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EasyImgur
{
    internal static class Interop
    {
        // From Commctrl.h of the Winapi
        // http://i.imgur.com/abaok1G.png
        // ReSharper disable once InconsistentNaming
        private const uint EM_SETCUEBANNER = 0x1501;

        public static IntPtr SetCueBanner(TextBox tb, string text, bool showEvenWhileFocused)
        {
            return SendMessage(tb.Handle, EM_SETCUEBANNER, Convert.ToUInt32(showEvenWhileFocused), text);
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb761639(v=vs.85).aspx
        [DllImport("user32.dll", CharSet=CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
    }
}
