using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class SpreadsheetHyperlink
	{
		public CellRange Range
		{
			get
			{
				return this.cellRange;
			}
		}

		public HyperlinkInfo HyperlinkInfo
		{
			get
			{
				return this.hyperlinkInfo;
			}
		}

		internal SpreadsheetHyperlink(CellRange cellRange, HyperlinkInfo hyperlinkInfo)
		{
			this.cellRange = cellRange;
			this.hyperlinkInfo = hyperlinkInfo;
		}

		public override bool Equals(object obj)
		{
			SpreadsheetHyperlink spreadsheetHyperlink = obj as SpreadsheetHyperlink;
			return spreadsheetHyperlink != null && this.Range.Equals(spreadsheetHyperlink.Range) && this.HyperlinkInfo.Equals(spreadsheetHyperlink.HyperlinkInfo);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Range.GetHashCodeOrZero(), this.HyperlinkInfo.GetHashCodeOrZero());
		}

		readonly HyperlinkInfo hyperlinkInfo;

		readonly CellRange cellRange;
	}
}
