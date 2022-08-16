using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class GradientFill : IFill
	{
		public GradientType GradientType
		{
			get
			{
				return this.gradientType;
			}
		}

		public ThemableColor Color1
		{
			get
			{
				return this.color1;
			}
		}

		public ThemableColor Color2
		{
			get
			{
				return this.color2;
			}
		}

		public GradientFill(GradientType gradientType, Color color1, Color color2)
			: this(gradientType, new ThemableColor(color1), new ThemableColor(color2))
		{
		}

		public GradientFill(GradientType gradientType, ThemableColor color1, ThemableColor color2)
		{
			this.gradientType = gradientType;
			this.color1 = color1;
			this.color2 = color2;
		}

		public override bool Equals(object obj)
		{
			GradientFill gradientFill = obj as GradientFill;
			return gradientFill != null && (this.GradientType == gradientFill.GradientType && TelerikHelper.EqualsOfT<ThemableColor>(this.Color1, gradientFill.Color1)) && TelerikHelper.EqualsOfT<ThemableColor>(this.Color2, gradientFill.Color2);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.GradientType.GetHashCode(), this.Color1.GetHashCode(), this.Color2.GetHashCode());
		}

		readonly GradientType gradientType;

		readonly ThemableColor color1;

		readonly ThemableColor color2;
	}
}
