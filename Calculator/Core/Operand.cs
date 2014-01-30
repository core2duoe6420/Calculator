using System;

namespace Net.AlexKing.Calculator.Core
{
    public abstract class Operand
    {
        //symbol主要用于标记函数中的参数，见Function类
        private string symbol;

        public string Symbol {
            get { return symbol; }
            set { symbol = value; }
        }


        public abstract Object GetValue();
        public abstract void SetValue(Operand other);
    }
}
