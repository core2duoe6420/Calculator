using System;
using System.Windows.Forms;

namespace Net.AlexKing.Calculator.Forms
{
    public partial class FrmHistory : Form
    {
        public static string strH;
        public FrmHistory() {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void frmHistory_Load(object sender, EventArgs e) {
            richTextBox1.Text = strH;
        }
    }
}
