using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public delegate Operand DoOperationDelegate(List<Operand> operands);
    public class Operator
    {
        private OperatorPriority priority;
        private OperatorType type;
        private int operandCount;

        public int OperandCount {
            get { return operandCount; }
        }
        private string name;

        public string Name {
            get { return name; }
        }

        public DoOperationDelegate DoOperation;

        public Operator(OperatorPriority priority, OperatorType type, int operandCount,
            string name, DoOperationDelegate doOperation) {
            this.priority = priority;
            this.type = type;
            this.operandCount = operandCount;
            this.name = name;
            this.DoOperation = doOperation;
        }

        public bool IsTwoOperandSign() {
            return OperandCount == 2;
        }

        public bool IsPosiviveNegativeSign() {
            return type == OperatorType.addminus;
        }

        public bool IsFunction() {
            return type == OperatorType.function;
        }

        public override string ToString() {
            return Name;
        }

        public bool PriorityEqual(int priorityCode) {
            return this.priority.GetHashCode() == priorityCode;
        }

        public bool Equals(string operatorName) {
            return Name == operatorName;
        }
    }

    public enum OperatorPriority
    {
        functionLevel, factorialLevel, powerLevel, multiplyLevel, addLevel, bracketsLevel
    }

    public enum OperatorType
    {
        addminus, sign, function, other
    }
}
