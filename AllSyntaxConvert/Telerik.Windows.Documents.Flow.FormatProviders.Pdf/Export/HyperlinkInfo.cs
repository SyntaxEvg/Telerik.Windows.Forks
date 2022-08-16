using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export
{
	class HyperlinkInfo
	{
		public HyperlinkInfo(Uri uri)
		{
			Guard.ThrowExceptionIfNull<Uri>(uri, "uri");
			this.uri = uri;
		}

		public Uri Uri
		{
			get
			{
				return this.uri;
			}
		}

		readonly Uri uri;
	}
}
