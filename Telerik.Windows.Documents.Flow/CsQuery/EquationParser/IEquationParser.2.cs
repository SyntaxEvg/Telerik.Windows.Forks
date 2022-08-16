using System;

namespace CsQuery.EquationParser
{
	interface IEquationParser<T> : IEquationParser where T : IConvertible
	{
		bool TryParse(string text, out IOperand<T> operand);

		IOperand<T> Parse(string text);
	}
}
