using System;
using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public abstract class Calculate
    {
        private List<Selector> selectors;
        private List<Element> postfix;
        private string expression;
        private Parser infixParser;

        protected List<Element> Postfix {
            get { return postfix; }
        }

        protected void addSelector(Selector selector) {
            selectors.Add(selector);
        }

        //根据运算对象不同区分常量和运算符方法，子类负责初始化常量和运算符选择器
        //并通过addSelector将其加入列表
        protected abstract void initialSelector();

        private void instanceMembers() {
            postfix = new List<Element>();
            selectors = new List<Selector>();
        }

        public Calculate() {
            instanceMembers();
            initialSelector();
        }

        public Calculate(string expStr) {
            instanceMembers();
            initialSelector();
            expression = expStr.Replace(" ", "");
            checkExpression();
        }

        public string Expression {
            get { return expression; }
            set {
                //通过属性设置expression会立刻检查语法
                this.expression = value.Replace(" ", "");
                checkExpression();
            }
        }

        //词法分析和语法分析
        //语法分析的同时会生成后缀表达式
        protected void checkExpression() {
            infixParser = new Parser(this);
            parseInfix();
        }


        public Operand DoCalculation() {
            return calPostfix();
        }

        private Operand calPostfix() {
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

        //语法分析器 提供一些比较函数以及对infix的遍历
        private class Parser
        {
            private Calculate cal;
            private int nextLexerPos = 0;
            private int curLexerPos = 0;
            private Element curElement = null;
            public Parser(Calculate cal) {
                this.cal = cal;
            }

            //判断词法单元，数字则认为是操作数
            //字母则在选择器列表中依次查找标识符，找不到说明出错
            private Element getNewElement(string lex) {
                if (lex.Length == 0)
                    return null;
                if (lex[0].IsNumber())
                    return new Element(OperandFactory.GetNewOperand(lex));

                Element ret = null;
                bool isValid = false;
                foreach (Selector selector in cal.selectors) {
                    if (selector.HasValue(lex)) {
                        Object obj = selector.GetValue(lex);
                        if (obj != null) {
                            ret = new Element(obj);
                            isValid = true;
                            break;
                        }
                    }
                }
                if (isValid == false)
                    ThrowException("Unknown key words " + lex);
                return ret;
            }

            private void setCharArraySpaces(char[] charArray, int pos) {
                for (int i = 0; i < pos; i++)
                    charArray[i] = ' ';
            }

            //词法分析
            //仅3种元素 数字 一个字符的运算符 全部由字母组成的标识符
            private Element lexerGetElement() {
                char[] lexical = new char[cal.expression.Length];
                setCharArraySpaces(lexical, lexical.Length);
                int pos = 0;
                for (int i = curLexerPos; i < cal.expression.Length; i++) {
                    char curChar = cal.expression[i];
                    // we assume that a sign takes one char
                    if (pos != 0 && (curChar.IsSymbol() || !curChar.IsSameClass(lexical[pos - 1]))) {
                        nextLexerPos = i;
                        return getNewElement(new string(lexical).Trim());
                    }
                    lexical[pos++] = curChar;
                }
                if (pos != 0) {
                    nextLexerPos = cal.expression.Length;
                    return getNewElement(new string(lexical).Trim());
                }
                return null;
            }


            public Element GetNextElement(bool increment) {
                if (!HasNextElement())
                    ThrowException("Statement wrong at " + curElement.ToString());
                //当curLexerPos与nextLexerPos相等时说明执行过MoveToNextElement操作
                //于是获得下一个元素
                if (curElement == null || curLexerPos == nextLexerPos)
                    curElement = lexerGetElement();
                if (increment == true)
                    MoveToNextElement();
                return curElement;
            }

            public void MoveToNextElement() {
                curLexerPos = nextLexerPos;
            }

            public bool HasNextElement() {
                return curLexerPos < cal.expression.Length;
            }

            public bool TestElement(Element element, string name) {
                return element.isOperator && element.TheOperator.Equals(name);
            }

            public bool CheckElement(Element element, string name) {
                if (!TestElement(element, name))
                    ThrowException("Statement wrong at " + element.ToString());
                return true;
            }

            public bool CheckFunctionParameter(Operator theOperator, int parameterCount) {
                if (parameterCount != theOperator.OperandCount)
                    ThrowException("Function " + theOperator.Name + " need " + theOperator.OperandCount + " parameters");
                return true;
            }

            public void ThrowException(string err) {
                throw new FormatException(err);
            }
        }
        
        //语法分析入口
        private void parseInfix() {
            parseExpression();
            if (infixParser.HasNextElement())
                infixParser.ThrowException("Statement wrong at " + infixParser.GetNextElement(false).ToString());
        }

        private Operator getRequestOperator(string operatorName) {
            foreach (Selector selector in selectors) {
                Object obj = selector.GetValue(operatorName);
                if (obj != null && obj is Operator)
                    return (Operator)obj;
            }
            return null;
        }

        /*
         * 语法分析分为3个函数，其中项的分析按照加减优先级、
         * 乘除优先级、幂运算优先级、阶乘优先级依次递归，当遇到+/-并且
         * 语法词汇应为正负号时，会设置Operator为正负号
         * 表达式 => +/- 表达式
         * 表达式 => 加减优先级项
         * 加减优先级项 => 乘除优先级项 +/- 乘除优先级项
         * 乘除优先级项 => 幂优先级项 * / 幂优先级项
         * 幂优先级项 => 阶乘优先级项 ^ 阶乘优先级项
         * 阶乘优先级项 => 因子 ! 因子
         * 因子 => +/- 因子
         * 因子 => 函数(表达式[,表达式]+)
         * 因子 => (表达式)
         * 因子 => 数
         */
        private void parseExpression() {
            Element curEle = infixParser.GetNextElement(false);
            Operator theOperator = null;
            if (curEle.isOperator)
                theOperator = curEle.TheOperator;
            if (theOperator != null && infixParser.TestElement(curEle, "+")) {
                curEle.TheOperator = getRequestOperator("pos");
                infixParser.MoveToNextElement();
                parseExpression();
                postfix.Add(curEle);
            } else if (theOperator != null && infixParser.TestElement(curEle, "-")) {
                curEle.TheOperator = getRequestOperator("neg");
                infixParser.MoveToNextElement();
                parseExpression();
                postfix.Add(curEle);
            } else {
                parseItem(OperatorPriority.addLevel.GetHashCode());
            }
        }

        private void parseItem(int priority) {
            if (priority > OperatorPriority.factorialLevel.GetHashCode())
                parseItem(priority - 1);
            else
                parseFactor();
            Element curEle = null;
            if (infixParser.HasNextElement())
                curEle = infixParser.GetNextElement(false);
            while (curEle != null && curEle.isOperator && curEle.TheOperator.PriorityEqual(priority)) {
                infixParser.MoveToNextElement();
                Operator theOperator = curEle.TheOperator;
                if (theOperator.IsTwoOperandSign()) {
                    if (priority > OperatorPriority.functionLevel.GetHashCode())
                        parseItem(priority - 1);
                    else
                        parseFactor();
                }
                postfix.Add(curEle);
                if (infixParser.HasNextElement())
                    curEle = infixParser.GetNextElement(false);
                else
                    curEle = null;
            }
        }

        private void parseFactor() {
            Element curEle = infixParser.GetNextElement(true);
            Element frameEle = curEle;
            if (curEle.isOperand) {
                postfix.Add(frameEle);
                return;
            }
            Operator curOperator = curEle.TheOperator;
            if (infixParser.TestElement(curEle, "+")) {
                curEle.TheOperator = getRequestOperator("pos");
                parseFactor();
                postfix.Add(frameEle);
                return;
            } else if (infixParser.TestElement(curEle, "-")) {
                curEle.TheOperator = getRequestOperator("neg");
                parseFactor();
                postfix.Add(frameEle);
                return;
            } else if (infixParser.TestElement(curEle, "(")) {
                parseExpression();
                curEle = infixParser.GetNextElement(true);
                infixParser.CheckElement(curEle, ")");
                return;
            } else if (curOperator.IsFunction()) {
                int paraCount = 0;
                curEle = infixParser.GetNextElement(true);
                infixParser.CheckElement(curEle, "(");
                parseExpression();
                paraCount++;
                curEle = infixParser.GetNextElement(false);
                while (infixParser.TestElement(curEle, ",")) {
                    infixParser.MoveToNextElement();
                    parseExpression();
                    paraCount++;
                    curEle = infixParser.GetNextElement(false);
                }
                infixParser.CheckFunctionParameter(curOperator, paraCount);
                infixParser.CheckElement(curEle, ")");
                postfix.Add(frameEle);
                infixParser.MoveToNextElement();
                return;
            }
            // here meens something wrong
            infixParser.ThrowException("Statement wrong at " + curEle.ToString());
        }
    }

    static class CharacterExtensions
    {
        public static bool IsNumber(this char ch) {
            return Char.IsNumber(ch) || ch == '.';
        }

        public static bool IsAlphabet(this char ch) {
            return Char.IsLetter(ch);
        }

        public static bool IsSymbol(this char ch) {
            return IsNumber(ch) == false && IsAlphabet(ch) == false && ch != '.';
        }

        public static bool IsSameClass(this char first, char second) {
            if ((IsNumber(first) && IsNumber(second)) || (IsAlphabet(first) && IsAlphabet(second)))
                return true;
            return false;
        }
    }
}
