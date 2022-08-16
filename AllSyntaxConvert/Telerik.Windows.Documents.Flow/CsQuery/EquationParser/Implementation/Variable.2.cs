using System;

namespace CsQuery.EquationParser.Implementation
{
	class Variable<T> : Variable, IVariable<T>, IOperand<T>, IVariable, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Variable()
		{
		}

		public Variable(string name)
		{
			base.Name = name;
		}

		public new IVariable<T> Clone()
		{
			return (IVariable<T>)base.Clone();
		}

		public IVariable<U> CloneAs<U>() where U : IConvertible
		{
			throw new NotImplementedException();
		}

		public new T Value
		{
			get
			{
				return (T)((object)base.Value);
			}
			set
			{
				base.Value = value;
			}
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}
	}
}
