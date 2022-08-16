using System;
using CsQuery.EquationParser.Implementation;

namespace CsQuery.EquationParser
{
	interface IVariable : IOperand, IConvertible, ICloneable, IVariableContainer
	{
		string Name { get; }

		event EventHandler<VariableReadEventArgs> OnGetValue;

		IVariable Clone();
	}
}
