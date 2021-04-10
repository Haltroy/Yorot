using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yorot.UI.SystemApp
{
    public partial class console : Form
    {
        public console()
        {
            InitializeComponent();
            Icon = HTAlt.Tools.IconFromImage(Properties.Resources.console);
        }
    }
}
