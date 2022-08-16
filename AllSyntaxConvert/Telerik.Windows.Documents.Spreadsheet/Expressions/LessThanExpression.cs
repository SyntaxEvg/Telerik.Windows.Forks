using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class LessThanExpression : ComparisonOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.LessThan;
			}
		}

		public LessThanExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new LessThanExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override bool CompareStringExpressions(StringExpression left, StringExpression right)
		{
			return string.Compare(left.Value, right.Value, StringComparison.OrdinalIgnoreCase) < 0;
		}

		protected override bool CompareNumberExpressions(NumberExpression left, NumberExpression right)
		{
			return left.Value < right.Value;
		}

		protected override bool CompareBooleanExpressions(BooleanExpression left, BooleanExpression right)
		{
			return !left.Value && right.Value;
		}

		protected override bool CompareDifferentTypeExpressions(ConstantExpression leftOperandValue, ConstantExpression rightOperandValue)
		{
			int expressionTypeNumber = base.GetExpressionTypeNumber(leftOperandValue);
			int expressionTypeNumber2 = base.GetExpressionTypeNumber(rightOperandValue);
			return expressionTypeNumber < expressionTypeNumber2;
		}
	}
}
