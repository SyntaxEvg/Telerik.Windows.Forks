using System;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Headers : HeadersFootersBase<Header>
	{
		internal Headers(RadFlowDocument document, Section section)
			: base(document, section)
		{
		}

		protected override Header CreateHeaderFooterInstance()
		{
			return new Header(base.Document, base.Section);
		}
	}
}
