﻿using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Sum<T> : Sum, IFunction<T>, IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Sum()
		{
		}

		public Sum(params IConvertible[] operands)
			: base(operands)
		{
		}

		public new T Value
		{
			get
			{
				return (T)((object)Convert.ChangeType(this.GetValue(), typeof(T)));
			}
		}

		protected override IOperand GetNewInstance()
		{
			return new Sum<T>();
		}

		public new Sum<T> Clone()
		{
			return this.Clone();
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}
	}
}
