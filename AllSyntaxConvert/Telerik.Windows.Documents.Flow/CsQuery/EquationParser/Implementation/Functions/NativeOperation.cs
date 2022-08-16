using System;
using System.Collections.Generic;

namespace CsQuery.EquationParser.Implementation.Functions
{
	abstract class NativeOperation : Function, IOperation, INativeOperation, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public NativeOperation(string name)
			: base(name)
		{
		}

		public NativeOperation(string name, params IConvertible[] operands)
			: base(name)
		{
			foreach (IConvertible value in operands)
			{
				this.AddOperand(Equations.CreateOperand(value));
			}
		}

		protected abstract OperationType ComplementaryOperator { get; }

		protected abstract OperationType PrimaryOperator { get; }

		public override bool IsInteger
		{
			get
			{
				return base.IsInteger;
			}
		}

		public IList<OperationType> Operators
		{
			get
			{
				return this._Operators.AsReadOnly();
			}
		}

		protected override IOperand CopyTo(IOperand operand)
		{
			NativeOperation nativeOperation = (NativeOperation)operand;
			foreach (OperationType item in this._Operators)
			{
				nativeOperation._Operators.Add(item);
			}
			return base.CopyTo(nativeOperation);
		}

		public override void AddOperand(IConvertible operand)
		{
			base.AddOperand(Utils.EnsureOperand(operand));
			this._Operators.Add(this.PrimaryOperator);
		}

		public virtual void AddOperand(IConvertible operand, bool invert)
		{
			base.AddOperand(Utils.EnsureOperand(operand));
			this._Operators.Add(invert ? this.ComplementaryOperator : this.PrimaryOperator);
		}

		public void ReplaceLastOperand(IOperand operand)
		{
			if (base.Operands.Count == 0)
			{
				throw new InvalidOperationException("There are no operands to replace.");
			}
			int index = base.Operands.Count - 1;
			NativeOperation nativeOperation = base.Operands[index] as NativeOperation;
			if (nativeOperation != null && nativeOperation.Operands.Count > 1 && nativeOperation.AssociationType == AssociationType.Multiplicaton)
			{
				IOperation operation = nativeOperation;
				operation.ReplaceLastOperand(operand);
				return;
			}
			this._Operands[base.Operands.Count - 1] = operand;
		}

		public override int RequiredParmCount
		{
			get
			{
				return 1;
			}
		}

		public override int MaxParmCount
		{
			get
			{
				return 1;
			}
		}

		public override string ToString()
		{
			string text = this.WrapParenthesis(base.Operands[0]);
			for (int i = 1; i < base.Operands.Count; i++)
			{
				text = text + this.OperationTypeName(this.Operators[i]) + this.WrapParenthesis(base.Operands[i]);
			}
			return text;
		}

		protected override IConvertible GetValue()
		{
			return this.GetValueDouble();
		}

		protected double GetValueDouble()
		{
			double num = Convert.ToDouble(base.Operands[0].Value);
			for (int i = 1; i < base.Operands.Count; i++)
			{
				double num2 = Convert.ToDouble(base.Operands[i].Value);
				switch (this.Operators[i])
				{
				case OperationType.Addition:
					num += num2;
					break;
				case OperationType.Subtraction:
					num -= num2;
					break;
				case OperationType.Multiplication:
					num *= num2;
					break;
				case OperationType.Division:
					num /= num2;
					break;
				case OperationType.Modulus:
					num %= num2;
					break;
				case OperationType.Power:
					num = Math.Pow(num, num2);
					break;
				}
			}
			return num;
		}

		protected long GetValueLong()
		{
			long num = Convert.ToInt64(base.Operands[0].Value);
			for (int i = 1; i < base.Operands.Count; i++)
			{
				long num2 = Convert.ToInt64(base.Operands[i].Value);
				switch (this.Operators[i])
				{
				case OperationType.Addition:
					num += num2;
					break;
				case OperationType.Subtraction:
					num -= num2;
					break;
				case OperationType.Multiplication:
					num *= num2;
					break;
				case OperationType.Division:
					num /= num2;
					break;
				case OperationType.Modulus:
					num %= num2;
					break;
				case OperationType.Power:
					num = (long)Math.Pow(Convert.ToDouble(num), Convert.ToDouble(num2));
					break;
				}
			}
			return num;
		}

		protected string WrapParenthesis(IOperand operand)
		{
			INativeOperation nativeOperation = operand as INativeOperation;
			if (nativeOperation != null && nativeOperation.Operands.Count > 1 && nativeOperation.AssociationType == AssociationType.Addition)
			{
				return "(" + operand.ToString() + ")";
			}
			return operand.ToString();
		}

		string OperationTypeName(OperationType operationType)
		{
			return "+-*/%^".Substring(operationType - OperationType.Addition, 1).ToString();
		}

		protected List<OperationType> _Operators = new List<OperationType>();
	}
}
