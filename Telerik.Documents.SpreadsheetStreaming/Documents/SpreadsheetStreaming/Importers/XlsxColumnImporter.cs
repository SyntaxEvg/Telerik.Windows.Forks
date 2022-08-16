using System;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Importers
{
	class XlsxColumnImporter : IColumnImporter
	{
		internal XlsxColumnImporter(ColumnRange columnRange)
		{
			this.columnRange = columnRange;
		}

		public int FromIndex
		{
			get
			{
				return this.columnRange.FirstColumnNumber - 1;
			}
		}

		public int ToIndex
		{
			get
			{
				return this.columnRange.LastColumnNumber - 1;
			}
		}

		public bool IsCustomWidth
		{
			get
			{
				return this.columnRange.CustomWidth.Value;
			}
		}

		public double WidthInPixels
		{
			get
			{
				if (this.columnRange.CustomWidth.Value)
				{
					double value = this.columnRange.Width.Value;
					return UnitHelper.ExcelColumnWidthToPixelWidth(value);
				}
				return UnitHelper.CharactersCountToPixelWidth(DefaultValues.DefaultColumnWidthInCharacters);
			}
		}

		public double WidthInCharacters
		{
			get
			{
				return UnitHelper.PixelsToCharacterCount(this.WidthInPixels);
			}
		}

		public int OutlineLevel
		{
			get
			{
				return this.columnRange.OutlineLevel.Value;
			}
		}

		public bool IsHidden
		{
			get
			{
				return this.columnRange.Hidden.Value;
			}
		}

		readonly ColumnRange columnRange;
	}
}
