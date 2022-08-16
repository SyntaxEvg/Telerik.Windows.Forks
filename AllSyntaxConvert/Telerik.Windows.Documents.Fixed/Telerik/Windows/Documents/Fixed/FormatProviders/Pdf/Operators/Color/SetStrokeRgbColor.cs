using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetStrokeRgbColor : SetRgbColorBase
	{
		public override string Name
		{
			get
			{
				return "RG";
			}
		}

		protected override bool IsStrokeColorSetter
		{
			get
			{
				return true;
			}
		}
	}
}
