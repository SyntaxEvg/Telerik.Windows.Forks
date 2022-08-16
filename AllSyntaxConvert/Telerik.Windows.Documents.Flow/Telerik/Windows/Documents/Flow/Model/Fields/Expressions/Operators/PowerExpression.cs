using System;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class PowerExpression : BinaryOperatorExpression
	{
		public PowerExpression(Expression left, Expression right)
			: base(left, right)
		{
		}

		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Power;
			}
		}

		public override ExpressionResult GetResult()
		{
			double value = base.Left.GetResult().Value;
			double value2 = base.Right.GetResult().Value;
			if (value == 0.0)
			{
				if (value2 < 0.0)
				{
					return ErrorExpressions.GetExpressionResult(ExpressionErrorType.DivisionByZero);
				}
			}
			else if (value < 0.0 && (double)((int)value2) != value2)
			{
				return ErrorExpressions.GetExpressionResult(ExpressionErrorType.ExponentNotAnInteger);
			}
			double value3 = Math.Pow(value, value2);
			return new ExpressionResult(value3);
		}
	}
}
