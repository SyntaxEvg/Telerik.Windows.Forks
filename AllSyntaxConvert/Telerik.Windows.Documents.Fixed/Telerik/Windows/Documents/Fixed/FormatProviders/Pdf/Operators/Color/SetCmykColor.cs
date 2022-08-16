using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetCmykColor : SetCmykColorBase
	{
		public override string Name
		{
			get
			{
				return "k";
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
