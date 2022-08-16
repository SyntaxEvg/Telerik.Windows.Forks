using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetFillColorSpace : SetColorSpaceBase
	{
		public override string Name
		{
			get
			{
				return "cs";
			}
		}

		protected override void SetColorSpaceInternal(GraphicsState graphicsState, ColorSpaceObject colorSpace)
		{
			graphicsState.SetFillColorSpace(colorSpace);
		}
	}
}
