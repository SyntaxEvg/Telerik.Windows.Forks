using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class NotEqualExpression : ComparisonOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.NotEqual;
			}
		}

		public NotEqualExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new NotEqualExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override bool CompareStringExpressions(StringExpression left, StringExpression right)
		{
			return !string.Equals(left.Value, right.Value, StringComparison.CurrentCultureIgnoreCase);
		}

		protected override bool CompareNumberExpressions(NumberExpression left, NumberExpression right)
		{
			return left.Value != right.Value;
		}

		protected override bool CompareBooleanExpressions(BooleanExpression left, BooleanExpression right)
		{
			return left.Value ^ right.Value;
		}

		protected override bool CompareDifferentTypeExpressions(ConstantExpression leftOperandValue, ConstantExpression rightOperandValue)
		{
			return true;
		}
	}
}
