using System;

namespace CsQuery.EquationParser
{
	interface INativeOperation : IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
	}
}
