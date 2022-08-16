using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class NotEqualExpression : ComparisonOperatorExpression
	{
		public NotEqualExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.NotEqual;
			}
		}

		protected override bool Compare(double left, double right)
		{
			return left != right;
		}
	}
}
