using System;
using System.Collections.Generic;
using System.Numerics;

namespace Net.AlexKing.Calculator.Core
{
    public abstract class SelectorCollection
    {
        public abstract Selector GetSelector(int index);

        public abstract Selector GetSelector(string key);

        public abstract Object GetValueInSelectors(string key);

        public abstract void AddSelector(Selector selector);
    }

    public class DefaultSelectorCollection : SelectorCollection
    {
        private static Selector operatorSelector = null;
        private static Selector constantSelector = null;
        private List<Selector> selectors;
        private CalculateFactory factory;

        public DefaultSelectorCollection(CalculateFactory factory) {
            this.factory = factory;
            selectors = new List<Selector>();
            if (operatorSelector == null) {
                operatorSelector = new Selector();
                initialBasicOperators();
            }
            if (constantSelector == null) {
                constantSelector = new Selector();
                initialBasciOperands();
            }
            selectors.Add(operatorSelector);
            selectors.Add(constantSelector);
        }

        public override Selector GetSelector(int index) {
            if (index < 0 || index >= selectors.Count)
                return null;
            return selectors[index];
        }

        public override void AddSelector(Selector selector) {
            selectors.Add(selector);
        }

        public override Selector GetSelector(string key) {
            if (key == "Operator")
                return operatorSelector;
            if (key == "Constant")
                return constantSelector;
            return null;
        }

        public override Object GetValueInSelectors(string key) {
            foreach (Selector selector in selectors) {
                Object obj = selector.GetValue(key);
                if (obj != null)
                    return obj;
            }
            return null;
        }

        private void initialBasciOperands() {
            constantSelector.AddValue("e", factory.GetOperand(Math.E));
            constantSelector.AddValue("pi", factory.GetOperand(Math.PI));
        }

        private void initialBasicOperators() {
            operatorSelector.AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "+", delegate(List<Operand> operands) {
                return new DefaultOperand((Double)operands[0].GetValue() + (Double)operands[1].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "-", delegate(List<Operand> operands) {
                return new DefaultOperand((Double)operands[1].GetValue() - (Double)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "*", delegate(List<Operand> operands) {
                return new DefaultOperand((Double)operands[0].GetValue() * (Double)operands[1].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "/", delegate(List<Operand> operands) {
                return new DefaultOperand((Double)operands[1].GetValue() / (Double)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, "(", delegate(List<Operand> operands) {
                return null;
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, ")", delegate(List<Operand> operands) {
                return null;
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, ",", delegate(List<Operand> operands) {
                return null;
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sin", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Sin((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sinr", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Sin((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sind", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Sin((Double)operands[0].GetValue() / 180 * Math.PI));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "cos", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Cos((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "cosr", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Cos((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "cosd", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Cos((Double)operands[0].GetValue() / 180 * Math.PI));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "tan", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Tan((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "tanr", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Tan((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "tand", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Tan((Double)operands[0].GetValue() / 180 * Math.PI));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arcsin", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Asin((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arcsinr", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Asin((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arcsind", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Asin((Double)operands[0].GetValue()) / Math.PI * 180);
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arccos", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Acos((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arccosr", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Acos((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arccosd", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Acos((Double)operands[0].GetValue()) / Math.PI * 180);
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arctan", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Atan((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arctanr", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Atan((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "arctand", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Atan((Double)operands[0].GetValue()) / Math.PI * 180);
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "abs", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Abs((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "ln", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Log((Double)operands[0].GetValue(), Math.E));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "sqrt", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Sqrt((Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "pos", delegate(List<Operand> operands) {
                return operands[0];
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "neg", delegate(List<Operand> operands) {
                return new DefaultOperand(-(Double)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "mod", delegate(List<Operand> operands) {
                return new DefaultOperand((Double)operands[1].GetValue() % (Double)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "yroot", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Pow((Double)operands[1].GetValue(), 1 / (Double)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "log", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Log((Double)operands[0].GetValue(), (Double)operands[1].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.factorialLevel, OperatorType.sign, 1, "!", delegate(List<Operand> operands) {
                double result = 1, n = (Double)operands[0].GetValue();
                while (n > 0) {
                    result = result * n;
                    n--;
                }
                return new DefaultOperand(result);
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.powerLevel, OperatorType.sign, 2, "^", delegate(List<Operand> operands) {
                return new DefaultOperand(Math.Pow((Double)operands[1].GetValue(), (Double)operands[0].GetValue()));
            }));
        }
    }

    public class BigNumberSelectorCollection : SelectorCollection
    {
        private static Selector operatorSelector = null;
        private static Selector constantSelector = null;
        private List<Selector> selectors;

        public BigNumberSelectorCollection() {
            selectors = new List<Selector>();
            if (operatorSelector == null) {
                operatorSelector = new Selector();
                initialBasicOperators();
            }
            if (constantSelector == null) {
                constantSelector = new Selector();
                initialBasciOperands();
            }
            selectors.Add(operatorSelector);
            selectors.Add(constantSelector);
        }

        public override Selector GetSelector(int index) {
            if (index < 0 || index >= selectors.Count)
                return null;
            return selectors[index];
        }

        public override void AddSelector(Selector selector) {
            selectors.Add(selector);
        }

        public override Selector GetSelector(string key) {
            if (key == "Operator")
                return operatorSelector;
            if (key == "Constant")
                return constantSelector;
            return null;
        }

        public override Object GetValueInSelectors(string key) {
            foreach (Selector selector in selectors) {
                Object obj = selector.GetValue(key);
                if (obj != null)
                    return obj;
            }
            return null;
        }

        private void initialBasciOperands() {

        }

        private void initialBasicOperators() {
            operatorSelector.AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "+", delegate(List<Operand> operands) {
                return new BigIntegerOperand((BigInteger)operands[0].GetValue() + (BigInteger)operands[1].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.addLevel, OperatorType.addminus, 2, "-", delegate(List<Operand> operands) {
                return new BigIntegerOperand((BigInteger)operands[1].GetValue() - (BigInteger)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "*", delegate(List<Operand> operands) {
                return new BigIntegerOperand((BigInteger)operands[0].GetValue() * (BigInteger)operands[1].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.multiplyLevel, OperatorType.sign, 2, "/", delegate(List<Operand> operands) {
                return new BigIntegerOperand((BigInteger)operands[1].GetValue() / (BigInteger)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 2, "mod", delegate(List<Operand> operands) {
                BigInteger rem;
                BigInteger.DivRem((BigInteger)operands[1].GetValue(), (BigInteger)operands[0].GetValue(), out rem);
                return new BigIntegerOperand(rem);
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "pos", delegate(List<Operand> operands) {
                return operands[0];
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.addminus, 1, "neg", delegate(List<Operand> operands) {
                return new BigIntegerOperand(-(BigInteger)operands[0].GetValue());
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.powerLevel, OperatorType.sign, 2, "^", delegate(List<Operand> operands) {
                return new BigIntegerOperand(BigInteger.Pow((BigInteger)operands[1].GetValue(), Convert.ToInt32(((BigInteger)operands[0].GetValue()).ToString())));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.functionLevel, OperatorType.function, 1, "abs", delegate(List<Operand> operands) {
                return new BigIntegerOperand(BigInteger.Abs((BigInteger)operands[0].GetValue()));
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.factorialLevel, OperatorType.sign, 1, "!", delegate(List<Operand> operands) {
                BigInteger n = (BigInteger)operands[0].GetValue();
                BigInteger result = new BigInteger(1);
                while (n > 0) {
                    result = result * n;
                    n--;
                }
                return new BigIntegerOperand(result);
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, "(", delegate(List<Operand> operands) {
                return null;
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, ")", delegate(List<Operand> operands) {
                return null;
            }));
            operatorSelector.AddValue(new Operator(OperatorPriority.bracketsLevel, OperatorType.other, 0, ",", delegate(List<Operand> operands) {
                return null;
            }));
        }
    }
}
