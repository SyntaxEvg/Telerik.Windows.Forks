using System;

namespace CsQuery.EquationParser.Implementation
{
	class Equation<T> : Equation, IEquation<T>, IOperand<T>, IEquation, IOperand, IConvertible, ICloneable, IVariableContainer where T : IConvertible
	{
		public Equation()
		{
		}

		public Equation(IConvertible operand)
		{
			base.Operand = Utils.EnsureOperand(operand);
		}

		public new IEquation<T> Clone()
		{
			return (IEquation<T>)this.CopyTo(this.GetNewInstance());
		}

		public IEquation<U> CloneAs<U>() where U : IConvertible
		{
			return (IEquation<U>)this.CloneAsImpl<U>();
		}

		public new T GetValue(params IConvertible[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.SetVariable(i, values[i]);
			}
			return this.Value;
		}

		public bool TryGetValue(out T result)
		{
			IConvertible value;
			if (base.TryGetValue(out value))
			{
				result = (T)((object)Convert.ChangeType(value, typeof(T)));
				return true;
			}
			result = default(T);
			return false;
		}

		public virtual bool TryGetValue(out T result, params IConvertible[] values)
		{
			bool result2;
			try
			{
				for (int i = 0; i < values.Length; i++)
				{
					this.SetVariable(i, values[i]);
				}
				result = (T)((object)Convert.ChangeType(this.Value, typeof(T)));
				result2 = true;
			}
			catch
			{
				result = default(T);
				result2 = false;
			}
			return result2;
		}

		public new T Value
		{
			get
			{
				return (T)((object)Convert.ChangeType(base.Operand.Value, typeof(T)));
			}
		}

		public override string ToString()
		{
			if (base.Operand != null)
			{
				return base.Operand.ToString();
			}
			return "";
		}

		protected IOperand<U> CloneAsImpl<U>() where U : IConvertible
		{
			Equation<U> equation = new Equation<U>();
			this.CopyTo(equation);
			return equation;
		}

		protected override IOperand GetNewInstance()
		{
			return new Equation<T>();
		}

		protected override IOperand CopyTo(IOperand operand)
		{
			this.CopyTo((IEquation)operand);
			return operand;
		}

		IConvertible IEquation.GetValue(params IConvertible[] values)
		{
			return this.GetValue(values);
		}

		bool IEquation.TryGetValue(out IConvertible value)
		{
			T t;
			if (this.TryGetValue(out t))
			{
				value = t;
				return true;
			}
			value = default(T);
			return false;
		}

		bool IEquation.TryGetValue(out IConvertible value, params IConvertible[] variableValues)
		{
			T t;
			if (this.TryGetValue(out t, variableValues))
			{
				value = t;
				return true;
			}
			value = default(T);
			return false;
		}

		IOperand<T> IOperand<T>.Clone()
		{
			return this.Clone();
		}

		IEquation IEquation.Clone()
		{
			return this.Clone();
		}
	}
}
