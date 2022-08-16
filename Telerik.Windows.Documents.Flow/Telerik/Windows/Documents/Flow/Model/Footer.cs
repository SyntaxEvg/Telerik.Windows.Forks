using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Footer : HeaderFooterBase
	{
		internal Footer(RadFlowDocument document, Section section)
			: base(document, section)
		{
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Footer;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Footer footer = new Footer(cloneContext.Document, cloneContext.CurrentSection);
			footer.Blocks.AddClonedChildrenFrom(base.Blocks, cloneContext);
			return footer;
		}
	}
}
