using System;

namespace CsQuery.EquationParser.Implementation
{
	class Literal : Operand, ILiteral, IOperand, IConvertible, ICloneable
	{
		public Literal()
		{
		}

		public Literal(IConvertible value)
		{
			this.Set(value);
		}

		public new ILiteral Clone()
		{
			return (ILiteral)base.Clone();
		}

		protected override IOperand GetNewInstance()
		{
			return new Literal();
		}

		protected override IOperand CopyTo(IOperand operand)
		{
			((Literal)operand).Set(this.GetValue());
			return operand;
		}

		protected override IConvertible GetValue()
		{
			return this._Value;
		}

		public override string ToString()
		{
			return base.Value.ToString();
		}

		public virtual void Set(IConvertible value)
		{
			this._Value = value;
		}

		protected IConvertible _Value;
	}
}
