using System;

namespace Net.AlexKing.Calculator.Core
{
    public sealed class OperandDouble : Operand
    {
        private double value;

        public override Object GetValue() {
            return value;
        }

        public override void SetValue(Operand other) {
            if (!(other is OperandDouble))
                throw new InvalidOperationException("Cannot set value with another type of operand");

            this.value = ((OperandDouble)other).value;
        }

        public OperandDouble(double value) {
            this.value = value;
        }

        public OperandDouble(string symbolOrLex) {
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
}
