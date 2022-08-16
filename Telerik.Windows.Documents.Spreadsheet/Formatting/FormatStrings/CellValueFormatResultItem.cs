using System;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public class CellValueFormatResultItem
	{
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public bool IsTransparent
		{
			get
			{
				return this.isTransparent;
			}
		}

		public bool ShouldExpand
		{
			get
			{
				return this.shouldExpand;
			}
		}

		public bool ApplyFormat
		{
			get
			{
				return this.applyFormat;
			}
		}

		public CellValueFormatResultItem(string text, bool isTransparent, bool shouldExpand, bool applyFormat)
		{
			this.text = text;
			this.isTransparent = isTransparent;
			this.shouldExpand = shouldExpand;
			this.applyFormat = applyFormat;
		}

		readonly string text;

		readonly bool isTransparent;

		readonly bool shouldExpand;

		readonly bool applyFormat;
	}
}
