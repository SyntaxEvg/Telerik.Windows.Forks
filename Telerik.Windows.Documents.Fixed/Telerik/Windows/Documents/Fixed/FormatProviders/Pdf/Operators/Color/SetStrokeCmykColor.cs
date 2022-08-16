using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetStrokeCmykColor : SetCmykColorBase
	{
		public override string Name
		{
			get
			{
				return "K";
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
