using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadGradientFill : ISpreadFill
	{
		public SpreadGradientFill(SpreadGradientType gradientType, SpreadThemableColor color1, SpreadThemableColor color2)
		{
			Guard.ThrowExceptionIfNull<SpreadThemableColor>(color1, "color1");
			Guard.ThrowExceptionIfNull<SpreadThemableColor>(color2, "color2");
			this.GradientType = gradientType;
			this.Color1 = color1;
			this.Color2 = color2;
		}

		public SpreadThemableColor Color1 { get; set; }

		public SpreadThemableColor Color2 { get; set; }

		public SpreadGradientType GradientType { get; set; }

		public override bool Equals(object obj)
		{
			SpreadGradientFill spreadGradientFill = obj as SpreadGradientFill;
			return spreadGradientFill != null && (ObjectExtensions.EqualsOfT<SpreadThemableColor>(this.Color1, spreadGradientFill.Color1) && ObjectExtensions.EqualsOfT<SpreadThemableColor>(this.Color2, spreadGradientFill.Color2)) && ObjectExtensions.EqualsOfT<SpreadGradientType>(this.GradientType, spreadGradientFill.GradientType);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Color1.GetHashCodeOrZero(), this.Color2.GetHashCodeOrZero(), this.GradientType.GetHashCodeOrZero());
		}
	}
}
