using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetStrokeColorSpace : SetColorSpaceBase
	{
		public override string Name
		{
			get
			{
				return "CS";
			}
		}

		protected override void SetColorSpaceInternal(GraphicsState graphicsState, ColorSpaceObject colorSpace)
		{
			graphicsState.SetStrokeColorSpace(colorSpace);
		}
	}
}
