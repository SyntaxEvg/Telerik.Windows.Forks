using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	static class IconScaleConditions
	{
		public const string AlwaysScale = "A";

		public const string ScaleIfBiggerThanAnnotationRectangle = "B";

		public const string ScaleIfSmallerThanAnnotationRectangle = "S";

		public const string NeverScale = "N";
	}
}
