using System;
using System.Collections.Generic;
using System.Numerics;

namespace Net.AlexKing.Calculator.Core
{
    public sealed class BigIntegerCalculate : Calculate
    {
        private static Selector operatorSelector = new Selector();
        private static Selector constantSelector = new Selector();

        static BigIntegerCalculate() {
            initialBasciOperands();
            initialBasicOperators();
        }
        protected override void initialSelector() {
            addSelector(operatorSelector);
            addSelector(constantSelector);
        }

        public static Selector GetOperatorSelector() {
            return operatorSelector;
        }

        public static Selector GetConstantSelector() {
            return constantSelector;
        }

        public BigIntegerCalculate()
            : base() {
        }

        public BigIntegerCalculate(String expStr)
            : base(expStr) {
        }

        private static void initialBasciOperands() {
            return;
        }

        private static void initialBasicOperators() {
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "+", delegate(List<Operand> operands) {
                return new OperandBigInteger((BigInteger)operands[0].GetValue() + (BigInteger)operands[1].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "-", delegate(List<Operand> operands) {
                return new OperandBigInteger((BigInteger)operands[1].GetValue() - (BigInteger)operands[0].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "*", delegate(List<Operand> operands) {
                return new OperandBigInteger((BigInteger)operands[0].GetValue() * (BigInteger)operands[1].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "/", delegate(List<Operand> operands) {
                return new OperandBigInteger((BigInteger)operands[1].GetValue() / (BigInteger)operands[0].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "mod", delegate(List<Operand> operands) {
                BigInteger rem;
                BigInteger.DivRem((BigInteger)operands[1].GetValue(), (BigInteger)operands[0].GetValue(), out rem);
                return new OperandBigInteger(rem);
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "pos", delegate(List<Operand> operands) {
                return operands[0];
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "neg", delegate(List<Operand> operands) {
                return new OperandBigInteger(-(BigInteger)operands[0].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.powerLevel, OperatorType.sign, 2, "^", delegate(List<Operand> operands) {
                return new OperandBigInteger(BigInteger.Pow((BigInteger)operands[1].GetValue(), Convert.ToInt32(((BigInteger)operands[0].GetValue()).ToString())));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "abs", delegate(List<Operand> operands) {
                return new OperandBigInteger(BigInteger.Abs((BigInteger)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.factorialLevel, OperatorType.sign, 1, "!", delegate(List<Operand> operands) {
                BigInteger n = (BigInteger)operands[0].GetValue();
                BigInteger result = new BigInteger(1);
                while (n > 0) {
                    result = result * n;
                    n--;
                }
                return new OperandBigInteger(result);
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, "(", delegate(List<Operand> operands) {
                return null;
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, ")", delegate(List<Operand> operands) {
                return null;
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, ",", delegate(List<Operand> operands) {
                return null;
            }));
        }
    }
}
