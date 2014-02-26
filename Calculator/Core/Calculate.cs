using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public class Calculate
    {
        private string expression;
        protected SelectorCollection selectors;
        protected List<Element> postfix;
        protected CalculateFactory factory;

        public Calculate(CalculateFactory factory) {
            this.factory = factory;
            this.selectors = factory.GetSelectorCollection();
        }

        public Calculate(CalculateFactory factory, string expStr)
            : this(factory) {
            expression = expStr.Replace(" ", "");
            checkExpression();
        }

        public Calculate(string expStr)
            : this(new DefaultCalculateFactory(), expStr) {
        }

        public Calculate()
            : this(new DefaultCalculateFactory()) {
        }

        public string Expression {
            get { return expression; }
            set {
                //通过属性设置expression会立刻检查语法
                this.expression = value.Replace(" ", "");
                checkExpression();
            }
        }

        protected void checkExpression() {
            IParser<Element> parser = factory.GetParser(selectors);
            postfix = parser.Parse(expression);
        }

        public Operand DoCalculation() {
            IPostFixComputor<Element> computor = factory.GetPostFixComputor();
            return computor.ComputePostFix(postfix);
        }
    }
}
