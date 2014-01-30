#Calculator
A calculator written in C#, supporting expression calculation and simple function.

##How to use
I have made a calculator-like UI. However, the class is written to support Assembly reference.

Use `using Net.AlexKing.Calculator.Core` to import the core classes. For normal expression calculation, use

	Calculate cal = new NormalCalculate("1+2");
	Operand operand = cal.DoCalculation();
	double result = (double)operand.GetValue();

The static method `AddFunction`  in class `Function`  support simple user-defined functions, the function must follow the BNF:

	function::=<FunctionName> "(" <Parameter>{,<Parameter>} ")=" <Function Expression>

For example

	Function.AddFunction("test(x,y)=x*5+y");
	Calculate cal = new NormalCalculate("test(1,2)");
	Console.WriteLn(cal.DoCalculation().GetValue());

The result will be `7`.

##How to expand
The class `Calculate` is an abstract class. The subclass must implement the abstract method `initialSelector()`, the subclass should add some `Selector` instance using method `addSelector()`. The `Selector` is a dictionary which stores a map of string key and `Operator` instance or constant. For example, `NormalCalculate` has two Selectors: `operatorSelector` and `constantSelector`. In `operatorSelector`, the operators like '+' '-' or 'sin' is defined. The class `Function` inherits from `NormalCalculate` and `AddFunction` add an item to `operatorSelector` to register the function name as an operator.

The class `Operand` is also an abstract class. `Operand` is the actual operand of operator. Normaly, an implementation of `Calculate` associates with an implementation of `Operand`. For example, `NormalCalculate` associates with `OperandDouble`. I also implement a `BigIntegerCalculate` associating with `OperandBigInteger` using `System.Numerics.BigInteger`.

In my design, some other features can be implemented by implement `Operand` and `Calculate`. For example, we can define a `OperandBinary` and `BinaryCalculate` to support binary calculation. However, I don't know whether it really works.

For more information, please read codes :-)