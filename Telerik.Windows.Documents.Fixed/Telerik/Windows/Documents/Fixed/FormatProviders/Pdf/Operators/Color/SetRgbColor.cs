using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetRgbColor : SetRgbColorBase
	{
		public override string Name
		{
			get
			{
				return "rg";
			}
		}

		protected override bool IsStrokeColorSetter
		{
			get
			{
				return false;
			}
		}
	}
}
