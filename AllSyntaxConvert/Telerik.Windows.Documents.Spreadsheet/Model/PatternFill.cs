using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class PatternFill : IFill
	{
		public PatternType PatternType
		{
			get
			{
				return this.patternType;
			}
		}

		public ThemableColor PatternColor
		{
			get
			{
				return this.patternColor;
			}
		}

		public ThemableColor BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
		}

		public PatternFill(PatternType patternType, Color patternColor, Color backgroundColor)
			: this(patternType, new ThemableColor(patternColor), new ThemableColor(backgroundColor))
		{
		}

		public PatternFill(PatternType patternType, ThemableColor patternColor, ThemableColor backgroundColor)
		{
			this.patternType = patternType;
			this.patternColor = patternColor;
			this.backgroundColor = backgroundColor;
		}

		public static IFill CreateSolidFill(Color color)
		{
			return new PatternFill(PatternType.Solid, color, Colors.Black);
		}

		public static IFill CreateSolidFill(ThemableColor color)
		{
			return new PatternFill(PatternType.Solid, color, new ThemableColor(Colors.Black));
		}

		internal ThemableColor GetBackgroundDependingOnPatternType()
		{
			if (this.PatternType != PatternType.Solid)
			{
				return this.BackgroundColor;
			}
			return this.PatternColor;
		}

		public override bool Equals(object obj)
		{
			PatternFill patternFill = obj as PatternFill;
			return patternFill != null && (this.patternType == patternFill.patternType && TelerikHelper.EqualsOfT<ThemableColor>(this.patternColor, patternFill.patternColor)) && TelerikHelper.EqualsOfT<ThemableColor>(this.backgroundColor, patternFill.backgroundColor);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.patternType.GetHashCode(), this.patternColor.GetHashCodeOrZero(), this.backgroundColor.GetHashCodeOrZero());
		}

		public override string ToString()
		{
			return string.Format("pt:{0} fg:{1} bg:{2}", this.PatternType, this.PatternColor, this.BackgroundColor);
		}

		public static readonly PatternFill Default = (PatternFill)PatternFill.CreateSolidFill(Colors.Transparent);

		readonly PatternType patternType;

		readonly ThemableColor patternColor;

		readonly ThemableColor backgroundColor;
	}
}
