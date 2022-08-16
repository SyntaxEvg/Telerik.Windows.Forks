using System;

namespace CsQuery.EquationParser
{
	interface ILiteral : IOperand, IConvertible, ICloneable
	{
		void Set(IConvertible value);

		ILiteral Clone();
	}
}
