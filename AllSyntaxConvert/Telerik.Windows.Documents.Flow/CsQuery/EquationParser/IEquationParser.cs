using System;

namespace CsQuery.EquationParser
{
	interface IEquationParser
	{
		bool TryParse(string text, out IOperand operand);

		IOperand Parse(string text);

		IOperand Parse<T>(string text) where T : IConvertible;

		string Error { get; }
	}
}
