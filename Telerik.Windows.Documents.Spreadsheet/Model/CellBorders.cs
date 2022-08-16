using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellBorders
	{
		public CellBorder Left { get; set; }

		public CellBorder Top { get; set; }

		public CellBorder Right { get; set; }

		public CellBorder Bottom { get; set; }

		public CellBorder InsideHorizontal { get; set; }

		public CellBorder InsideVertical { get; set; }

		public CellBorder DiagonalUp { get; set; }

		public CellBorder DiagonalDown { get; set; }

		public CellBorders()
		{
		}

		public CellBorders(CellBorder all)
			: this(all, all, all, all, all, all, all, all)
		{
		}

		public CellBorders(CellBorder left, CellBorder top, CellBorder right, CellBorder bottom, CellBorder insideHorizontal, CellBorder insideVertical, CellBorder diagonalUp, CellBorder diagonalDown)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
			this.InsideHorizontal = insideHorizontal;
			this.InsideVertical = insideVertical;
			this.DiagonalUp = diagonalUp;
			this.DiagonalDown = diagonalDown;
		}

		public static bool operator ==(CellBorders first, CellBorders second)
		{
			return object.ReferenceEquals(first, second) || (first != null && first.Equals(second));
		}

		public static bool operator !=(CellBorders first, CellBorders second)
		{
			return !(first == second);
		}

		internal void MergeWith(CellBorders other)
		{
			Func<CellBorder, CellBorder, CellBorder> func = delegate(CellBorder firstBorder, CellBorder secondBorder)
			{
				if (!(firstBorder == secondBorder))
				{
					return null;
				}
				return firstBorder;
			};
			this.Left = func(this.Left, other.Left);
			this.Top = func(this.Top, other.Top);
			this.Right = func(this.Right, other.Right);
			this.Bottom = func(this.Bottom, other.Bottom);
			this.InsideHorizontal = func(this.InsideHorizontal, other.InsideHorizontal);
			this.InsideVertical = func(this.InsideVertical, other.InsideVertical);
			this.DiagonalUp = func(this.DiagonalUp, other.DiagonalUp);
			this.DiagonalDown = func(this.DiagonalDown, other.DiagonalDown);
		}

		public static CellBorders CreateOutline(CellBorder all)
		{
			return CellBorders.CreateOutline(all, all, all, all);
		}

		public static CellBorders CreateOutline(CellBorder left, CellBorder top, CellBorder right, CellBorder bottom)
		{
			return new CellBorders
			{
				Left = left,
				Top = top,
				Right = right,
				Bottom = bottom
			};
		}

		public static CellBorders CreateInside(CellBorder all)
		{
			return CellBorders.CreateInside(all, all);
		}

		public static CellBorders CreateInside(CellBorder insideHorizontal, CellBorder insideVertical)
		{
			return new CellBorders
			{
				InsideHorizontal = insideHorizontal,
				InsideVertical = insideVertical
			};
		}

		public override bool Equals(object obj)
		{
			CellBorders cellBorders = obj as CellBorders;
			return !(cellBorders == null) && (TelerikHelper.EqualsOfT<CellBorder>(this.Left, cellBorders.Left) && TelerikHelper.EqualsOfT<CellBorder>(this.Top, cellBorders.Top) && TelerikHelper.EqualsOfT<CellBorder>(this.Right, cellBorders.Right) && TelerikHelper.EqualsOfT<CellBorder>(this.Bottom, cellBorders.Bottom) && TelerikHelper.EqualsOfT<CellBorder>(this.InsideHorizontal, cellBorders.InsideHorizontal) && TelerikHelper.EqualsOfT<CellBorder>(this.InsideVertical, cellBorders.InsideVertical) && TelerikHelper.EqualsOfT<CellBorder>(this.DiagonalUp, cellBorders.DiagonalUp)) && TelerikHelper.EqualsOfT<CellBorder>(this.DiagonalDown, cellBorders.DiagonalDown);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Left.GetHashCodeOrZero(), this.Top.GetHashCodeOrZero(), this.Right.GetHashCodeOrZero(), this.Bottom.GetHashCodeOrZero(), this.InsideHorizontal.GetHashCodeOrZero(), this.InsideVertical.GetHashCodeOrZero(), this.DiagonalUp.GetHashCodeOrZero(), this.DiagonalDown.GetHashCodeOrZero());
		}

		public static readonly CellBorders Default = new CellBorders(CellBorder.Default);
	}
}
