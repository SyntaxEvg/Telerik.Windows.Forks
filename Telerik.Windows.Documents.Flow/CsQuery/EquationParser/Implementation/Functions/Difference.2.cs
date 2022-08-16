using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Difference<T> : Difference, IFunction<T>, IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Difference(IOperand operand1, IOperand operand2)
			: base(operand1, operand2)
		{
		}

		public new T Value
		{
			get
			{
				return (T)((object)this.GetValue());
			}
		}

		public new Difference<T> Clone()
		{
			return this.Clone();
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}
	}
}
