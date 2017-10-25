using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomBorderForm
{
    public class HoverLabel : Label
    {
        private enum MouseState { Normal, Hover, Down }

        private MouseState _CurrentMouseState;

        private Color _NormalBackColor = Color.White;
        private Color _HoverBackColor = Color.FromArgb(213, 225, 242);
        private Color _DownBackColor = Color.FromArgb(163, 189, 227);

        private Color _NormalForeColor = Color.FromArgb(68, 68, 68);
        private Color _HoverForeColor = Color.FromArgb(62, 109, 181);
        private Color _DownForeColor = Color.FromArgb(25, 71, 138);

        private MouseState CurrentMouseState
        {
            get => _CurrentMouseState;
            set => SetCurrentMouseState(value);
        }

        public Color NormalBackColor
        {
            get => _NormalBackColor;
            set { _NormalBackColor = value; UpdateColors(); }
        }

        public Color HoverBackColor
        {
            get => _HoverBackColor;
            set { _HoverBackColor = value; UpdateColors(); }
        }

        public Color DownBackColor
        {
            get => _DownBackColor;
            set { _DownBackColor = value; UpdateColors(); }
        }

        public Color NormalForeColor
        {
            get => _NormalForeColor;
            set { _NormalForeColor = value; UpdateColors(); }
        }

        public Color HoverForeColor
        {
            get => _HoverForeColor;
            set { _HoverForeColor = value; UpdateColors(); }
        }

        public Color DownForeColor
        {
            get => _DownForeColor;
            set { _DownForeColor = value; UpdateColors(); }
        }

        public bool PassThrough { get; set; }

        public HoverLabel()
        {
            UseCompatibleTextRendering = false;
            UpdateColors();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (PassThrough && m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
        }

        private (Color Back, Color Fore) GetCurrentColors()
        {
            switch (CurrentMouseState)
            {
                case MouseState.Normal:
                    return (NormalBackColor, NormalForeColor);
                case MouseState.Hover:
                    return (HoverBackColor, HoverForeColor);
                case MouseState.Down:
                    return (DownBackColor, DownForeColor);
                default:
                    throw new Exception("Unexpected mouse state.");
            }
        }

        private void UpdateColors()
        {
            var colors = GetCurrentColors();
            BackColor = colors.Back;
            ForeColor = colors.Fore;
        }

        private void SetCurrentMouseState(MouseState value)
        {
            if (_CurrentMouseState != value)
            {
                _CurrentMouseState = value;
                OnMouseStateChanged(_CurrentMouseState);
            }
        }

        private void OnMouseStateChanged(MouseState e)
        {
            UpdateColors();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            CurrentMouseState = MouseState.Hover;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                CurrentMouseState = MouseState.Hover;

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            CurrentMouseState = MouseState.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                CurrentMouseState = MouseState.Down;

            base.OnMouseDown(e);
        }
    }
}
