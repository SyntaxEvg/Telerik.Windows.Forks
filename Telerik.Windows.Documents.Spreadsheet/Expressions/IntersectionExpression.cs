using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class IntersectionExpression : CellReferenceBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Intersection;
			}
		}

		public IntersectionExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new IntersectionExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(CellReferenceRangeExpression[] operands)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression;
			if (CellReferenceRangeExpression.TryCreateIntersection(operands[0], operands[1], out cellReferenceRangeExpression))
			{
				base.AttachToChildEvent(cellReferenceRangeExpression);
				return cellReferenceRangeExpression;
			}
			return ErrorExpressions.NullError;
		}
	}
}
