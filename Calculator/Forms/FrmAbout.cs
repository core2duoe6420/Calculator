using System;
using System.Windows.Forms;

namespace Net.AlexKing.Calculator.Forms
{
    public partial class FrmAbout : Form
    {
        public FrmAbout() {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e) {
            label1.Text = "关于我的计算器 Calculator 2.0\nC#作业\n作者：C# Boys Team" +
                "\n\n版本历史：\n2.0:完全重写整个内核，完全支持正负号运算，支持任何函数名和任意参数数量\n" +
                "1.41:添加上下键翻页和函数变量保存功能\n1.4:添加变量存储功能，重写函数菜单\n1.3:添加大数支持功能" +
                "\n1.2:添加函数功能，重写核心类运算符方法\n1.1:增加Inv按钮\n1.0:实现基本功能";
        }

        private void btnOK_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
