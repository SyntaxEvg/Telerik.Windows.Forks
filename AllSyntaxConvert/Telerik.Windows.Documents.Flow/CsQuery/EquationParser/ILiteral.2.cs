using System;

namespace CsQuery.EquationParser
{
	interface ILiteral<T> : IOperand<T>, ILiteral, IOperand, IConvertible, ICloneable where T : IConvertible
	{
		void Set(T value);

		ILiteral<T> Clone();
	}
}
