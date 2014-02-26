using System;
using System.Numerics;

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

    public sealed class DefaultOperand : Operand
    {
        private double value;

        public override Object GetValue() {
            return value;
        }

        public override void SetValue(Operand other) {
            if (!(other is DefaultOperand))
                throw new InvalidOperationException("Cannot set value with another type of operand");

            this.value = ((DefaultOperand)other).value;
        }

        public DefaultOperand(double value) {
            this.value = value;
        }

        public DefaultOperand(string symbolOrLex) {
            char first = symbolOrLex[0];
            if (Char.IsDigit(first))
                this.value = Convert.ToDouble(symbolOrLex);
            else {
                this.Symbol = symbolOrLex;
                this.value = 1;
            }
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    public sealed class BigIntegerOperand : Operand
    {
        private BigInteger value;

        public override Object GetValue() {
            return value;
        }

        public override void SetValue(Operand other) {
            if (!(other is BigIntegerOperand))
                throw new InvalidOperationException("Cannot set value with another type of operand");

            this.value = ((BigIntegerOperand)other).value;
        }

        public BigIntegerOperand(BigInteger value) {
            this.value = value;
        }

        public BigIntegerOperand(string symbolOrLex) {
            char first = symbolOrLex[0];
            if (Char.IsDigit(first))
                this.value = BigInteger.Parse(symbolOrLex);
            else {
                this.Symbol = symbolOrLex;
                this.value = 1;
            }
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
