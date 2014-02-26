using Net.AlexKing.Calculator.Core;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Net.AlexKing.Calculator.Forms
{
    public partial class FrmCalc : Form
    {
        private bool degOrRad = false;
        private bool needToClear = false;
        private bool isInv = false;
        private static bool bigNumberSupport = false;
        private static DefaultCalculateFactory normalFactory = new DefaultCalculateFactory();
        private static BigNumberCalculateFactory bigNumberFactory = new BigNumberCalculateFactory();
        public static bool BigNumberSupport {
            get { return bigNumberSupport; }
        }
        private struct history
        {
            public string[] str;
            public int top;
            public int curr;
            public int count;
            public history(int length) {
                str = new string[length];
                top = 0;
                curr = 0;
                count = 0;
            }
        };
        private history historyRecord;
        public FrmCalc() {
            InitializeComponent();
            historyRecord = new history(20);
            rMenu.btnA.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnB.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnC.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnD.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnE.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnF.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnG.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnH.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnI.Click += new System.EventHandler(this.rMenu_click);
            rMenu.btnJ.Click += new System.EventHandler(this.rMenu_click);
            rMenu.BringToFront();

            Selector operandSelector = new DefaultSelectorCollection(normalFactory).GetSelector("Constant");
            operandSelector.AddValue("Ra", normalFactory.GetOperand(Settings.Default.Ra));
            operandSelector.AddValue("Rb", normalFactory.GetOperand(Settings.Default.Rb));
            operandSelector.AddValue("Rc", normalFactory.GetOperand(Settings.Default.Rc));
            operandSelector.AddValue("Rd", normalFactory.GetOperand(Settings.Default.Rd));
            operandSelector.AddValue("Re", normalFactory.GetOperand(Settings.Default.Re));
            operandSelector.AddValue("Rf", normalFactory.GetOperand(Settings.Default.Rf));
            operandSelector.AddValue("Rg", normalFactory.GetOperand(Settings.Default.Rg));
            operandSelector.AddValue("Rh", normalFactory.GetOperand(Settings.Default.Rh));
            operandSelector.AddValue("Ri", normalFactory.GetOperand(Settings.Default.Ri));
            operandSelector.AddValue("Rj", normalFactory.GetOperand(Settings.Default.Rj));

            rMenu.tt.SetToolTip(rMenu.btnA, Settings.Default.Ra.ToString());
            rMenu.tt.SetToolTip(rMenu.btnB, Settings.Default.Rb.ToString());
            rMenu.tt.SetToolTip(rMenu.btnC, Settings.Default.Rc.ToString());
            rMenu.tt.SetToolTip(rMenu.btnD, Settings.Default.Rd.ToString());
            rMenu.tt.SetToolTip(rMenu.btnE, Settings.Default.Re.ToString());
            rMenu.tt.SetToolTip(rMenu.btnF, Settings.Default.Rf.ToString());
            rMenu.tt.SetToolTip(rMenu.btnG, Settings.Default.Rg.ToString());
            rMenu.tt.SetToolTip(rMenu.btnH, Settings.Default.Rh.ToString());
            rMenu.tt.SetToolTip(rMenu.btnI, Settings.Default.Ri.ToString());
            rMenu.tt.SetToolTip(rMenu.btnJ, Settings.Default.Rj.ToString());
        }
        private void rMenu_click(object sender, EventArgs e) {
            string key = "R" + ((Button)sender).Name[3].ToString().ToLower();
            if (rMenu.status == 0) {
                //执行STO
                if (txtExpression.Text != "") {
                    try {
                        double value = Convert.ToDouble(txtExpression.Text);
                        Selector operandSelector = new DefaultSelectorCollection(normalFactory).GetSelector("Constant");
                        operandSelector.AddValue(key, normalFactory.GetOperand(value));
                        switch (key[1]) {
                            case 'a': Settings.Default.Ra = value; break;
                            case 'b': Settings.Default.Rb = value; break;
                            case 'c': Settings.Default.Rc = value; break;
                            case 'd': Settings.Default.Rd = value; break;
                            case 'e': Settings.Default.Re = value; break;
                            case 'f': Settings.Default.Rf = value; break;
                            case 'g': Settings.Default.Rg = value; break;
                            case 'h': Settings.Default.Rh = value; break;
                            case 'i': Settings.Default.Ri = value; break;
                            case 'j': Settings.Default.Rj = value; break;
                        }
                        Settings.Default.Save();
                        needToClear = true;
                        rMenu.tt.SetToolTip((Button)sender, txtExpression.Text.ToString());
                    } catch (Exception) {
                        MessageBox.Show("只能存储纯数字", "发生错误");
                    }
                }
            } else {
                changeText(key);
            }
            rMenu.Visible = false;
        }
        private void changeText(string s) {
            if (needToClear == true) {
                needToClear = false;
                txtExpression.Clear();
            }
            if (isInv == true) {
                this.btnInv.BackColor = System.Drawing.Color.White;
                isInv = false;
                btnSin.Text = "sin";
                btnCos.Text = "cos";
                btnTan.Text = "tan";
                btnPi.Text = "π";
            }
            string str1, str2;
            int t = txtExpression.SelectionStart;
            str1 = txtExpression.Text.Substring(0, t);
            str2 = txtExpression.Text.Substring(t, txtExpression.Text.Length - t);
            txtExpression.Text = str1 + s + str2;
            txtExpression.Focus();
            txtExpression.SelectionLength = 0;
            if (!s.Contains("=") && s.IndexOf('(') >= 0)
                txtExpression.SelectionStart = t + s.IndexOf('(') + 1;
            else
                txtExpression.SelectionStart = t + s.Length;
        }

        private static int funcId = 0;
        private void addMenuFunction(Function function, string expression) {
            if (funcId == 0)
                menuInsertFunc.DropDownItems.Clear();
            string oldMenuFuncName = null;
            foreach (ToolStripMenuItem item in menuInsertFunc.DropDownItems) {
                oldMenuFuncName = item.Text.Substring(0, expression.IndexOf('('));
                if (oldMenuFuncName == function.FuncName) {
                    menuInsertFunc.DropDownItems.Remove(item);
                    break;
                }
            }
            ToolStripMenuItem newFuncMenuItem = new ToolStripMenuItem(expression);
            newFuncMenuItem.Name = "menuFunc" + funcId.ToString();
            newFuncMenuItem.Click += new System.EventHandler(frmMenu_click);
            funcId++;
            menuInsertFunc.DropDownItems.Add(newFuncMenuItem);
        }

        private void btnEqual_Click(object sender, EventArgs e) {
            if (txtExpression.Text != "") {
                try {
                    string str = txtExpression.Text;

                    if (str.Contains("=")) {
                        Function newFunc = new Function(str);
                        newFunc.Add();
                        addMenuFunction(newFunc, txtExpression.Text);
                        txtExpression.Text = "";
                        FrmHistory.strH += str + "\n";
                    } else {
                        Calculate calculator;
                        if (bigNumberSupport == false)
                            calculator = new Calculate(normalFactory, str);
                        else
                            calculator = new Calculate(bigNumberFactory, str);
                        Operand result = calculator.DoCalculation();
                        //向历史记录窗口添加内容
                        FrmHistory.strH += str + " = " + result.ToString() + "\n";
                        txtExpression.Text = result.ToString();
                        txtExpression.SelectionStart = txtExpression.Text.Length;
                        needToClear = true;
                    }
                    //为上下键翻页提供内容
                    if (historyRecord.curr > historyRecord.str.Length - 1)
                        historyRecord.curr = 0;
                    historyRecord.str[historyRecord.curr++] = str;
                    //数组中下一个字符串永远是空值
                    if (historyRecord.curr == historyRecord.str.Length)
                        historyRecord.str[0] = "";
                    else
                        historyRecord.str[historyRecord.curr] = "";
                    historyRecord.top = historyRecord.curr;
                    historyRecord.count++;

                } catch (Exception ex) {
                    MessageBox.Show("表达式错误\n内部原因：" + ex.Message, "发生错误", MessageBoxButtons.OK,
                       MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        #region txtExpression
        private void txtExpression_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue == 13)
                btnEqual_Click(sender, e);
            if (e.KeyCode == Keys.Down) {
                //向下
                if (historyRecord.count <= historyRecord.str.Length - 1) {
                    if (historyRecord.top < historyRecord.curr) {
                        historyRecord.top++;
                    }
                } else {
                    int topValue = 0;
                    if (historyRecord.curr == historyRecord.str.Length)
                        topValue = 0;
                    else
                        topValue = historyRecord.curr;
                    if (historyRecord.top != topValue) {
                        if (historyRecord.top == historyRecord.str.Length - 1)
                            historyRecord.top = 0;
                        else
                            historyRecord.top++;
                    }
                }
                txtExpression.Text = historyRecord.str[historyRecord.top];
                txtExpression.SelectionStart = historyRecord.str[historyRecord.top].Length;
                needToClear = false;
            } else if (e.KeyCode == Keys.Up) {
                //向上
                if (historyRecord.count <= historyRecord.str.Length - 1) {
                    if (historyRecord.top > 0) {
                        historyRecord.top--;
                    }
                } else {
                    int topValue = 0;
                    if (historyRecord.curr == historyRecord.str.Length)
                        topValue = 1;
                    else if (historyRecord.curr == historyRecord.str.Length - 1)
                        topValue = 0;
                    else
                        topValue = historyRecord.curr + 1;
                    if (historyRecord.top != topValue) {
                        if (historyRecord.top == 0)
                            historyRecord.top = historyRecord.str.Length - 1;
                        else
                            historyRecord.top--;
                    }
                }
                txtExpression.Text = historyRecord.str[historyRecord.top];
                txtExpression.SelectionStart = historyRecord.str[historyRecord.top].Length + 1;
                needToClear = false;
            }

        }

        #endregion

        private void frmMenu_click(object sender, EventArgs e) {
            string name = ((ToolStripMenuItem)sender).Name;
            if (name == "menuPaste") {
                txtExpression.Paste();
            } else if (name == "menuCopy") {
                txtExpression.Copy();
            } else if (name == "menuHistory") {
                FrmHistory h = new FrmHistory();
                h.ShowDialog();
            } else if (name == "menuQuit") {
                this.Close();
            } else if (name == "menuDeg") {
                if (degOrRad == false) {
                    degOrRad = true;
                    menuRad.Checked = false;
                    menuDeg.Checked = true;
                }
            } else if (name == "menuRad") {
                if (degOrRad == true) {
                    degOrRad = false;
                    menuRad.Checked = true;
                    menuDeg.Checked = false;
                }
            } else if (name == "menuBigNumber") {
                txtExpression.Text = "";
                if (bigNumberSupport == false) {
                    menuBigNumber.Checked = true;
                    bigNumberSupport = true;
                    menuInsertFunc.Enabled = false;
                    btnInv.Enabled = false;
                    btnSqrt.Enabled = false;
                    btnSin.Enabled = false;
                    btnCos.Enabled = false;
                    btnTan.Enabled = false;
                    btnLn.Enabled = false;
                    btnLog.Enabled = false;
                    btnYroot.Enabled = false;
                    btnReciprocal.Enabled = false;
                    btnPi.Enabled = false;
                    btnSto.Enabled = false;
                    btnRcl.Enabled = false;
                } else {
                    menuBigNumber.Checked = false;
                    menuInsertFunc.Enabled = true;
                    bigNumberSupport = false;
                    btnInv.Enabled = true;
                    btnSqrt.Enabled = true;
                    btnSin.Enabled = true;
                    btnCos.Enabled = true;
                    btnTan.Enabled = true;
                    btnLn.Enabled = true;
                    btnLog.Enabled = true;
                    btnYroot.Enabled = true;
                    btnReciprocal.Enabled = true;
                    btnPi.Enabled = true;
                    btnSto.Enabled = true;
                    btnRcl.Enabled = true;
                }
            } else if (name == "menuAbout") {
                FrmAbout about = new FrmAbout();
                about.ShowDialog();
            } else if (Regex.IsMatch(name, "menuFunc[0-9]+")) {
                string expression = ((ToolStripMenuItem)sender).Text;
                string funcName = expression.Substring(0, expression.IndexOf('('));
                //零宽断言
                string parameters = Regex.Match(expression, @"(?<=\()[a-zA-Z]+(,[a-zA-Z]+)*(?=\))").Value;
                int parameterCount = parameters.Split(',').Length;
                string text = funcName + "(";
                for (int i = 0; i < parameterCount - 1; i++)
                    text += ",";
                text += ")";
                changeText(text);
            }
        }

        private void frmBtn_click(object sender, EventArgs e) {
            string name = ((Button)sender).Name;
            if (name != "btnRcl" && name != "btnSto")
                rMenu.Visible = false;

            //btn0
            if (name == "btn0") {
                changeText("0");
            }
                //btn1
            else if (name == "btn1") {
                changeText("1");
            }
                //btn2
            else if (name == "btn2") {
                changeText("2");
            }
                //btn3
            else if (name == "btn3") {
                changeText("3");
            }
                //btn4
            else if (name == "btn4") {
                changeText("4");
            }
                //btn5
            else if (name == "btn5") {
                changeText("5");
            }
                //btn6
            else if (name == "btn6") {
                changeText("6");
            }
                //btn7
            else if (name == "btn7") {
                changeText("7");
            }
                //btn8
            else if (name == "btn8") {
                changeText("8");
            }
                //btn9
            else if (name == "btn9") {
                changeText("9");
            }
                //btnX
            else if (name == "btnX") {
                changeText("x");
            }
                //btnPoint
            else if (name == "btnPoint") {
                changeText(".");
            }
                //btnAdd
            else if (name == "btnAdd") {
                changeText("+");
            }
                //btnMinus
            else if (name == "btnMinus") {
                changeText("-");
            }
                //btnMultipy
            else if (name == "btnMultipy") {
                changeText("*");
            }
                //btnDivide
            else if (name == "btnDivide") {
                changeText("/");
            }
                //btnRight
            else if (name == "btnRight") {
                changeText(")");
            }
                //btnLeft
            else if (name == "btnLeft") {
                changeText("(");
            }
                //btnCE
            else if (name == "btnCE") {
                txtExpression.Focus();
                txtExpression.SelectionLength = 0;
                SendKeys.Send("\u0008");
            }
                //btnC
            else if (name == "btnC") {
                txtExpression.Text = "";
            }
                //btnMod
            else if (name == "btnMod") {
                changeText("mod(,)");
            }
                //btnYroot
            else if (name == "btnYroot") {
                changeText("yroot(,)");
            }
                //btnSqrt
            else if (name == "btnSqrt") {
                changeText("sqrt()");
            }
                //btnReciprocal
            else if (name == "btnReciprocal") {
                changeText("^-1");
            }
                //btnLn
            else if (name == "btnLn") {
                changeText("ln()");
            }
                //btnSin
            else if (name == "btnSin") {
                if (isInv == false) {
                    if (degOrRad == false)
                        changeText("sinr()");
                    else
                        changeText("sind()");
                } else {
                    if (degOrRad == false)
                        changeText("arcsinr()");
                    else
                        changeText("arcsind()");
                }
            }
                //btnCos
            else if (name == "btnCos") {
                if (isInv == false) {
                    if (degOrRad == false)
                        changeText("cosr()");
                    else
                        changeText("cosd()");
                } else {
                    if (degOrRad == false)
                        changeText("arccosr()");
                    else
                        changeText("arccosd()");
                }
            }
                //btnTan
            else if (name == "btnTan") {
                if (isInv == false) {
                    if (degOrRad == false)
                        changeText("tanr()");
                    else
                        changeText("tand()");
                } else {
                    if (degOrRad == false)
                        changeText("arctanr()");
                    else
                        changeText("arctand()");
                }
            }
                //btnLog
            else if (name == "btnLog") {
                changeText("log(,)");
            } else if (name == "btnPower") {
                changeText("^");
            } else if (name == "btnPi") {
                if (isInv == false) {
                    changeText("pi");
                } else {
                    changeText("e");
                }
            }
                //btnX10
              else if (name == "btnX10") {
                changeText("*10^");
            }
                //btnFactorial
              else if (name == "btnFactorial") {
                changeText("!");
            }
                //btnAbs
                else if (name == "btnAbs") {
                changeText("abs()");
            }
                //btnComma
                else if (name == "btnComma") {
                changeText(",");
            }
                //btnInv
              else if (name == "btnInv") {
                if (isInv == false) {
                    isInv = true;
                    this.btnInv.BackColor = System.Drawing.Color.AliceBlue;
                    btnSin.Text = "asin";
                    btnCos.Text = "acos";
                    btnTan.Text = "atan";
                    btnPi.Text = "e";
                } else {
                    this.btnInv.BackColor = System.Drawing.Color.White;
                    isInv = false;
                    btnSin.Text = "sin";
                    btnCos.Text = "cos";
                    btnTan.Text = "tan";
                    btnPi.Text = "π";
                }
            }
                //btnSto
              else if (name == "btnSto") {
                rMenu.Left = groupBox4.Left + btnSto.Left + btnSto.Size.Width;
                rMenu.Top = groupBox4.Top + btnSto.Top + btnSto.Size.Height / 2;
                if (rMenu.Visible == false)
                    rMenu.Visible = true;
                else if (rMenu.status == 0 && rMenu.Visible == true)
                    rMenu.Visible = false;
                rMenu.status = 0;
            }
                //btnRcl
              else if (name == "btnRcl") {
                rMenu.Left = groupBox4.Left + btnRcl.Left + btnRcl.Size.Width;
                rMenu.Top = groupBox4.Top + btnRcl.Top + btnRcl.Size.Height / 2;
                if (rMenu.Visible == false)
                    rMenu.Visible = true;
                else if (rMenu.status == 1 && rMenu.Visible == true)
                    rMenu.Visible = false;
                rMenu.status = 1;
            }
        }
    }
}
