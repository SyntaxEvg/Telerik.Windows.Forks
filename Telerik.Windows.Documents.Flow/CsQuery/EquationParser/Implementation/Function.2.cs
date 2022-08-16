using System;

namespace CsQuery.EquationParser.Implementation
{
	abstract class Function<T> : Function, IFunction<T>, IOperand<T>, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Function(string name)
			: base(name)
		{
			base.Name = name;
		}

		public new T Value
		{
			get
			{
				return (T)((object)base.Value);
			}
		}

		public new IFunction<T> Clone()
		{
			return this.Clone();
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}
	}
}
