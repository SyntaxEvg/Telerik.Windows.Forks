using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class SubtractionExpression : BinaryOperatorExpression
	{
		public SubtractionExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Minus;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(base.Left.GetResult().Value - base.Right.GetResult().Value);
		}
	}
}
