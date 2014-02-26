#Calculator
A calculator written in C#, supporting expression calculation and simple function.

##How to use
I have made a calculator-like UI. However, the class is written to support Assembly reference.

Use `using Net.AlexKing.Calculator.Core` to import the core classes. For normal expression calculation, use

	Calculate cal = new Calculate("1+2");
	Operand operand = cal.DoCalculation();
	double result = (double)operand.GetValue();

The static method `AddFunction`  in class `Function`  support simple user-defined functions, the function must follow the BNF:

	function::=<FunctionName> "(" <Parameter>{,<Parameter>} ")=" <Function Expression>

For example

	Function newFunc = new Function("test(x,y)=x*5+y");
	newFunc.Add();
	Calculate cal = new Calculate("test(1,2)");
	Console.WriteLn(cal.DoCalculation().GetValue());

The result will be `7`.