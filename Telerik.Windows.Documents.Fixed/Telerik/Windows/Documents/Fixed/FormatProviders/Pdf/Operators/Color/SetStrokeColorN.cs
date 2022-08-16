using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetStrokeColorN : SetColorNBase
	{
		public override string Name
		{
			get
			{
				return "SCN";
			}
		}

		protected override bool TryGetColorSpace(ContentStreamInterpreter interpreter, out ColorSpaceObject colorSpace)
		{
			return interpreter.GraphicState.TryGetStrokeColorSpace(out colorSpace);
		}

		protected override void SetColor(ContentStreamInterpreter interpreter, ColorObjectBase color)
		{
			interpreter.GraphicState.StrokeColor = color;
		}
	}
}
