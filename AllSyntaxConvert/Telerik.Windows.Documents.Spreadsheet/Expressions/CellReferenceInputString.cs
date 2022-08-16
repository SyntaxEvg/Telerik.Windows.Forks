using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class CellReferenceInputString : UserInputStringBase<CellReferenceRangeExpression>
	{
		public override bool IsSheetNameDependent
		{
			get
			{
				return base.Expression.IsValid;
			}
		}

		public CellReferenceInputString(CellReferenceRangeExpression expression)
			: base(expression)
		{
		}

		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return base.Expression.ToString(cultureInfo);
		}

		public override UserInputStringBase Clone()
		{
			return new CellReferenceInputString(base.Expression);
		}
	}
}
