using System;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class PageSetupInfo
	{
		public PageSetupInfo()
		{
			this.fitToWidth = 1;
			this.fitToHeight = 1;
		}

		public PageOrientation PageOrientation
		{
			get
			{
				return this.pageOrientation;
			}
			set
			{
				this.pageOrientation = value;
			}
		}

		public PaperTypes PaperType
		{
			get
			{
				return this.paperType;
			}
			set
			{
				this.paperType = value;
			}
		}

		public ErrorsPrintStyle Errors
		{
			get
			{
				return this.errors;
			}
			set
			{
				this.errors = value;
			}
		}

		public int? FirstPageNumber
		{
			get
			{
				return this.firstPageNumber;
			}
			set
			{
				this.firstPageNumber = value;
			}
		}

		public int FitToHeight
		{
			get
			{
				return this.fitToHeight;
			}
			set
			{
				this.fitToHeight = value;
			}
		}

		public int FitToWidth
		{
			get
			{
				return this.fitToWidth;
			}
			set
			{
				this.fitToWidth = value;
			}
		}

		public PageOrder PageOrder
		{
			get
			{
				return this.pageOrder;
			}
			set
			{
				this.pageOrder = value;
			}
		}

		public int Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		PageOrientation pageOrientation;

		PaperTypes paperType;

		ErrorsPrintStyle errors;

		int? firstPageNumber;

		int fitToHeight;

		int fitToWidth;

		PageOrder pageOrder;

		int scale;
	}
}
