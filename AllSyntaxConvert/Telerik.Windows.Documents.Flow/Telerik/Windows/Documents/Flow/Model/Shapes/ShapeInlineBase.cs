using System;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public abstract class ShapeInlineBase : InlineBase
	{
		internal ShapeInlineBase(RadFlowDocument document)
			: base(document)
		{
		}

		internal abstract ShapeBase Shape { get; }
	}
}
