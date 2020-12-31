using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yorot
{
    public class Settings
    {
        public Settings()
        {

        }

        public AppMan AppMan { get; set; } = new AppMan(YorotGlobal.UserApp);
    }
}
