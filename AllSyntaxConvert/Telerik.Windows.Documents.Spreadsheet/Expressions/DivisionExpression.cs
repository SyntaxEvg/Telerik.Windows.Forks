using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class DivisionExpression : NumberBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Divide;
			}
		}

		public DivisionExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new DivisionExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(double[] operands)
		{
			if (operands[1] == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = (operands[0] / operands[1]).DoubleWithPrecision();
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}
	}
}
