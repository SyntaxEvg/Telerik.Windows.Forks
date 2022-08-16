using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser.Implementation
{
	class Equation : Operand, IEquation, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Equation()
		{
			this.Initialize();
		}

		public Equation(IOperand operand)
		{
			this.Initialize();
			this.Operand = operand;
		}

		protected virtual void Initialize()
		{
			this._VariableValues = new OrderedDictionary<string, IConvertible>();
		}

		public IOrderedDictionary<string, IConvertible> VariableValues
		{
			get
			{
				return this._VariableValues;
			}
		}

		public IOperand Operand
		{
			get
			{
				return this._Operand;
			}
			set
			{
				this._Operand = value;
				this.VariableValues.Clear();
				if (value != null && value is IVariableContainer)
				{
					foreach (IVariable variable in ((IVariableContainer)value).Variables)
					{
						this.AddVariable(variable);
					}
				}
			}
		}

		public IEnumerable<IVariable> Variables
		{
			get
			{
				if (this.Operand is IVariableContainer)
				{
					return ((IVariableContainer)this.Operand).Variables;
				}
				return Utils.EmptyEnumerable<IVariable>();
			}
		}

		public new IEquation Clone()
		{
			return (IEquation)this.CopyTo(this.GetNewInstance());
		}

		public void Compile()
		{
			if (this.Operand is IFunction)
			{
				((IFunction)this.Operand).Compile();
			}
		}

		public bool TryGetValue(out IConvertible result)
		{
			bool result2;
			try
			{
				result = base.Value;
				result2 = true;
			}
			catch
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		public virtual bool TryGetValue(out IConvertible result, params IConvertible[] values)
		{
			bool result2;
			try
			{
				for (int i = 0; i < values.Length; i++)
				{
					this.SetVariable(i, values[i]);
				}
				result = base.Value;
				result2 = true;
			}
			catch
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		public virtual void SetVariable(string name, IConvertible value)
		{
			this.VariableValues[name] = value;
		}

		public virtual void SetVariable(int index, IConvertible value)
		{
			if (this.VariableValues.Count == index)
			{
				this.SetVariable(this.VariableValues.Count.ToString(), value);
				return;
			}
			this.SetVariable(this._VariableValues.Keys[index], value);
		}

		public virtual void SetVariable<U>(string name, U value) where U : IConvertible
		{
			this.SetVariable(name, value);
		}

		protected override IConvertible GetValue()
		{
			return this.Operand.Value;
		}

		public IConvertible GetValue(params IConvertible[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				this.SetVariable(i, values[i]);
			}
			return base.Value;
		}

		public override string ToString()
		{
			return this.Operand.ToString();
		}

		protected override IOperand GetNewInstance()
		{
			return new Equation();
		}

		protected override IOperand CopyTo(IOperand operand)
		{
			IEquation equation = (IEquation)operand;
			equation.Operand = this.Operand.Clone();
			return operand;
		}

		protected void Variable_OnGetValue(object sender, VariableReadEventArgs e)
		{
			IConvertible convertible;
			if (this.VariableValues.TryGetValue(e.Name, out convertible))
			{
				e.Value = this.VariableValues[e.Name];
				return;
			}
			throw new InvalidOperationException("The value for variable '" + e.Name + "' was not set.");
		}

		protected void AddVariable(IVariable variable)
		{
			if (!this.VariableValues.ContainsKey(variable.Name))
			{
				this._VariableValues[variable.Name] = null;
			}
			variable.OnGetValue += this.Variable_OnGetValue;
		}

		IEnumerable<IVariable> IVariableContainer.Variables
		{
			get
			{
				return this.Variables;
			}
		}

		IOperand _Operand;

		OrderedDictionary<string, IConvertible> _VariableValues;
	}
}
