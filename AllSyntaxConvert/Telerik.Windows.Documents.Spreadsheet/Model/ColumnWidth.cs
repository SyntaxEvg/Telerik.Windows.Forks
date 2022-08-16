using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ColumnWidth : WidthHeightBase
	{
		internal bool IsAutoFit
		{
			get
			{
				return !base.IsCustom && base.Value != SpreadsheetDefaultValues.DefaultColumnWidth;
			}
		}

		public ColumnWidth(double value, bool isCustom)
			: base(value, isCustom)
		{
			Guard.ThrowExceptionIfInvalidColumnWidth(value);
		}
	}
}
