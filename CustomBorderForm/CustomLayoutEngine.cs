using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace CustomBorderForm
{
    public interface ICustomLayoutForm
    {
        bool DoLayout(object container, LayoutEventArgs layoutEventArgs);
    }

    public class CustomLayoutEngine : LayoutEngine
    {
        private readonly ICustomLayoutForm Form;

        public CustomLayoutEngine(ICustomLayoutForm form)
        {
            Form = form;
        }

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            return Form.DoLayout(container, layoutEventArgs);
        }
    }
}
