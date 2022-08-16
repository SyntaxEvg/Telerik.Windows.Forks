using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class UnionExpression : CellReferenceBinaryOperatorExpression
	{
		public override OperatorInfo OperatorInfo
		{
			get
			{
				return OperatorInfos.Union;
			}
		}

		public UnionExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			return new UnionExpression(base.Left.CloneAndTranslate(context), base.Right.CloneAndTranslate(context));
		}

		protected override RadExpression GetValueOverride(CellReferenceRangeExpression[] operands)
		{
			if (!operands[0].CellReferenceRange.IsInRange || !operands[1].CellReferenceRange.IsInRange)
			{
				return ErrorExpressions.ReferenceError;
			}
			CellReferenceRangeExpression cellReferenceRangeExpression;
			if (CellReferenceRangeExpression.TryCreateUnion(operands[0], operands[1], out cellReferenceRangeExpression))
			{
				base.AttachToChildEvent(cellReferenceRangeExpression);
				return cellReferenceRangeExpression;
			}
			return ErrorExpressions.ValueError;
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			if (base.Left.ToString(cultureInfo) == "#REF!" || base.Right.ToString(cultureInfo) == "#REF!")
			{
				return "#REF!";
			}
			if (this.toStringValueCache == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.GetOperandAsString(base.Left, cultureInfo));
				stringBuilder.Append(cultureInfo.ListSeparator);
				stringBuilder.Append(base.GetOperandAsString(base.Right, cultureInfo));
				this.toStringValueCache = stringBuilder.ToString();
			}
			return this.toStringValueCache;
		}

		string toStringValueCache;
	}
}
