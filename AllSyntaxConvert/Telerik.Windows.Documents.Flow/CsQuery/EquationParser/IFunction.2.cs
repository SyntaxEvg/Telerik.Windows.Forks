using System;

namespace CsQuery.EquationParser
{
	interface IFunction<T> : IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
	}
}
