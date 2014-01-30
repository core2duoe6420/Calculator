using System.Windows.Forms;

namespace Net.AlexKing.Calculator.Forms
{
    public partial class CalStoreMenu : UserControl
    {
        public int status = 0;
        public CalStoreMenu() {
            InitializeComponent();
            tt.SetToolTip(btnA, "0");
            tt.SetToolTip(btnB, "0");
            tt.SetToolTip(btnC, "0");
            tt.SetToolTip(btnD, "0");
            tt.SetToolTip(btnE, "0");
            tt.SetToolTip(btnF, "0");
            tt.SetToolTip(btnG, "0");
            tt.SetToolTip(btnH, "0");
            tt.SetToolTip(btnI, "0");
            tt.SetToolTip(btnJ, "0");
        }
    }
}
