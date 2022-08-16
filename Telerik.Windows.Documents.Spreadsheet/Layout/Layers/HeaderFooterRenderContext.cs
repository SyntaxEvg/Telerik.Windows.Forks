using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class HeaderFooterRenderContext
	{
		public HeaderFooterRenderContext(Worksheet worksheet, DateTime dateTime, int pageNumber, int numberOfPages)
		{
			this.worksheet = worksheet;
			this.dateTime = dateTime;
			this.pageNumber = pageNumber;
			this.numberOfPages = numberOfPages;
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public DateTime DateTime
		{
			get
			{
				return this.dateTime;
			}
		}

		public int PageNumber
		{
			get
			{
				return this.pageNumber;
			}
		}

		public int NumberOfPages
		{
			get
			{
				return this.numberOfPages;
			}
		}

		readonly Worksheet worksheet;

		readonly DateTime dateTime;

		readonly int pageNumber;

		readonly int numberOfPages;
	}
}
