using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser
{
	interface IOperation : IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		IList<OperationType> Operators { get; }

		void AddOperand(IConvertible operand, bool invert);

		void ReplaceLastOperand(IOperand operand);
	}
}
