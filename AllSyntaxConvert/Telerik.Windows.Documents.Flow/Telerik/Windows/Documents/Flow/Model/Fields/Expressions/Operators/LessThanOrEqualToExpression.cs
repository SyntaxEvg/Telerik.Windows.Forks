using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class LessThanOrEqualToExpression : ComparisonOperatorExpression
	{
		public LessThanOrEqualToExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.LessThanOrEqualTo;
			}
		}

		protected override bool Compare(double left, double right)
		{
			return left <= right;
		}
	}
}
