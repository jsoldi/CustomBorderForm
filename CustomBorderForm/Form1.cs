using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomBorderForm
{
    public partial class Form1 : BaseForm
    {
        private readonly Image IconImage;

        public Form1()
        {
            InitializeComponent();

            using (var icon = new Icon(Icon, 16, 16))
                IconImage = icon.ToBitmap();

            Border = new Padding(2, 2, 2, 2);
            Padding = new Padding(8, 8, 8, 8);
            TitleHeight = 32;
            ActiveBorderColor = ColorTranslator.FromHtml("#007acc");
            InactiveBorderColor = ColorTranslator.FromHtml("#48484b");
            ActiveTitleForeColor = ColorTranslator.FromHtml("#909090");
            InactiveTitleForeColor = ColorTranslator.FromHtml("#48484b");
            TitleBarBackColor = ColorTranslator.FromHtml("#2d2d30");
            ButtonHoverBackColor = ColorTranslator.FromHtml("#3f3f41");
            ButtonDownBackColor = ColorTranslator.FromHtml("#007acc");
            ButtonNormalForeColor = ColorTranslator.FromHtml("#d0d0d0");
            ButtonHoverForeColor = ColorTranslator.FromHtml("#f1f1f1");
            ButtonDownForeColor = ColorTranslator.FromHtml("#ffffff");
            TitleFont = new Font("Segoe UI", 8);
            BackColor = ColorTranslator.FromHtml("#2d2d30");
            Text = "The DarkForm";
            Image = IconImage;

            var button = new Button() { Text = "Hola", Location = new Point(10, 20), Size = new Size(200, 75) };
            ClientPanel.Controls.Add(button);

            Disposed += Form1_Disposed;
        }

        private void Form1_Disposed(object sender, EventArgs e)
        {
            IconImage.Dispose();
        }
    }
}
