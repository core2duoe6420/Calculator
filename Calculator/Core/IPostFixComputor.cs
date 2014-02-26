using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public interface IPostFixComputor<T>
    {
        Operand ComputePostFix(IList<T> postfix);
    }

    internal class DefaultPostFixComputor : IPostFixComputor<Element>
    {
        public Operand ComputePostFix(IList<Element> postfix) {
            Stack<Element> stack = new Stack<Element>();
            List<Operand> operands = null;
            for (int i = 0; i < postfix.Count; i++) {
                Element curEle = postfix[i];
                if (curEle.isOperand)
                    stack.Push(curEle);
                if (curEle.isOperator) {
                    Operator theOperator = curEle.TheOperator;
                    operands = new List<Operand>();
                    for (int j = 0; j < theOperator.OperandCount; j++)
                        operands.Add(stack.Pop().TheOperand);
                    stack.Push(new Element(theOperator.DoOperation(operands)));
                }
            }
            return stack.Pop().TheOperand;
        }
    }
}
