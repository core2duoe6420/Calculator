using System;
using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public interface IParser<T>
    {
        List<T> Parse(string expression);
    }

    internal class DefaultParser : IParser<Element>
    {
        private SelectorCollection selectors;
        private InnerParser infixParser;
        private CalculateFactory factory;
        private List<Element> postfix;

        public DefaultParser(SelectorCollection selectors, CalculateFactory factory) {
            this.selectors = selectors;
            this.factory = factory;
        }
        public List<Element> Parse(string expression) {
            postfix = new List<Element>();
            infixParser = new InnerParser(this, expression);
            parseExpression();
            if (infixParser.HasNextElement())
                infixParser.ThrowException("Statement wrong at " + infixParser.GetNextElement(false).ToString());

            return postfix;
        }

        //语法分析器 提供一些比较函数以及对infix的遍历
        private class InnerParser
        {
            private DefaultParser outerParser;
            private string expression;
            private int nextLexerPos = 0;
            private int curLexerPos = 0;
            private Element curElement = null;
            public InnerParser(DefaultParser outerParser, string expression) {
                this.outerParser = outerParser;
                this.expression = expression;
            }

            //判断词法单元，数字则认为是操作数
            //字母则在选择器列表中依次查找标识符，找不到说明出错
            private Element getNewElement(string lex) {
                if (lex.Length == 0)
                    return null;
                if (lex[0].IsNumber())
                    return new Element(outerParser.factory.GetOperand(lex));

                Object obj = outerParser.selectors.GetValueInSelectors(lex);
                if (obj != null)
                    return new Element(obj);
                else
                    ThrowException("Unknown key words " + lex);
                return null;
            }

            private void setCharArraySpaces(char[] charArray, int pos) {
                for (int i = 0; i < pos; i++)
                    charArray[i] = ' ';
            }

            //词法分析
            //仅3种元素 数字 一个字符的运算符 全部由字母组成的标识符
            private Element lexerGetElement() {
                char[] lexical = new char[expression.Length];
                setCharArraySpaces(lexical, lexical.Length);
                int pos = 0;
                for (int i = curLexerPos; i < expression.Length; i++) {
                    char curChar = expression[i];
                    // we assume that a sign takes one char
                    if (pos != 0 && (curChar.IsSymbol() || !curChar.IsSameClass(lexical[pos - 1]))) {
                        nextLexerPos = i;
                        return getNewElement(new string(lexical).Trim());
                    }
                    lexical[pos++] = curChar;
                }
                if (pos != 0) {
                    nextLexerPos = expression.Length;
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
                return curLexerPos < expression.Length;
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
                curEle.TheOperator = selectors.GetValueInSelectors("pos") as Operator;
                infixParser.MoveToNextElement();
                parseExpression();
                postfix.Add(curEle);
            } else if (theOperator != null && infixParser.TestElement(curEle, "-")) {
                curEle.TheOperator = selectors.GetValueInSelectors("neg") as Operator;
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
                curEle.TheOperator = selectors.GetValueInSelectors("pos") as Operator;
                parseFactor();
                postfix.Add(frameEle);
                return;
            } else if (infixParser.TestElement(curEle, "-")) {
                curEle.TheOperator = selectors.GetValueInSelectors("neg") as Operator;
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
