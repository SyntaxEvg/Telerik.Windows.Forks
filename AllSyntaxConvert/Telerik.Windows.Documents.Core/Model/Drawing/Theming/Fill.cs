using System;

namespace Telerik.Windows.Documents.Model.Drawing.Theming
{
	public abstract class Fill
	{
		internal abstract ShapeType ShapeFillType { get; }

		public abstract Fill Clone();
	}
}
