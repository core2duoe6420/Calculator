using System;

namespace Net.AlexKing.Calculator.Core
{
    public abstract class CalculateFactory
    {
        public abstract SelectorCollection GetSelectorCollection();

        public abstract Operand GetOperand(Object obj);

        public abstract IParser<Element> GetParser(SelectorCollection selectors);

        public abstract IPostFixComputor<Element> GetPostFixComputor();
    }

    internal class DefaultCalculateFactory : CalculateFactory
    {
        public override SelectorCollection GetSelectorCollection() {
            return new DefaultSelectorCollection(this);
        }

        public override Operand GetOperand(Object obj) {
            if (obj is Double)
                return new OperandDouble((double)obj);
            if (obj is string)
                return new OperandDouble((string)obj);
            return null;
        }

        public override IParser<Element> GetParser(SelectorCollection selectors) {
            return new DefaultParser(selectors, this);
        }

        public override IPostFixComputor<Element> GetPostFixComputor() {
            return new DefaultPostFixComputor();
        }
    }

    internal class BigNumberCalculateFactory : CalculateFactory
    {
        public override SelectorCollection GetSelectorCollection() {
            return new BigNumberSelectorCollection();
        }

        public override Operand GetOperand(Object obj) {
            if (obj is string)
                return new OperandBigInteger((string)obj);
            return null;
        }

        public override IParser<Element> GetParser(SelectorCollection selectors) {
            return new DefaultParser(selectors, this);
        }

        public override IPostFixComputor<Element> GetPostFixComputor() {
            return new DefaultPostFixComputor();
        }
    }
}
