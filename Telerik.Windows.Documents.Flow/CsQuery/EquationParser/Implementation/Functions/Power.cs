using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Power : NativeOperation, IOperation, IFunction, IOperand, IConvertible, ICloneable, IVariableContainer
	{
		public Power()
			: base("power")
		{
		}

		public Power(IOperand operand1, IOperand operand2)
			: base("power")
		{
			this.AddOperand(operand1);
			this.AddOperand(operand2);
		}

		public override int RequiredParmCount
		{
			get
			{
				return 2;
			}
		}

		public override int MaxParmCount
		{
			get
			{
				return 2;
			}
		}

		public override AssociationType AssociationType
		{
			get
			{
				return AssociationType.Power;
			}
		}

		protected override IOperand GetNewInstance()
		{
			return new Power();
		}

		protected override OperationType ComplementaryOperator
		{
			get
			{
				return (OperationType)0;
			}
		}

		protected override OperationType PrimaryOperator
		{
			get
			{
				return OperationType.Power;
			}
		}
	}
}
