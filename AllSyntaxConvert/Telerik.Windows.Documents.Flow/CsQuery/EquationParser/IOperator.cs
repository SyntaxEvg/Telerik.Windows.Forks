using System;

namespace CsQuery.EquationParser
{
	interface IOperator : ICloneable
	{
		void Set(string value);

		bool TrySet(string value);

		OperationType OperationType { get; }

		AssociationType AssociationType { get; }

		bool IsInverted { get; }

		IOperator Clone();

		IOperation GetFunction();
	}
}
