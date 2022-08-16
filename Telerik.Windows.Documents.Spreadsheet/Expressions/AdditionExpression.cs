using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class AdditionExpression : NumberBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Plus;
			}
		}

		public AdditionExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new AdditionExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(double[] operands)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(operands[0] + operands[1]);
		}
	}
}
