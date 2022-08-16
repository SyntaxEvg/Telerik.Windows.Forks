using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class SubtractionExpression : NumberBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Minus;
			}
		}

		public SubtractionExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new SubtractionExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(double[] operands)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(operands[0] - operands[1]);
		}
	}
}
