using System;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public abstract class HeaderNameRenderingConverterBase
	{
		internal string ConvertRowIndexToName(HeaderNameRenderingConverterContext context, int rowIndex)
		{
			return this.ConvertRowIndexToNameOverride(context, rowIndex);
		}

		internal string ConvertColumnIndexToName(HeaderNameRenderingConverterContext context, int columnIndex)
		{
			return this.ConvertColumnIndexToNameOverride(context, columnIndex);
		}

		protected virtual string ConvertRowIndexToNameOverride(HeaderNameRenderingConverterContext context, int rowIndex)
		{
			return NameConverter.ConvertRowIndexToName(rowIndex);
		}

		protected virtual string ConvertColumnIndexToNameOverride(HeaderNameRenderingConverterContext context, int columnIndex)
		{
			return NameConverter.ConvertColumnIndexToName(columnIndex);
		}
	}
}
