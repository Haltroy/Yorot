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
    public partial class fileman : Form
    {
        public fileman(string[] args)
        {
            InitializeComponent();
            Icon = HTAlt.Tools.IconFromImage(Properties.Resources.fileman);
            if(args != null)
            {
                for(int i = 0; i < args.Length;i++)
                {
                    Console.WriteLine("[" + i + "] " + args[i]);
                }
            }
        }
    }
}
