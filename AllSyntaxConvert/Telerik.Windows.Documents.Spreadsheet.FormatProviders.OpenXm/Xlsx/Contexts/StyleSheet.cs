using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class StyleSheet
	{
		public StyleSheet(bool isExport)
		{
			this.fontInfoTable = new ResourceIndexedTable<FontInfo>(isExport, 0);
			this.fillTable = new ResourceIndexedTable<IFill>(isExport, 0);
			this.cellValueFormatTable = new ResourceIndexedTable<CellValueFormat>(isExport, NumberFormatTypes.FirstCustomFormatId);
			this.bordersInfoTable = new ResourceIndexedTable<BordersInfo>(isExport, 0);
			this.styleInfoTable = new ResourceIndexedTable<StyleInfo>(isExport, 0);
			this.indexedColorTable = new ResourceIndexedTable<Color>(isExport, 0);
			this.directFormattingTable = new ResourceIndexedTable<FormattingRecord>(isExport, 0);
			this.styleFormattingTable = new ResourceIndexedTable<FormattingRecord>(isExport, 0);
		}

		public ResourceIndexedTable<FontInfo> FontInfoTable
		{
			get
			{
				return this.fontInfoTable;
			}
		}

		public ResourceIndexedTable<IFill> FillTable
		{
			get
			{
				return this.fillTable;
			}
		}

		public ResourceIndexedTable<CellValueFormat> CellValueFormatTable
		{
			get
			{
				return this.cellValueFormatTable;
			}
		}

		public ResourceIndexedTable<BordersInfo> BordersInfoTable
		{
			get
			{
				return this.bordersInfoTable;
			}
		}

		public ResourceIndexedTable<StyleInfo> StyleInfoTable
		{
			get
			{
				return this.styleInfoTable;
			}
		}

		public ResourceIndexedTable<FormattingRecord> DirectFormattingTable
		{
			get
			{
				return this.directFormattingTable;
			}
		}

		public ResourceIndexedTable<FormattingRecord> StyleFormattingTable
		{
			get
			{
				return this.styleFormattingTable;
			}
		}

		public ResourceIndexedTable<Color> IndexedColorTable
		{
			get
			{
				return this.indexedColorTable;
			}
		}

		public StyleInfo? GetStyleInfoByStyleFormattingRecordId(int formattingRecordId)
		{
			foreach (StyleInfo value in this.StyleInfoTable)
			{
				if (value.FormattingRecordId == formattingRecordId)
				{
					return new StyleInfo?(value);
				}
			}
			return null;
		}

		readonly ResourceIndexedTable<FontInfo> fontInfoTable;

		readonly ResourceIndexedTable<IFill> fillTable;

		readonly ResourceIndexedTable<CellValueFormat> cellValueFormatTable;

		readonly ResourceIndexedTable<BordersInfo> bordersInfoTable;

		readonly ResourceIndexedTable<StyleInfo> styleInfoTable;

		readonly ResourceIndexedTable<FormattingRecord> directFormattingTable;

		readonly ResourceIndexedTable<FormattingRecord> styleFormattingTable;

		readonly ResourceIndexedTable<Color> indexedColorTable;
	}
}
