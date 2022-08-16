using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class CellReferenceRangeExpressionToken : ExpressionToken
	{
		public CellReferenceExpressionToken Left { get; set; }

		public CellReferenceExpressionToken Right { get; set; }

		public CellReferenceRangeExpressionToken(CellReferenceExpressionToken left, CellReferenceExpressionToken right)
			: base(ExpressionTokenType.A1CellReferenceRange, left.Value + ":" + right.Value)
		{
			this.Left = left;
			this.Right = right;
		}
	}
}
