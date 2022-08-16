using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetStrokeGrayColor : SetGrayColorBase
	{
		public override string Name
		{
			get
			{
				return "G";
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
