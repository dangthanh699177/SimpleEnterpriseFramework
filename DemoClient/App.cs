using SEP.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public class App : IApp
    {
        IFormMain _frmMain;

        public App(IFormMain frmMain)
        {
            _frmMain = frmMain;
        }

        public void Run()
        {
            //_frmMain.ShowDialog();
        }
    }
}
