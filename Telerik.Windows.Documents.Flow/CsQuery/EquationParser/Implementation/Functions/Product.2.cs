using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Product<T> : Product, IFunction<T>, IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Product()
		{
		}

		public Product(params IConvertible[] operands)
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
			return new Product();
		}

		public new Product<T> Clone()
		{
			return this.Clone();
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}
	}
}
