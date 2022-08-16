using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Quotient<T> : Quotient, IFunction<T>, IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Quotient()
		{
		}

		public Quotient(params IConvertible[] operands)
			: base(operands)
		{
		}

		public new T Value
		{
			get
			{
				return (T)((object)this.GetValue());
			}
		}

		protected override IOperand GetNewInstance()
		{
			return new Quotient();
		}

		public new Quotient<T> Clone()
		{
			return this.Clone();
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}
	}
}
