using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class RangeExpression : CellReferenceBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Range;
			}
		}

		public RangeExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new RangeExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(CellReferenceRangeExpression[] operands)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression;
			if (CellReferenceRangeExpression.TryCreateRange(operands[0], operands[1], out cellReferenceRangeExpression))
			{
				base.AttachToChildEvent(cellReferenceRangeExpression);
				return cellReferenceRangeExpression;
			}
			if (operands[0].CellReferenceRange == null || !operands[0].CellReferenceRange.IsInRange || operands[1].CellReferenceRange == null || !operands[1].CellReferenceRange.IsInRange)
			{
				return ErrorExpressions.ReferenceError;
			}
			return ErrorExpressions.ValueError;
		}
	}
}
