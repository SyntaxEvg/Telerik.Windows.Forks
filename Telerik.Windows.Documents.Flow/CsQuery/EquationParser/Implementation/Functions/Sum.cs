using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Sum : NativeOperation, IOperation, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Sum()
			: base("sum")
		{
		}

		public Sum(params IConvertible[] operands)
			: base("sum", operands)
		{
		}

		public override AssociationType AssociationType
		{
			get
			{
				return AssociationType.Addition;
			}
		}

		protected override IOperand GetNewInstance()
		{
			return new Sum();
		}

		protected override OperationType PrimaryOperator
		{
			get
			{
				return OperationType.Addition;
			}
		}

		protected override OperationType ComplementaryOperator
		{
			get
			{
				return OperationType.Subtraction;
			}
		}
	}
}
