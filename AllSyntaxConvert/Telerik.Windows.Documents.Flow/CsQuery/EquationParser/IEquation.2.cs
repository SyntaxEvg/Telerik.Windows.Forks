using System;

namespace CsQuery.EquationParser
{
	interface IEquation<T> : IOperand<T>, IEquation, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		T GetValue(params IConvertible[] values);

		bool TryGetValue(out T result);

		bool TryGetValue(out T result, params IConvertible[] values);

		IEquation<T> Clone();

		IEquation<U> CloneAs<U>() where U : IConvertible;
	}
}
