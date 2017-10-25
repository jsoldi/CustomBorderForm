using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomBorderForm
{
    [Flags]
    public enum WindowStyle
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = -2147483648, //0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
        WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU |
                                WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
        WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),
        WS_CHILDWINDOW = (WS_CHILD)
    }

    public static class HitTestValues
    {
        public static readonly int HTERROR = -2;
        public static readonly int HTTRANSPARENT = -1;
        public static readonly int HTNOWHERE = 0;
        public static readonly int HTCLIENT = 1;
        public static readonly int HTCAPTION = 2;
        public static readonly int HTSYSMENU = 3;
        public static readonly int HTGROWBOX = 4;
        public static readonly int HTMENU = 5;
        public static readonly int HTHSCROLL = 6;
        public static readonly int HTVSCROLL = 7;
        public static readonly int HTMINBUTTON = 8;
        public static readonly int HTMAXBUTTON = 9;
        public static readonly int HTLEFT = 10;
        public static readonly int HTRIGHT = 11;
        public static readonly int HTTOP = 12;
        public static readonly int HTTOPLEFT = 13;
        public static readonly int HTTOPRIGHT = 14;
        public static readonly int HTBOTTOM = 15;
        public static readonly int HTBOTTOMLEFT = 16;
        public static readonly int HTBOTTOMRIGHT = 17;
        public static readonly int HTBORDER = 18;
        public static readonly int HTOBJECT = 19;
        public static readonly int HTCLOSE = 20;
        public static readonly int HTHELP = 2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public int cbSize; // initialize this field using: Marshal.SizeOf(typeof(APPBARDATA));
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    public static class NativeConstants
    {
        public const int SM_CXSIZEFRAME = 32;
        public const int SM_CYSIZEFRAME = 33;
        public const int SM_CXPADDEDBORDER = 92;

        public const int GWL_ID = (-12);
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);

        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCRBUTTONUP = 0x00A5;

        public const uint TPM_LEFTBUTTON = 0x0000;
        public const uint TPM_RIGHTBUTTON = 0x0002;
        public const uint TPM_RETURNCMD = 0x0100;

        public static readonly IntPtr TRUE = new IntPtr(1);
        public static readonly IntPtr FALSE = new IntPtr(0);

        public const uint ABM_GETSTATE = 0x4;
        public const int ABS_AUTOHIDE = 0x1;
    }

    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32.dll")]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 Offset);

        [DllImport("shell32.dll")]
        public static extern int SHAppBarMessage(uint dwMessage, [In] ref APPBARDATA pData);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }
    }
}
