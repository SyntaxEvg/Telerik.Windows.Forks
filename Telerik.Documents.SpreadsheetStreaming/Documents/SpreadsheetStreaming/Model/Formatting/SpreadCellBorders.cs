using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Formatting
{
	class SpreadCellBorders
	{
		public SpreadBorder Left { get; set; }

		public SpreadBorder Right { get; set; }

		public SpreadBorder Top { get; set; }

		public SpreadBorder Bottom { get; set; }

		public SpreadBorder DiagonalUp { get; set; }

		public SpreadBorder DiagonalDown { get; set; }

		public override bool Equals(object obj)
		{
			SpreadCellBorders spreadCellBorders = obj as SpreadCellBorders;
			return spreadCellBorders != null && (ObjectExtensions.EqualsOfT<SpreadBorder>(this.Left, spreadCellBorders.Left) && ObjectExtensions.EqualsOfT<SpreadBorder>(this.Right, spreadCellBorders.Right) && ObjectExtensions.EqualsOfT<SpreadBorder>(this.Top, spreadCellBorders.Top) && ObjectExtensions.EqualsOfT<SpreadBorder>(this.Bottom, spreadCellBorders.Bottom) && ObjectExtensions.EqualsOfT<SpreadBorder>(this.DiagonalUp, spreadCellBorders.DiagonalUp)) && ObjectExtensions.EqualsOfT<SpreadBorder>(this.DiagonalDown, spreadCellBorders.DiagonalDown);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Left.GetHashCodeOrZero(), this.Right.GetHashCodeOrZero(), this.Top.GetHashCodeOrZero(), this.Bottom.GetHashCodeOrZero(), this.DiagonalUp.GetHashCodeOrZero(), this.DiagonalDown.GetHashCodeOrZero());
		}
	}
}
