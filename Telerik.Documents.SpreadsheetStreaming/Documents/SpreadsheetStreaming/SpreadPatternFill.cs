using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadPatternFill : ISpreadFill
	{
		public SpreadPatternFill(SpreadPatternType patternType)
			: this(patternType, null, null)
		{
		}

		public SpreadPatternFill(SpreadPatternType patternType, SpreadThemableColor patternColor, SpreadThemableColor backgroundColor)
		{
			this.PatternType = patternType;
			this.PatternColor = patternColor;
			this.BackgroundColor = backgroundColor;
		}

		public SpreadPatternType PatternType { get; set; }

		public SpreadThemableColor PatternColor { get; set; }

		public SpreadThemableColor BackgroundColor { get; set; }

		public static SpreadPatternFill CreateSolidFill(SpreadColor color)
		{
			Guard.ThrowExceptionIfNull<SpreadColor>(color, "color");
			return SpreadPatternFill.CreateSolidFill(new SpreadThemableColor(color));
		}

		public static SpreadPatternFill CreateSolidFill(SpreadThemableColor color)
		{
			Guard.ThrowExceptionIfNull<SpreadThemableColor>(color, "color");
			return new SpreadPatternFill(SpreadPatternType.Solid, new SpreadThemableColor(new SpreadColor(0, 0, 0)), color);
		}

		public override bool Equals(object obj)
		{
			SpreadPatternFill spreadPatternFill = obj as SpreadPatternFill;
			return spreadPatternFill != null && (ObjectExtensions.EqualsOfT<SpreadPatternType>(this.PatternType, spreadPatternFill.PatternType) && ObjectExtensions.EqualsOfT<SpreadThemableColor>(this.PatternColor, spreadPatternFill.PatternColor)) && ObjectExtensions.EqualsOfT<SpreadThemableColor>(this.BackgroundColor, spreadPatternFill.BackgroundColor);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.PatternType.GetHashCodeOrZero(), this.PatternColor.GetHashCodeOrZero(), this.BackgroundColor.GetHashCodeOrZero());
		}
	}
}
