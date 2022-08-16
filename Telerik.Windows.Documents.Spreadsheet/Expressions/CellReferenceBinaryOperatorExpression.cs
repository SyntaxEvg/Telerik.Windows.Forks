using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class CellReferenceBinaryOperatorExpression : BinaryOperatorExpression<CellReferenceRangeExpression>
	{
		public override ArgumentType OperandsType
		{
			get
			{
				return ArgumentType.Reference;
			}
		}

		protected CellReferenceBinaryOperatorExpression(RadExpression left, RadExpression right)
			: base(left, right)
		{
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			if (base.Left.ToString(cultureInfo) == "#REF!" || base.Right.ToString(cultureInfo) == "#REF!")
			{
				return "#REF!";
			}
			return base.ToString(cultureInfo);
		}
	}
}
