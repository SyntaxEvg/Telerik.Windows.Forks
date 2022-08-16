using System;

namespace CsQuery.EquationParser.Implementation.Functions
{
	class Abs : Function
	{
		public Abs()
			: base("abs")
		{
		}

		protected override IConvertible GetValue()
		{
			IConvertible value = base.FirstOperand.Value;
			if (Utils.IsIntegralType(value))
			{
				return Math.Abs(Convert.ToInt64(value));
			}
			return Math.Abs(Convert.ToDouble(value));
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

		public override AssociationType AssociationType
		{
			get
			{
				return AssociationType.Function;
			}
		}

		protected override IOperand GetNewInstance()
		{
			return new Abs();
		}
	}
}
