using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	abstract class UserInputStringBase
	{
		public virtual bool IsSheetNameDependent
		{
			get
			{
				return false;
			}
		}

		public virtual bool IsWorkbookNameDependent
		{
			get
			{
				return false;
			}
		}

		public abstract string ToString(SpreadsheetCultureHelper cultureInfo);

		public abstract UserInputStringBase Clone();
	}
}
