using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser.Implementation
{
	abstract class Function : Operand, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Function(string name)
		{
			this.Name = name;
		}

		public IList<IOperand> Operands
		{
			get
			{
				return this._Operands.AsReadOnly();
			}
		}

		public string Name { get; protected set; }

		public IEnumerable<IVariable> Variables
		{
			get
			{
				foreach (IOperand operand in this.Operands)
				{
					if (operand is IVariableContainer)
					{
						foreach (IVariable variable in ((IVariableContainer)operand).Variables)
						{
							yield return variable;
						}
					}
				}
				yield break;
			}
		}

		public abstract int RequiredParmCount { get; }

		public abstract int MaxParmCount { get; }

		public abstract AssociationType AssociationType { get; }

		public new IFunction Clone()
		{
			return (IFunction)this.CopyTo(this.GetNewInstance());
		}

		public void Compile()
		{
			throw new NotImplementedException();
		}

		protected override IOperand CopyTo(IOperand operand)
		{
			IFunction function = (IFunction)operand;
			foreach (IOperand operand2 in this.Operands)
			{
				function.AddOperand(operand2.Clone());
			}
			return operand;
		}

		public virtual void AddOperand(IConvertible operand)
		{
			this._Operands.Add(Utils.EnsureOperand(operand));
		}

		public override string ToString()
		{
			return this.Name + "(" + string.Join<IOperand>(",", this.Operands) + ")";
		}

		protected IOperand FirstOperand
		{
			get
			{
				if (this.Operands.Count > 0)
				{
					return this.Operands[0];
				}
				return null;
			}
		}

		protected IOperand SecondOperand
		{
			get
			{
				if (this.Operands.Count > 1)
				{
					return this.Operands[1];
				}
				return null;
			}
		}

		protected List<IOperand> _Operands = new List<IOperand>();
	}
}
