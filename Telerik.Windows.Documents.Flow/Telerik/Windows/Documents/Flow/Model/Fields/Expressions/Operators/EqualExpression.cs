using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class EqualExpression : ComparisonOperatorExpression
	{
		public EqualExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Equal;
			}
		}

		protected override bool Compare(double left, double right)
		{
			return left == right;
		}
	}
}
