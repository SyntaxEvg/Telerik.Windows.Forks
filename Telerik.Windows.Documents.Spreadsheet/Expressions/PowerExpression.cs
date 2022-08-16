using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class PowerExpression : NumberBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Power;
			}
		}

		public PowerExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new PowerExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(double[] operands)
		{
			double num = operands[0];
			double num2 = operands[1];
			if (num == 0.0)
			{
				if (num2 == 0.0)
				{
					return ErrorExpressions.NumberError;
				}
				if (num2 < 0.0)
				{
					return ErrorExpressions.DivisionByZero;
				}
			}
			else if (num < 0.0)
			{
				if (MathUtility.IsOddEgyptianFraction(num2))
				{
					return NumberExpression.CreateValidNumberOrErrorExpression(-Math.Pow(-num, num2));
				}
				if ((double)((int)num2) != num2)
				{
					return ErrorExpressions.NumberError;
				}
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(Math.Pow(num, num2));
		}
	}
}
