using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class PageMargins
	{
		public double Left { get; set; }

		public double Bottom { get; set; }

		public double Right { get; set; }

		public double Top { get; set; }

		public double Header { get; set; }

		public double Footer { get; set; }

		public PageMargins()
			: this(0.0)
		{
		}

		public PageMargins(double leftRight, double topBottom)
			: this(leftRight, topBottom, 0.0)
		{
		}

		public PageMargins(double left, double top, double right, double bottom)
			: this(left, top, right, bottom, 0.0, 0.0)
		{
		}

		public PageMargins(double all)
			: this(all, all, all)
		{
		}

		public PageMargins(double leftRight, double topBottom, double headerFooter)
			: this(leftRight, topBottom, leftRight, topBottom, headerFooter, headerFooter)
		{
		}

		public PageMargins(double left, double top, double right, double bottom, double header, double footer)
		{
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
			this.Header = header;
			this.Footer = footer;
		}

		internal static bool AreValidMarginsWithinPageSize(PageMargins pageMargins, Size rotatedPageSize, Size minimumContent)
		{
			return PageMargins.IsValidMarginInequation(pageMargins.Left, pageMargins.Right, rotatedPageSize.Width, minimumContent.Width) && PageMargins.IsValidMarginInequation(pageMargins.Top, pageMargins.Bottom, rotatedPageSize.Height, minimumContent.Height) && PageMargins.IsValidMarginInequation(pageMargins.Header, pageMargins.Footer, rotatedPageSize.Height, 0.0);
		}

		static bool IsValidMarginInequation(double firstMargin, double secondMargin, double maxMarginSum, double minimumContentSize = 0.0)
		{
			return firstMargin >= 0.0 && secondMargin >= 0.0 && minimumContentSize >= 0.0 && firstMargin + secondMargin + minimumContentSize < maxMarginSum;
		}

		public override bool Equals(object obj)
		{
			PageMargins pageMargins = obj as PageMargins;
			return pageMargins != null && (this.Bottom == pageMargins.Bottom && this.Top == pageMargins.Top && this.Right == pageMargins.Right && this.Left == pageMargins.Left && this.Header == pageMargins.Header) && this.Footer == pageMargins.Footer;
		}

		public override int GetHashCode()
		{
			return this.Bottom.GetHashCode() ^ this.Top.GetHashCode() ^ this.Left.GetHashCode() ^ this.Right.GetHashCode() ^ this.Footer.GetHashCode() ^ this.Header.GetHashCode();
		}

		public static readonly PageMargins NormalMargins = new PageMargins(UnitHelper.InchToDip(0.7), UnitHelper.InchToDip(0.75), UnitHelper.InchToDip(0.3));

		public static readonly PageMargins WideMargins = new PageMargins(UnitHelper.InchToDip(1.0), UnitHelper.InchToDip(1.0), UnitHelper.InchToDip(0.5));

		public static readonly PageMargins NarrowMargins = new PageMargins(UnitHelper.InchToDip(0.25), UnitHelper.InchToDip(0.75), UnitHelper.InchToDip(0.3));
	}
}
