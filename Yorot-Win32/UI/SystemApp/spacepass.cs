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
    public partial class spacepass : Form
    {
        public spacepass()
        {
            InitializeComponent();
            Icon = Yorot.Tools.IconFromImage(Properties.Resources.spacepass);
        }
    }
}
