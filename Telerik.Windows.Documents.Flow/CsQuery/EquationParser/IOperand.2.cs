using System;

namespace CsQuery.EquationParser
{
	interface IOperand<T> : IOperand, IConvertible, ICloneable where T : IConvertible
	{
		T Value { get; }

		IOperand<T> Clone();
	}
}
