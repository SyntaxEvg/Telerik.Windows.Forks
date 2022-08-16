using System;

namespace CsQuery.EquationParser.Implementation
{
	abstract class Operand<T> : Operand, IOperand<T>, IOperand, IConvertible, ICloneable where T : IConvertible
	{
		public Operand()
		{
		}

		public new T Value
		{
			get
			{
				return (T)((object)this.GetValue());
			}
		}

		public new IOperand<T> Clone()
		{
			return this.Clone();
		}

		public override string ToString()
		{
			T value = this.Value;
			return value.ToString();
		}

		IConvertible IOperand.Value
		{
			get
			{
				return this.Value;
			}
		}
	}
}
