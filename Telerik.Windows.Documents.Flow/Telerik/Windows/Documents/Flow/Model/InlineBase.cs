using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Flow.Model
{
	public abstract class InlineBase : DocumentElementBase
	{
		internal InlineBase(RadFlowDocument document)
			: base(document)
		{
		}

		public Paragraph Paragraph
		{
			get
			{
				return (Paragraph)base.Parent;
			}
		}

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return Enumerable.Empty<DocumentElementBase>();
			}
		}
	}
}
