using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class AdditionExpression : BinaryOperatorExpression
	{
		public AdditionExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Plus;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(base.Left.GetResult().Value + base.Right.GetResult().Value);
		}
	}
}
