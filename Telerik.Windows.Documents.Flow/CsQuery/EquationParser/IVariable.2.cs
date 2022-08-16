using System;

namespace CsQuery.EquationParser
{
	interface IVariable<T> : IOperand<T>, IVariable, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		Type Type { get; }

		IVariable<T> Clone();

		T Value { get; set; }
	}
}
