using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class LessThanExpression : ComparisonOperatorExpression
	{
		public LessThanExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.LessThan;
			}
		}

		protected override bool Compare(double left, double right)
		{
			return left < right;
		}
	}
}
