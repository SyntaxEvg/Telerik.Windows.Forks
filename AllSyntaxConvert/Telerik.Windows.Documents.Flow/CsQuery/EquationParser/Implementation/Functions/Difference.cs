using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Difference : Sum
	{
		public Difference(IOperand operand1, IOperand operand2)
		{
			this.AddOperand(operand1);
			this.AddOperand(operand2);
		}

		protected override OperationType PrimaryOperator
		{
			get
			{
				return OperationType.Subtraction;
			}
		}

		protected override OperationType ComplementaryOperator
		{
			get
			{
				return OperationType.Addition;
			}
		}
	}
}
