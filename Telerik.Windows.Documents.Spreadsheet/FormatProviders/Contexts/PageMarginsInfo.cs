using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	class PageMarginsInfo
	{
		public double Bottom
		{
			get
			{
				return this.bottom;
			}
			set
			{
				this.bottom = value;
			}
		}

		public double Footer
		{
			get
			{
				return this.footer;
			}
			set
			{
				this.footer = value;
			}
		}

		public double Header
		{
			get
			{
				return this.header;
			}
			set
			{
				this.header = value;
			}
		}

		public double Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
			}
		}

		public double Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		public double Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.top = value;
			}
		}

		double bottom;

		double footer;

		double header;

		double left;

		double right;

		double top;
	}
}
