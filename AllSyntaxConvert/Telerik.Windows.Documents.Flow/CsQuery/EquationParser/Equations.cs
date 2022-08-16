using System;
using CsQuery.EquationParser.Implementation;

namespace CsQuery.EquationParser
{
	static class Equations
	{
		public static Equation<T> CreateEquation<T>() where T : IConvertible
		{
			return new Equation<T>();
		}

		public static Equation<T> CreateEquation<T>(IOperand operand) where T : IConvertible
		{
			return new Equation<T>(operand);
		}

		public static Equation<T> CreateEquation<T>(string text) where T : IConvertible
		{
			IEquationParser equationParser = new EquationParserEngine();
			IOperand operand = equationParser.Parse<T>(text);
			return new Equation<T>(operand);
		}

		public static Equation CreateEquation(string text)
		{
			IEquationParser equationParser = new EquationParserEngine();
			IOperand operand = equationParser.Parse(text);
			return new Equation(operand);
		}

		public static Equation CreateEquationOperand(string text)
		{
			IEquationParser equationParser = new EquationParserEngine();
			IOperand operand = equationParser.Parse(text);
			return new Equation(operand);
		}

		public static IOperand CreateOperand(IConvertible value)
		{
			if (value is IOperand)
			{
				return (IOperand)value;
			}
			if (value is string)
			{
				return Equations.CreateVariable((string)value);
			}
			return Equations.CreateLiteral(value);
		}

		public static IOperand CreateOperand<T>(T value) where T : IConvertible
		{
			if (value is IOperand)
			{
				return (IOperand)((object)value);
			}
			if (value is string)
			{
				return Equations.CreateVariable<T>((string)((object)value));
			}
			return Equations.CreateLiteral<T>(value);
		}

		public static IVariable CreateVariable<T>(string name) where T : IConvertible
		{
			return new Variable<T>(name);
		}

		public static IVariable CreateVariable(string name)
		{
			return new Variable(name);
		}

		public static ILiteral CreateLiteral<T>(IConvertible value) where T : IConvertible
		{
			return new Literal<T>(value);
		}

		public static ILiteral CreateLiteral(IConvertible value)
		{
			if (Utils.IsText(value))
			{
				return new Literal<string>(value.ToString());
			}
			if (Utils.IsIntegralType(value))
			{
				return new Literal<int>(Convert.ToInt64(value));
			}
			return new Literal<double>(Convert.ToDouble(value));
		}

		public static bool TryCreate(IConvertible value, out IOperand operand)
		{
			bool result;
			try
			{
				operand = Equations.CreateOperand(value);
				result = true;
			}
			catch
			{
				operand = null;
				result = false;
			}
			return result;
		}

		public static bool TryCreateEquation<T>(string text, out Equation<T> equation) where T : IConvertible
		{
			bool result;
			try
			{
				equation = Equations.CreateEquation<T>(text);
				result = true;
			}
			catch
			{
				equation = null;
				result = false;
			}
			return result;
		}
	}
}
