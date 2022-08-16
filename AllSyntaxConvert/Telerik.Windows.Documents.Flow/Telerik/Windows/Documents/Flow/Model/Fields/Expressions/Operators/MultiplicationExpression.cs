using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class MultiplicationExpression : BinaryOperatorExpression
	{
		public MultiplicationExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Multiply;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(base.Left.GetResult().Value * base.Right.GetResult().Value);
		}
	}
}
