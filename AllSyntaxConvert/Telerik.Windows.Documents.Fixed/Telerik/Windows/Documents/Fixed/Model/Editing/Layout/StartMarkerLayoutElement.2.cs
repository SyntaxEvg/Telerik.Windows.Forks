using System;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	abstract class StartMarkerLayoutElement<TStart> : StartMarkerLayoutElement where TStart : StartMarkerLayoutElement
	{
		public StartMarkerLayoutElement(FontBase font, double fontSize)
			: base(font, fontSize)
		{
		}
	}
}
