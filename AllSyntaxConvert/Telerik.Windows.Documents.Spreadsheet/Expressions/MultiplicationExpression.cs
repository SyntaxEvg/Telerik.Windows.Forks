using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class MultiplicationExpression : NumberBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Multiply;
			}
		}

		public MultiplicationExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new MultiplicationExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(double[] operands)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression(operands[0] * operands[1]);
		}
	}
}
