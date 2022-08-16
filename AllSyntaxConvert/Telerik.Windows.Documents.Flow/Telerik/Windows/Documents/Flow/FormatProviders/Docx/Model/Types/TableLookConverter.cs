using System;
using System.Globalization;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	class TableLookConverter : IStringConverter<TableLooks>
	{
		public TableLooks ConvertFromString(string hexValue)
		{
			int num;
			if (int.TryParse(hexValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				num >>= 5;
				TableLooks tableLooks = (TableLooks)num;
				tableLooks ^= TableLooks.BandedColumns;
				return tableLooks ^ TableLooks.BandedRows;
			}
			return TableLooks.BandedRows | TableLooks.BandedColumns;
		}

		public string ConvertToString(TableLooks tableLook)
		{
			tableLook ^= TableLooks.BandedColumns;
			tableLook ^= TableLooks.BandedRows;
			int num = (int)tableLook;
			return (num << 5).ToString("X4");
		}
	}
}
