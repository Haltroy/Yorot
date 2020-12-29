using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Korot_Win32
{
    public partial class frmTab : Form
    {
        public frmTab()
        {
            InitializeComponent();
        }

        public bool AutoTabColor { get; internal set; }
        public Color TabColor { get; internal set; }
    }
}
