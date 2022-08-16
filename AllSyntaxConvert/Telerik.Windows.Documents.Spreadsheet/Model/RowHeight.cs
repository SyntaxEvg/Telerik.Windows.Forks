using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class RowHeight : WidthHeightBase
	{
		public RowHeight(double value, bool isCustom)
			: base(value, isCustom)
		{
			Guard.ThrowExceptionIfInvalidRowHeight(value);
		}

		public static readonly RowHeight AutoFit = new RowHeight(SpreadsheetDefaultValues.DefaultRowHeight, false);
	}
}
