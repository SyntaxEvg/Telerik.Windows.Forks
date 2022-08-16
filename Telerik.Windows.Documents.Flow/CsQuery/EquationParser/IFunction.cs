using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser
{
	interface IFunction : IOperand, IConvertible, ICloneable, IVariableContainer
	{
		string Name { get; }

		AssociationType AssociationType { get; }

		int RequiredParmCount { get; }

		int MaxParmCount { get; }

		IList<IOperand> Operands { get; }

		void AddOperand(IConvertible operand);

		void Compile();
	}
}
