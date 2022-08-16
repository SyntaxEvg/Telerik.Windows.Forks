using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class TextCellValue : CellValueBase<string>
	{
		public override CellValueType ValueType
		{
			get
			{
				return CellValueType.Text;
			}
		}

		public override string RawValue
		{
			get
			{
				return base.Value.ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			}
		}

		internal TextCellValue(string value)
			: base(value)
		{
		}

		protected override string GetValueAsStringOverride(CellValueFormat format = null)
		{
			return this.RawValue;
		}
	}
}
