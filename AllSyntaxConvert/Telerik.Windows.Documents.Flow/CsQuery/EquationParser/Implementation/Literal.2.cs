using System;

namespace CsQuery.EquationParser.Implementation
{
	class Literal<T> : Literal, ILiteral<T>, IOperand<T>, ILiteral, IOperand, IConvertible, ICloneable where T : IConvertible
	{
		public Literal()
		{
		}

		public Literal(IConvertible value)
		{
			this.SetConvert(value);
		}

		public static implicit operator Literal<T>(int value)
		{
			return new Literal<T>(value);
		}

		public static implicit operator Literal<T>(double value)
		{
			return new Literal<T>(value);
		}

		public static implicit operator Literal<T>(string value)
		{
			return new Literal<T>(value);
		}

		public new T Value
		{
			get
			{
				return (T)((object)this._Value);
			}
		}

		public new ILiteral<T> Clone()
		{
			return (ILiteral<T>)this.CopyTo(this.GetNewInstance());
		}

		public void Set(T value)
		{
			this._Value = value;
		}

		protected override IOperand GetNewInstance()
		{
			return new Literal<T>(this.Value);
		}

		void SetConvert(IConvertible value)
		{
			this.Set((T)((object)Convert.ChangeType(value, typeof(T))));
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}

		void ILiteral.Set(IConvertible value)
		{
			this.Set((T)((object)Convert.ChangeType(value, typeof(T))));
		}
	}
}
