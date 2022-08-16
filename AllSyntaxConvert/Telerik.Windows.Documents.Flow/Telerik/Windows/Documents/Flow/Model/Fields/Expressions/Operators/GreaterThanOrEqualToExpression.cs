using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class GreaterThanOrEqualToExpression : ComparisonOperatorExpression
	{
		public GreaterThanOrEqualToExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.GreaterThanOrEqualTo;
			}
		}

		protected override bool Compare(double left, double right)
		{
			return left >= right;
		}
	}
}
