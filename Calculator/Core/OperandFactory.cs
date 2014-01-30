namespace Net.AlexKing.Calculator.Core
{
    class OperandFactory
    {
        private OperandFactory() { }

        public static Operand GetNewOperand(double value) {
            return new OperandDouble(value);
        }

        public static Operand GetNewOperand(string str) {
            if (Net.AlexKing.Calculator.Forms.FrmCalc.BigNumberSupport)
                return new OperandBigInteger(str);
            else
                return new OperandDouble(str);
        }
    }
}
