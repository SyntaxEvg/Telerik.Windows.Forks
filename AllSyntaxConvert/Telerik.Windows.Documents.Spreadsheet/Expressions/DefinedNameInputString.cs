using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class DefinedNameInputString : UserInputStringBase<SpreadsheetNameExpression>
	{
		public override bool IsSheetNameDependent
		{
			get
			{
				return base.Expression.IsNameReferringWorksheet;
			}
		}

		public override bool IsWorkbookNameDependent
		{
			get
			{
				return base.Expression.IsNameReferringWorbook;
			}
		}

		public DefinedNameInputString(SpreadsheetNameExpression expression)
			: base(expression)
		{
		}

		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return base.Expression.ToString(cultureInfo);
		}

		public override UserInputStringBase Clone()
		{
			return new DefinedNameInputString(base.Expression);
		}
	}
}
