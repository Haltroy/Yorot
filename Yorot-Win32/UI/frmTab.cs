using System.Drawing;
using System.Windows.Forms;

namespace Yorot
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
