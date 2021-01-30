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
    public partial class calendar : Form
    {
        public calendar()
        {
            InitializeComponent();
            Icon = YorotTools.IconFromImage(Properties.Resources.calendar);
        }
    }
}
