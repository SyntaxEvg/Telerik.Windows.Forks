using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class EqualExpression : ComparisonOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Equal;
			}
		}

		public EqualExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new EqualExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override bool CompareStringExpressions(StringExpression left, StringExpression right)
		{
			return string.Equals(left.Value, right.Value, StringComparison.CurrentCultureIgnoreCase);
		}

		protected override bool CompareNumberExpressions(NumberExpression left, NumberExpression right)
		{
			return left.Value == right.Value;
		}

		protected override bool CompareBooleanExpressions(BooleanExpression left, BooleanExpression right)
		{
			return !(left.Value ^ right.Value);
		}

		protected override bool CompareDifferentTypeExpressions(ConstantExpression leftOperandValue, ConstantExpression rightOperandValue)
		{
			return false;
		}
	}
}
