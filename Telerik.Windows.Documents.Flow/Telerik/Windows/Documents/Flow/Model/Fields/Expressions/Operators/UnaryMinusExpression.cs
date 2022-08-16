using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class UnaryMinusExpression : UnaryOperatorExpression
	{
		public UnaryMinusExpression(Expression operand)
			: base(operand)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.UnaryMinus;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(-base.Operand.GetResult().Value);
		}
	}
}
