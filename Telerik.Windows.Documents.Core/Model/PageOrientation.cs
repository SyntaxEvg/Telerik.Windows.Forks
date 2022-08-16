using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Model
{
	public enum PageOrientation
	{
		Portrait,
		Landscape,
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Portrait instead.")]
		Rotate180,
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Landscape instead.")]
		Rotate270,
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Portrait instead.")]
		Rotate0 = 0,
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Landscape instead.")]
		Rotate90
	}
}
