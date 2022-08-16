using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class PercentExpression : UnaryOperatorExpression
	{
		public PercentExpression(Expression operand)
			: base(operand)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Percent;
			}
		}

		public override ExpressionResult GetResult()
		{
			return new ExpressionResult(base.Operand.GetResult().Value / 100.0);
		}
	}
}
