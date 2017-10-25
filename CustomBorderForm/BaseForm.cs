using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace CustomBorderForm
{
    public class BaseForm : Form, ICustomLayoutForm
    {
        private int _TitleHeight = 24;

        private Padding _Border;
        private HoverLabel IconLabel;
        private HoverLabel TitleLabel;
        private HoverLabel CloseLabel;
        private SolidBrush _BackBrush;
        private Color BorderColor;
        private Color _ActiveBorderColor;
        private Color _InactiveBorderColor;
        private Color _ActiveTitleForeColor;
        private Color _InactiveTitleForeColor;
        private Color _TitleBarBackColor;
        private Color _ButtonHoverBackColor;
        private Color _ButtonDownBackColor;
        private Color _ButtonNormalForeColor;
        private Color _ButtonHoverForeColor;
        private Color _ButtonDownForeColor;
        private bool _IsActive;

        protected bool IsActive
        {
            get => _IsActive;
            private set { _IsActive = value; UpdateColors(); }
        }

        public override LayoutEngine LayoutEngine => new CustomLayoutEngine(this);
        public int ResizeBorder { get; set; } = 20;
        public Rectangle InnerClientRectangle { get; private set; }
        public Panel ClientPanel { get; }

        public Padding Border
        {
            get => _Border;
            set { _Border = value; PerformLayout(); }
        }

        public int TitleHeight
        {
            get => _TitleHeight;
            set { _TitleHeight = value; PerformLayout(); }
        }

        public Color ActiveBorderColor
        {
            get => _ActiveBorderColor;
            set { _ActiveBorderColor = value; UpdateColors(); }
        }

        public Color InactiveBorderColor
        {
            get => _InactiveBorderColor;
            set { _InactiveBorderColor = value; UpdateColors(); }
        }

        public Color ActiveTitleForeColor
        {
            get => _ActiveTitleForeColor;
            set { _ActiveTitleForeColor = value; UpdateColors(); }
        }

        public Color InactiveTitleForeColor
        {
            get => _InactiveTitleForeColor;
            set { _InactiveTitleForeColor = value; UpdateColors(); }
        }

        public Color TitleBarBackColor // Title back color and buttons normal back color
        {
            get => _TitleBarBackColor;
            set { _TitleBarBackColor = value; UpdateColors(); }
        }

        public Color ButtonHoverBackColor
        {
            get => _ButtonHoverBackColor;
            set { _ButtonHoverBackColor = value; UpdateColors(); }
        }

        public Color ButtonDownBackColor
        {
            get => _ButtonDownBackColor;
            set { _ButtonDownBackColor = value; UpdateColors(); }
        }

        public Color ButtonNormalForeColor
        {
            get => _ButtonNormalForeColor;
            set { _ButtonNormalForeColor = value; UpdateColors(); }
        }

        public Color ButtonHoverForeColor
        {
            get => _ButtonHoverForeColor;
            set { _ButtonHoverForeColor = value; UpdateColors(); }
        }

        public Color ButtonDownForeColor
        {
            get => _ButtonDownForeColor;
            set { _ButtonDownForeColor = value; UpdateColors(); }
        }

        public Font TitleFont
        {
            get => TitleLabel.Font;
            set => TitleLabel.Font = value;
        }

        public Image Image
        {
            get => IconLabel.Image;
            set => IconLabel.Image = value;
        }

        public override Color BackColor // Color of InnerClientRectangle
        {
            get => base.BackColor;
            set { base.BackColor = value; UpdateColors(); }
        }

        public override string Text // Text of title bar
        {
            get => base.Text;
            set { base.Text = value; TitleLabel.Text = value; }
        }

        public BaseForm(bool addClientPanel)
        {
            TitleLabel = new HoverLabel() { PassThrough = true, Text = Text, TextAlign = ContentAlignment.MiddleLeft }; // TODO: Should show ellipsis instead of word wrapping when form width is too small
            CloseLabel = new HoverLabel() { Text = "r", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Marlett", 8.5f) };
            IconLabel = new HoverLabel() { PassThrough = true, Text = "", ImageAlign = ContentAlignment.MiddleCenter };
            _BackBrush = new SolidBrush(SystemColors.Control);

            if (addClientPanel)
                ClientPanel = new Panel();

            Margin = new Padding(5, 5, 5, 5);
            Padding = new Padding(5, 5, 5, 5);
            Activated += (s, e) => IsActive = true;
            Deactivate += (s, e) => IsActive = false;
            CloseLabel.Click += (s, e) => Close();

            Disposed += BaseForm_Disposed;

            UpdateColors();
        }

        public BaseForm()
            : this(true) { }

        protected override void OnLoad(EventArgs loadEventArgs)
        {
            try
            {
                base.OnLoad(loadEventArgs);

                FormBorderStyle = FormBorderStyle.Sizable;
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, true);

                if (ClientPanel != null)
                    Controls.AddRange(new Control[] { IconLabel, TitleLabel, CloseLabel, ClientPanel });
                else
                    Controls.AddRange(new Control[] { IconLabel, TitleLabel, CloseLabel });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        private void BaseForm_Disposed(object sender, EventArgs e)
        {
            IconLabel.Dispose();
            TitleLabel.Dispose();
            CloseLabel.Dispose();
            ClientPanel?.Dispose();
            _BackBrush.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            using (var brush = new SolidBrush(BorderColor))
            {
                var reg = new Region(ClientRectangle);
                reg.Xor(new Rectangle(Border.Left, Border.Top, ClientRectangle.Width - Border.Horizontal, ClientRectangle.Height - Border.Vertical));
                e.Graphics.FillRegion(brush, reg);
            }
            e.Graphics.Clear(BorderColor);
            e.Graphics.FillRectangle(_BackBrush, InnerClientRectangle);
            base.OnPaint(e);
        }

        private void UpdateColors()
        {
            IconLabel.NormalBackColor = TitleBarBackColor;

            TitleLabel.NormalBackColor = TitleBarBackColor;
            TitleLabel.NormalForeColor = IsActive ? ActiveTitleForeColor : InactiveTitleForeColor;

            CloseLabel.NormalBackColor = TitleBarBackColor;
            CloseLabel.HoverBackColor = ButtonHoverBackColor;
            CloseLabel.DownBackColor = ButtonDownBackColor;
            CloseLabel.NormalForeColor = ButtonNormalForeColor;
            CloseLabel.HoverForeColor = ButtonHoverForeColor;
            CloseLabel.DownForeColor = ButtonDownForeColor;

            bool needsRefresh = false;

            if (_BackBrush.Color != BackColor)
            {
                _BackBrush.Dispose();
                _BackBrush = new SolidBrush(BackColor);
                needsRefresh = true;
            }

            var newBorderColor = IsActive ? ActiveBorderColor : InactiveBorderColor;

            if (newBorderColor != BorderColor)
            {
                BorderColor = newBorderColor;
                needsRefresh = true;
            }

            if (needsRefresh)
                Refresh();
        }

        public bool DoLayout(object container, LayoutEventArgs layoutEventArgs)
        {
            var size = ClientSize;
            var margin = WindowState == FormWindowState.Normal ? Border : Padding.Empty;
            var titleHeight = TitleHeight;
            var buttonWidth = TitleHeight;

            InnerClientRectangle = new Rectangle(
                margin.Left,
                margin.Top + TitleHeight,
                size.Width - margin.Horizontal,
                size.Height - (margin.Vertical + TitleHeight)
            );

            IconLabel.SetBounds(margin.Left, margin.Top, buttonWidth, titleHeight);
            TitleLabel.SetBounds(margin.Left + buttonWidth, margin.Top, size.Width - (margin.Horizontal + buttonWidth * 2), titleHeight);
            CloseLabel.SetBounds(size.Width - (margin.Right + buttonWidth), margin.Top, titleHeight, buttonWidth);

            ClientPanel?.SetBounds(
                InnerClientRectangle.Left + Padding.Left,
                InnerClientRectangle.Top + Padding.Top,
                InnerClientRectangle.Width - Padding.Horizontal,
                InnerClientRectangle.Height - Padding.Vertical
            );

            return false;
        }

        private int? GetHitPos(Point point)
        {
            var size = ClientSize;

            if (point.Y < ResizeBorder)
            {
                if (point.X < ResizeBorder)
                    return HitTestValues.HTTOPLEFT;
                else if (point.X > size.Width - ResizeBorder)
                    return HitTestValues.HTTOPRIGHT;
                else
                    return HitTestValues.HTTOP;
            }
            else if (point.Y > size.Height - ResizeBorder)
            {
                if (point.X < ResizeBorder)
                    return HitTestValues.HTBOTTOMLEFT;
                else if (point.X > size.Width - ResizeBorder)
                    return HitTestValues.HTBOTTOMRIGHT;
                else
                    return HitTestValues.HTBOTTOM;
            }
            else if (point.X < ResizeBorder)
                return HitTestValues.HTLEFT;
            else if (point.X > size.Width - ResizeBorder)
                return HitTestValues.HTRIGHT;
            else
                return null;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x83: // WM_NCCALCSIZE
                    WmNCCalcSize(ref m);
                    break;
                case 0x84: // WM_NCHITTEST
                    Point pos = new Point(m.LParam.ToInt32());
                    pos = PointToClient(pos);
                    var cursor = PointToClient(Cursor.Position);
                    int? hitPos;

                    if (new Rectangle(TitleLabel.Location, TitleLabel.Size).Contains(pos))
                        m.Result = (IntPtr)2; // HTCAPTION
                    else if (WindowState == FormWindowState.Normal && (hitPos = GetHitPos(cursor)).HasValue)
                        m.Result = (IntPtr)hitPos.Value;
                    else
                        base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private FormWindowState GetMinMaxState()
        {
            var s = NativeMethods.GetWindowLong(Handle, NativeConstants.GWL_STYLE);
            var max = (s & (int)WindowStyle.WS_MAXIMIZE) > 0;
            if (max) return FormWindowState.Maximized;
            var min = (s & (int)WindowStyle.WS_MINIMIZE) > 0;
            if (min) return FormWindowState.Minimized;
            return FormWindowState.Normal;
        }

        private void WmNCCalcSize(ref Message m)
        {
            // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmessages/wm_nccalcsize.asp
            // http://groups.google.pl/groups?selm=OnRNaGfDEHA.1600%40tk2msftngp13.phx.gbl
            var r = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
            var max = GetMinMaxState() == FormWindowState.Maximized;

            if (max)
            {
                var x = NativeMethods.GetSystemMetrics(NativeConstants.SM_CXSIZEFRAME);
                var y = NativeMethods.GetSystemMetrics(NativeConstants.SM_CYSIZEFRAME);
                var p = NativeMethods.GetSystemMetrics(NativeConstants.SM_CXPADDEDBORDER);
                var w = x + p;
                var h = y + p;

                r.left += w;
                r.top += h;
                r.right -= w;
                r.bottom -= h;

                var appBarData = new APPBARDATA();
                appBarData.cbSize = Marshal.SizeOf(typeof(APPBARDATA));
                var autohide = (NativeMethods.SHAppBarMessage(NativeConstants.ABM_GETSTATE, ref appBarData) & NativeConstants.ABS_AUTOHIDE) != 0;
                if (autohide) r.bottom -= 1;

                Marshal.StructureToPtr(r, m.LParam, true);
            }

            m.Result = IntPtr.Zero;
        }
    }
}
