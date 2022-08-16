using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	abstract class NativeOperation<T> : NativeOperation, IFunction<T>, IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public NativeOperation(string name)
			: base(name)
		{
		}

		public NativeOperation(string name, params IConvertible[] operands)
			: base(name)
		{
		}

		protected void Initialize()
		{
			this._IsInteger = Utils.IsIntegralType<T>();
		}

		protected override IConvertible GetValue()
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(long))
			{
				return base.GetValueLong();
			}
			if (typeFromHandle == typeof(double))
			{
				return base.GetValueDouble();
			}
			IConvertible value = (this.IsInteger ? ((double)base.GetValueLong()) : base.GetValueDouble());
			return (T)((object)Convert.ChangeType(value, typeof(T)));
		}

		public override bool IsInteger
		{
			get
			{
				return this._IsInteger;
			}
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return (IOperand<T>)base.Clone();
		}

		T IOperand<T>.Value
		{
			get
			{
				return (T)((object)base.Value);
			}
		}

		protected bool _IsInteger;
	}
}
