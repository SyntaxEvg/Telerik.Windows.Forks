using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class HeaderFooterSettings
	{
		internal HeaderFooterSettings(Action updateOnChange)
		{
			Guard.ThrowExceptionIfNull<Action>(updateOnChange, "updateOnChange");
			this.updateOnChange = updateOnChange;
			this.headerContents = new HeaderFooterContentCollection(updateOnChange);
			this.footerContents = new HeaderFooterContentCollection(updateOnChange);
			this.differentOddAndEvenPages = false;
			this.differentFirstPage = false;
			this.scaleWithDocument = true;
			this.alignWithPageMargins = true;
		}

		internal HeaderFooterSettings(HeaderFooterSettings other, Action updateOnChange)
		{
			Guard.ThrowExceptionIfNull<Action>(updateOnChange, "updateOnChange");
			this.updateOnChange = updateOnChange;
			this.headerContents = new HeaderFooterContentCollection(other.headerContents, updateOnChange);
			this.footerContents = new HeaderFooterContentCollection(other.footerContents, updateOnChange);
			this.differentOddAndEvenPages = other.differentOddAndEvenPages;
			this.differentFirstPage = other.differentFirstPage;
			this.scaleWithDocument = other.scaleWithDocument;
			this.alignWithPageMargins = other.alignWithPageMargins;
		}

		public bool DifferentOddAndEvenPages
		{
			get
			{
				return this.differentOddAndEvenPages;
			}
			set
			{
				if (this.differentOddAndEvenPages != value)
				{
					this.differentOddAndEvenPages = value;
					this.updateOnChange();
				}
			}
		}

		public bool DifferentFirstPage
		{
			get
			{
				return this.differentFirstPage;
			}
			set
			{
				if (this.differentFirstPage != value)
				{
					this.differentFirstPage = value;
					this.updateOnChange();
				}
			}
		}

		public bool ScaleWithDocument
		{
			get
			{
				return this.scaleWithDocument;
			}
			set
			{
				if (this.scaleWithDocument != value)
				{
					this.scaleWithDocument = value;
					this.updateOnChange();
				}
			}
		}

		public bool AlignWithPageMargins
		{
			get
			{
				return this.alignWithPageMargins;
			}
			set
			{
				if (this.alignWithPageMargins != value)
				{
					this.alignWithPageMargins = value;
					this.updateOnChange();
				}
			}
		}

		public HeaderFooterContent Header
		{
			get
			{
				return this.headerContents.DefaultContent;
			}
		}

		public HeaderFooterContent Footer
		{
			get
			{
				return this.footerContents.DefaultContent;
			}
		}

		public HeaderFooterContent FirstPageHeader
		{
			get
			{
				return this.headerContents.FirstPageContent;
			}
		}

		public HeaderFooterContent FirstPageFooter
		{
			get
			{
				return this.footerContents.FirstPageContent;
			}
		}

		public HeaderFooterContent EvenPageHeader
		{
			get
			{
				return this.headerContents.EvenPageContent;
			}
		}

		public HeaderFooterContent EvenPageFooter
		{
			get
			{
				return this.footerContents.EvenPageContent;
			}
		}

		internal HeaderFooterContentCollection HeaderContents
		{
			get
			{
				return this.headerContents;
			}
		}

		internal HeaderFooterContentCollection FooterContents
		{
			get
			{
				return this.footerContents;
			}
		}

		internal IEnumerable<HeaderFooterContent> Contents
		{
			get
			{
				foreach (HeaderFooterContent content in this.headerContents)
				{
					yield return content;
				}
				foreach (HeaderFooterContent content2 in this.footerContents)
				{
					yield return content2;
				}
				yield break;
			}
		}

		internal void CopyFrom(HeaderFooterSettings fromHeaderFooterSettings)
		{
			this.DifferentOddAndEvenPages = fromHeaderFooterSettings.DifferentOddAndEvenPages;
			this.DifferentFirstPage = fromHeaderFooterSettings.DifferentFirstPage;
			this.ScaleWithDocument = fromHeaderFooterSettings.ScaleWithDocument;
			this.AlignWithPageMargins = fromHeaderFooterSettings.AlignWithPageMargins;
			this.Header.CopyFrom(fromHeaderFooterSettings.Header);
			this.Footer.CopyFrom(fromHeaderFooterSettings.Footer);
			this.FirstPageHeader.CopyFrom(fromHeaderFooterSettings.FirstPageHeader);
			this.FirstPageFooter.CopyFrom(fromHeaderFooterSettings.FirstPageFooter);
			this.EvenPageHeader.CopyFrom(fromHeaderFooterSettings.EvenPageHeader);
			this.EvenPageFooter.CopyFrom(fromHeaderFooterSettings.EvenPageFooter);
		}

		internal void Clear()
		{
			this.DifferentOddAndEvenPages = false;
			this.DifferentFirstPage = false;
			this.ScaleWithDocument = true;
			this.AlignWithPageMargins = true;
			foreach (HeaderFooterContent headerFooterContent in this.Contents)
			{
				headerFooterContent.Clear();
			}
		}

		internal const bool DefaultDifferentOddAndEvenPages = false;

		internal const bool DefaultDifferentFirstPage = false;

		internal const bool DefaultScaleWithDocument = true;

		internal const bool DefaultAlignWithPageMargins = true;

		readonly Action updateOnChange;

		readonly HeaderFooterContentCollection headerContents;

		readonly HeaderFooterContentCollection footerContents;

		bool differentOddAndEvenPages;

		bool differentFirstPage;

		bool scaleWithDocument;

		bool alignWithPageMargins;
	}
}
