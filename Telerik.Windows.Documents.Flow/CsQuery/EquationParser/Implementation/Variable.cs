using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser.Implementation
{
	class Variable : Operand, IVariable, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Variable()
		{
		}

		public Variable(string name)
		{
			this.Name = name;
		}

		public event EventHandler<VariableReadEventArgs> OnGetValue;

		public string Name { get; set; }

		public Type Type
		{
			get
			{
				if (this._Value != null)
				{
					return this._Value.GetType();
				}
				return typeof(IConvertible);
			}
		}

		public new IConvertible Value
		{
			get
			{
				return this.GetValue();
			}
			set
			{
				this._Value = value;
			}
		}

		protected override IConvertible GetValue()
		{
			if (this.OnGetValue == null)
			{
				throw new InvalidOperationException("This variable is not bound to a formula, so it's value cannot be read.");
			}
			VariableReadEventArgs variableReadEventArgs = new VariableReadEventArgs(this.Name);
			variableReadEventArgs.Type = this.Type;
			this.OnGetValue(this, variableReadEventArgs);
			if (this.Type != typeof(IConvertible))
			{
				this._Value = (IConvertible)Convert.ChangeType(variableReadEventArgs.Value, this.Type);
			}
			else
			{
				this._Value = variableReadEventArgs.Value;
			}
			return this._Value;
		}

		public new IVariable Clone()
		{
			return (IVariable)this.CopyTo(this.GetNewInstance());
		}

		protected override IOperand GetNewInstance()
		{
			return new Variable(this.Name);
		}

		protected override IOperand CopyTo(IOperand operand)
		{
			return operand;
		}

		public override string ToString()
		{
			return this.Name;
		}

		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			IVariable variable = obj as IVariable;
			return obj != null && variable.Name == this.Name;
		}

		public IEnumerable<IVariable> Variables
		{
			get
			{
				yield return this;
				yield break;
			}
		}

		IConvertible IOperand.Value
		{
			get
			{
				return this.Value;
			}
		}

		IVariable IVariable.Clone()
		{
			return this.Clone();
		}

		protected IConvertible _Value;
	}
}
