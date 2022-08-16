using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Product : NativeOperation, IOperation, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Product()
			: base("product")
		{
		}

		public Product(params IConvertible[] operands)
			: base("product", operands)
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
			return new Product();
		}

		protected override OperationType PrimaryOperator
		{
			get
			{
				return OperationType.Multiplication;
			}
		}

		protected override OperationType ComplementaryOperator
		{
			get
			{
				return OperationType.Division;
			}
		}
	}
}
