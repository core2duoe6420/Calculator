using System;
using System.Numerics;

namespace Net.AlexKing.Calculator.Core
{
    public sealed class OperandBigInteger : Operand
    {
        private BigInteger value;

        public override Object GetValue() {
            return value;
        }

        public override void SetValue(Operand other) {
            if (!(other is OperandBigInteger))
                throw new InvalidOperationException("Cannot set value with another type of operand");

            this.value = ((OperandBigInteger)other).value;
        }

        public OperandBigInteger(BigInteger value) {
            this.value = value;
        }

        public OperandBigInteger(string symbolOrLex) {
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
