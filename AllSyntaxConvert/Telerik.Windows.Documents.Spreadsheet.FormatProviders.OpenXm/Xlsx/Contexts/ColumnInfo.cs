using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	struct ColumnInfo
	{
		public ColumnInfo(double defaultColumnWidth)
		{
			this = default(ColumnInfo);
			this.Width = defaultColumnWidth;
		}

		public bool BestFit { get; set; }

		public bool IsCustom { get; set; }

		public double Width { get; set; }

		public bool Hidden { get; set; }

		public int OutlineLevel { get; set; }

		public int StyleIndex { get; set; }

		public override bool Equals(object obj)
		{
			ColumnInfo columnInfo = (ColumnInfo)obj;
			return this.BestFit == columnInfo.BestFit && this.IsCustom == columnInfo.IsCustom && this.Width == columnInfo.Width && this.Hidden == columnInfo.Hidden && this.OutlineLevel == columnInfo.OutlineLevel && this.StyleIndex == columnInfo.StyleIndex;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.BestFit.GetHashCode(), this.IsCustom.GetHashCode(), this.Width.GetHashCode(), this.Hidden.GetHashCode(), this.OutlineLevel.GetHashCode(), this.StyleIndex.GetHashCode());
		}
	}
}
