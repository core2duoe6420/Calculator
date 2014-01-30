using System;
using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public class NormalCalculate : Calculate
    {
        private static Selector operatorSelector = new Selector();
        private static Selector constantSelector = new Selector();

        static NormalCalculate() {
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
        public NormalCalculate()
            : base() {
        }

        public NormalCalculate(String expStr)
            : base(expStr) {
        }

        private static void initialBasciOperands() {
            GetConstantSelector().AddValue("e", OperandFactory.GetNewOperand(Math.E));
            GetConstantSelector().AddValue("pi", OperandFactory.GetNewOperand(Math.PI));
        }

        private static void initialBasicOperators() {
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "+", delegate(List<Operand> operands) {
                return new OperandDouble((Double)operands[0].GetValue() + (Double)operands[1].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "-", delegate(List<Operand> operands) {
                return new OperandDouble((Double)operands[1].GetValue() - (Double)operands[0].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "*", delegate(List<Operand> operands) {
                return new OperandDouble((Double)operands[0].GetValue() * (Double)operands[1].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "/", delegate(List<Operand> operands) {
                return new OperandDouble((Double)operands[1].GetValue() / (Double)operands[0].GetValue());
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
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sin", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Sin((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sinr", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Sin((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sind", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Sin((Double)operands[0].GetValue() / 180 * Math.PI));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "cos", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Cos((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "cosr", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Cos((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "cosd", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Cos((Double)operands[0].GetValue() / 180 * Math.PI));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "tan", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Tan((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "tanr", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Tan((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "tand", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Tan((Double)operands[0].GetValue() / 180 * Math.PI));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arcsin", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Asin((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arcsinr", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Asin((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arcsind", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Asin((Double)operands[0].GetValue()) / Math.PI * 180);
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arccos", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Acos((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arccosr", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Acos((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arccosd", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Acos((Double)operands[0].GetValue()) / Math.PI * 180);
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arctan", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Atan((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arctanr", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Atan((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arctand", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Atan((Double)operands[0].GetValue()) / Math.PI * 180);
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "abs", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Abs((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "ln", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Log((Double)operands[0].GetValue(), Math.E));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sqrt", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Sqrt((Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "pos", delegate(List<Operand> operands) {
                return operands[0];
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "neg", delegate(List<Operand> operands) {
                return new OperandDouble(-(Double)operands[0].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "mod", delegate(List<Operand> operands) {
                return new OperandDouble((Double)operands[1].GetValue() % (Double)operands[0].GetValue());
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "yroot", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Pow((Double)operands[1].GetValue(), 1 / (Double)operands[0].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "log", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Log((Double)operands[0].GetValue(), (Double)operands[1].GetValue()));
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.factorialLevel, OperatorType.sign, 1, "!", delegate(List<Operand> operands) {
                double result = 1, n = (Double)operands[0].GetValue();
                while (n > 0) {
                    result = result * n;
                    n--;
                }
                return new OperandDouble(result);
            }));
            GetOperatorSelector().AddValue(new Operator(OperatorPriority.powerLevel, OperatorType.sign, 2, "^", delegate(List<Operand> operands) {
                return new OperandDouble(Math.Pow((Double)operands[1].GetValue(), (Double)operands[0].GetValue()));
            }));
        }

    }
}
