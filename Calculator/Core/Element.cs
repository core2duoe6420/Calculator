using System;

namespace Net.AlexKing.Calculator.Core
{
    public class Element
    {
        public bool isOperand;
        public bool isOperator;
        private Operator theOperator;

        public Operator TheOperator {
            get { return theOperator; }
            set { theOperator = value; }
        }
        private Operand theOperand;
        public Operand TheOperand {
            get { return theOperand; }
        }

        private void constructionOperator(Operator theOperator) {
            isOperand = false;
            isOperator = true;
            this.theOperand = null;
            this.theOperator = theOperator;
        }

        private void constructionOperand(Operand operand) {
            isOperand = true;
            isOperator = false;
            this.theOperand = operand;
            this.theOperator = null;
        }

        public Element(Object obj) {
            if (obj is Operator)
                constructionOperator((Operator)obj);
            else
                constructionOperand((Operand)obj);
        }

        public Element(Operand operand) {
            constructionOperand(operand);
        }

        public Element(Operator theOperator) {
            constructionOperator(theOperator);
        }

        public override string ToString() {
            if (isOperand == true)
                return theOperand.GetValue().ToString();
            else if (isOperator == true)
                return TheOperator.Name;
            return null;
        }
    }
}
