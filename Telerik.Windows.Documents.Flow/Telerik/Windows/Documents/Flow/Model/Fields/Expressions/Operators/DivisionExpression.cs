using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class DivisionExpression : BinaryOperatorExpression
	{
		public DivisionExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Divide;
			}
		}

		public override ExpressionResult GetResult()
		{
			double value = base.Right.GetResult().Value;
			if (value == 0.0)
			{
				return ErrorExpressions.GetExpressionResult(ExpressionErrorType.DivisionByZero);
			}
			return new ExpressionResult(base.Left.GetResult().Value / value);
		}
	}
}
