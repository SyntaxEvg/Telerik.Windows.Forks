using System;

namespace CsQuery.EquationParser
{
	interface IOperand : IConvertible, ICloneable
	{
		IConvertible Value { get; }

		bool IsInteger { get; }

		IOperand Clone();
	}
}
