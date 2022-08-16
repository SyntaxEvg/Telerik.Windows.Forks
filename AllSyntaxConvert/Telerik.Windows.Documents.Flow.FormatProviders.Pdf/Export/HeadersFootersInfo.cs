using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export
{
	class HeadersFootersInfo
	{
		public Header GetLastUsedHeader(HeaderFooterType type)
		{
			switch (type)
			{
			case HeaderFooterType.Default:
				return this.defaultHeader;
			case HeaderFooterType.Even:
				return this.evenHeader;
			case HeaderFooterType.First:
				return this.firstHeader;
			default:
				throw new NotSupportedException();
			}
		}

		public void SetLastUsedHeader(HeaderFooterType type, Header header)
		{
			switch (type)
			{
			case HeaderFooterType.Default:
				this.defaultHeader = header;
				return;
			case HeaderFooterType.Even:
				this.evenHeader = header;
				return;
			case HeaderFooterType.First:
				this.firstHeader = header;
				return;
			default:
				throw new NotSupportedException();
			}
		}

		public Footer GetLastUsedFooter(HeaderFooterType type)
		{
			switch (type)
			{
			case HeaderFooterType.Default:
				return this.defaultFooter;
			case HeaderFooterType.Even:
				return this.evenFooter;
			case HeaderFooterType.First:
				return this.firstFooter;
			default:
				throw new NotSupportedException();
			}
		}

		public void SetLastUsedFooter(HeaderFooterType type, Footer footer)
		{
			switch (type)
			{
			case HeaderFooterType.Default:
				this.defaultFooter = footer;
				return;
			case HeaderFooterType.Even:
				this.evenFooter = footer;
				return;
			case HeaderFooterType.First:
				this.firstFooter = footer;
				return;
			default:
				throw new NotSupportedException();
			}
		}

		Header defaultHeader;

		Header firstHeader;

		Header evenHeader;

		Footer defaultFooter;

		Footer firstFooter;

		Footer evenFooter;
	}
}
