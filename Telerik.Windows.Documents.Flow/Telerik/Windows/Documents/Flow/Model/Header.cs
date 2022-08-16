using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Header : HeaderFooterBase
	{
		internal Header(RadFlowDocument document, Section section)
			: base(document, section)
		{
			this.watermarkCollection = new WatermarkCollection(document);
		}

		public WatermarkCollection Watermarks
		{
			get
			{
				return this.watermarkCollection;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Header;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Header header = new Header(cloneContext.Document, cloneContext.CurrentSection);
			header.Blocks.AddClonedChildrenFrom(base.Blocks, cloneContext);
			header.Watermarks.AddClonedChildrenFrom(this.Watermarks);
			return header;
		}

		readonly WatermarkCollection watermarkCollection;
	}
}
