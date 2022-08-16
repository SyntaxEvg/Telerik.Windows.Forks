using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class GreaterThanExpression : ComparisonOperatorExpression
	{
		public GreaterThanExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.GreaterThan;
			}
		}

		protected override bool Compare(double left, double right)
		{
			return left > right;
		}
	}
}
