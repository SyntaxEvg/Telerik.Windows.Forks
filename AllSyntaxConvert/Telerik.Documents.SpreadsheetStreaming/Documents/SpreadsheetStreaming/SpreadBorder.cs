using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public class SpreadBorder
	{
		public SpreadBorder(SpreadBorderStyle style, SpreadThemableColor color)
		{
			Guard.ThrowExceptionIfNull<SpreadThemableColor>(color, "color");
			this.Style = style;
			this.Color = color;
		}

		internal SpreadBorder()
		{
		}

		public SpreadBorderStyle Style { get; internal set; }

		public SpreadThemableColor Color { get; internal set; }

		public override bool Equals(object obj)
		{
			SpreadBorder spreadBorder = obj as SpreadBorder;
			return spreadBorder != null && ObjectExtensions.EqualsOfT<SpreadBorderStyle>(this.Style, spreadBorder.Style) && ObjectExtensions.EqualsOfT<SpreadThemableColor>(this.Color, spreadBorder.Color);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Style.GetHashCodeOrZero(), this.Color.GetHashCodeOrZero());
		}
	}
}
