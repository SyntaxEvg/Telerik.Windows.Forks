using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	class SetGrayColor : SetGrayColorBase
	{
		public override string Name
		{
			get
			{
				return "g";
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
