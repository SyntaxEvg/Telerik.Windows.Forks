using System;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Footers : HeadersFootersBase<Footer>
	{
		internal Footers(RadFlowDocument document, Section section)
			: base(document, section)
		{
		}

		protected override Footer CreateHeaderFooterInstance()
		{
			return new Footer(base.Document, base.Section);
		}
	}
}
