using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	abstract class ComparisonOperatorExpression : BinaryOperatorExpression
	{
		public ComparisonOperatorExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override ExpressionResult GetResult()
		{
			double value = base.Left.GetResult().Value;
			double value2 = base.Right.GetResult().Value;
			return new ExpressionResult((double)(this.Compare(value, value2) ? 1 : 0));
		}

		protected abstract bool Compare(double left, double right);
	}
}
