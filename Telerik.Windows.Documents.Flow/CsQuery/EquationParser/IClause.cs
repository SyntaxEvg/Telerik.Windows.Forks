using System;

namespace CsQuery.EquationParser
{
	interface IClause : IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		IClause Clone();
	}
}
