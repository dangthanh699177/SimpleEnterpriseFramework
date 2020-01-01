using SEP.Forms;

namespace DemoForm
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
            _frmMain.Run();
        }
    }
}
