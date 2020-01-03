using SEP.Forms;

namespace SEP
{
    public class App : IApp
    {
        IFormMain _frmMain;

        public App(IFormMain frmMain)
        {
            _frmMain = frmMain;
        }

        public bool Run()
        {
            return _frmMain.Run();
        }
    }
}
