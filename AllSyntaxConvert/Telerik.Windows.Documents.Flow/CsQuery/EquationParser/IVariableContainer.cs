using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser
{
	interface IVariableContainer
	{
		IEnumerable<IVariable> Variables { get; }
	}
}
