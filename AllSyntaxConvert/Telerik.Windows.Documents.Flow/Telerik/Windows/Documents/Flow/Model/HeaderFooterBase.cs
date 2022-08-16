using System;

namespace Telerik.Windows.Documents.Flow.Model
{
	public abstract class HeaderFooterBase : BlockContainerBase
	{
		internal HeaderFooterBase(RadFlowDocument document, Section section)
			: base(document)
		{
			base.SetParent(section);
		}
	}
}
