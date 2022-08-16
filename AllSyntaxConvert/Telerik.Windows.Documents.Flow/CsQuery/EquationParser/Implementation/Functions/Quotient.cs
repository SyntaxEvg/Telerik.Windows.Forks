using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Quotient : NativeOperation, IOperation, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Quotient()
			: base("quotient")
		{
		}

		public Quotient(params IConvertible[] operands)
			: base("quotient", operands)
		{
		}

		public override AssociationType AssociationType
		{
			get
			{
				return AssociationType.Multiplicaton;
			}
		}

		protected override IOperand GetNewInstance()
		{
			return new Quotient();
		}

		protected override OperationType PrimaryOperator
		{
			get
			{
				return OperationType.Division;
			}
		}

		protected override OperationType ComplementaryOperator
		{
			get
			{
				return OperationType.Multiplication;
			}
		}
	}
}
