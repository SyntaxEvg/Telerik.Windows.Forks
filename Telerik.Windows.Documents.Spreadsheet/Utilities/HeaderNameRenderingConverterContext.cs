using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public class HeaderNameRenderingConverterContext
	{
		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public CellRange VisibleRange
		{
			get
			{
				return this.visibleRange;
			}
		}

		internal HeaderNameRenderingConverterContext(Worksheet worksheet, CellRange visibleRange)
		{
			this.worksheet = worksheet;
			this.visibleRange = visibleRange;
		}

		readonly Worksheet worksheet;

		readonly CellRange visibleRange;
	}
}
