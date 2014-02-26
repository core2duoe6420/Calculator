using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Net.AlexKing.Calculator.Core
{
    public sealed class Function : Calculate
    {
        private string funcName;
        private string[] parameters;

        public int ParameterCount {
            get { return parameters.Length; }
        }

        public string FuncName {
            get { return funcName; }
        }

        public Function(string expStr)
            : base() {
            expStr = expStr.Replace(" ", "");
            int equalIndex = expStr.IndexOf('=');
            string funcDefine = expStr.Substring(0, equalIndex);
            string funcEquation = expStr.Substring(equalIndex + 1);
            parseFuncDefine(funcDefine);
            this.Expression = funcEquation;
        }

        private void parseFuncDefine(string funcDefine) {
            int leftBracketsIndex = funcDefine.IndexOf('(');
            int rightBracketsIndex = funcDefine.IndexOf(')');
            if (leftBracketsIndex == -1 || rightBracketsIndex != funcDefine.Length - 1)
                throw new FormatException("Function define wrong");
            funcName = funcDefine.Substring(0, leftBracketsIndex);
            if (!isStringAllCharacter(funcName))
                throw new FormatException("Function name wrong");
            string funParameter = funcDefine.Substring(leftBracketsIndex + 1, funcDefine.Length - funcName.Length - 2);
            parameters = funParameter.Split(',');
            Selector paraSelector = new Selector();
            Selector operatorSelector = selectors.GetSelector("Operator");
            foreach (string parameter in parameters) {
                if (!isStringAllCharacter(parameter))
                    throw new FormatException("Function parameter wrong");
                if (operatorSelector.HasValue(parameter))
                    throw new FormatException("Function parameter conflicts with an existed function");
                paraSelector.AddValue(parameter, factory.GetOperand(parameter));
            }
            selectors.AddSelector(paraSelector);
        }

        private bool isStringAllCharacter(string str) {
            return Regex.IsMatch(str, "[a-zA-Z]+");
        }

        public Operand DoCalculation(List<Operand> operands) {
            //函数计算时将每一个参数替换为对应的值
            for (int i = operands.Count - 1; i >= 0; i--) {
                string parameter = parameters[operands.Count - 1 - i];
                foreach (Element ele in postfix) {
                    if (ele.isOperand) {
                        Operand operand = ele.TheOperand;
                        string symbol = operand.Symbol;
                        if (symbol != null && symbol == parameter)
                            operand.SetValue(operands[i]);
                    }

                }
            }
            return base.DoCalculation();
        }
        public void Add() {
            Operator newOperator = new Operator(OperatorPriority.functionLevel, OperatorType.function,
                ParameterCount, FuncName, delegate(List<Operand> operands) {
                return DoCalculation(operands);
            });
            selectors.GetSelector("Operator").AddValue(newOperator);
        }
    }
}
