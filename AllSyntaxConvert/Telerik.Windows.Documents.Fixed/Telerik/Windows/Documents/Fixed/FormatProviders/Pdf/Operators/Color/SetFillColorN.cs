using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetFillColorN : SetColorNBase
	{
		public override string Name
		{
			get
			{
				return "scn";
			}
		}

		protected override bool TryGetColorSpace(ContentStreamInterpreter interpreter, out ColorSpaceObject colorSpace)
		{
			return interpreter.GraphicState.TryGetFillColorSpace(out colorSpace);
		}

		protected override void SetColor(ContentStreamInterpreter intereter, ColorObjectBase color)
		{
			intereter.GraphicState.FillColor = color;
		}
	}
}
