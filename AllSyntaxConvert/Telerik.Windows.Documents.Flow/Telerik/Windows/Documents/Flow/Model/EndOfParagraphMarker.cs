using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;

namespace Telerik.Windows.Documents.Flow.Model
{
	class EndOfParagraphMarker : InlineBase
	{
		public EndOfParagraphMarker(RadFlowDocument document)
			: base(document)
		{
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Run;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			return new EndOfParagraphMarker(cloneContext.Document);
		}
	}
}
