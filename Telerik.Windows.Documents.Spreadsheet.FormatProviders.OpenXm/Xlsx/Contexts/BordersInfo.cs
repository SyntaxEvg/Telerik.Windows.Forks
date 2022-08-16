using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	struct BordersInfo
	{
		public BordersInfo(CellStyle cellStyle)
		{
			this = default(BordersInfo);
			this.Left = cellStyle.LeftBorder;
			this.Top = cellStyle.TopBorder;
			this.Right = cellStyle.RightBorder;
			this.Bottom = cellStyle.BottomBorder;
			this.DiagonalUp = cellStyle.DiagonalUpBorder;
			this.DiagonalDown = cellStyle.DiagonalDownBorder;
		}

		public CellBorder Left { get; set; }

		public CellBorder Top { get; set; }

		public CellBorder Right { get; set; }

		public CellBorder Bottom { get; set; }

		public CellBorder DiagonalUp { get; set; }

		public CellBorder DiagonalDown { get; set; }

		public BordersInfo MergeWith(BordersInfo other)
		{
			if (this.Left == null)
			{
				this.Left = other.Left;
			}
			if (this.Top == null)
			{
				this.Top = other.Top;
			}
			if (this.Right == null)
			{
				this.Right = other.Right;
			}
			if (this.Bottom == null)
			{
				this.Bottom = other.Bottom;
			}
			if (this.DiagonalUp == null)
			{
				this.DiagonalUp = other.DiagonalUp;
			}
			if (this.DiagonalDown == null)
			{
				this.DiagonalDown = other.DiagonalDown;
			}
			return this;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is BordersInfo))
			{
				return false;
			}
			BordersInfo bordersInfo = (BordersInfo)obj;
			return TelerikHelper.EqualsOfT<CellBorder>(this.Left, bordersInfo.Left) && TelerikHelper.EqualsOfT<CellBorder>(this.Top, bordersInfo.Top) && TelerikHelper.EqualsOfT<CellBorder>(this.Right, bordersInfo.Right) && TelerikHelper.EqualsOfT<CellBorder>(this.Bottom, bordersInfo.Bottom) && TelerikHelper.EqualsOfT<CellBorder>(this.DiagonalUp, bordersInfo.DiagonalUp) && TelerikHelper.EqualsOfT<CellBorder>(this.DiagonalDown, bordersInfo.DiagonalDown);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Left.GetHashCodeOrZero(), this.Top.GetHashCodeOrZero(), this.Right.GetHashCodeOrZero(), this.Bottom.GetHashCodeOrZero(), this.DiagonalUp.GetHashCodeOrZero(), this.DiagonalDown.GetHashCodeOrZero());
		}

		public override string ToString()
		{
			return string.Format("Left: {0};Top: {1}; Right: {2}; Bottom: {3}; DiagonalUp: {4}; DiagonalDown: {5}", new object[] { this.Left, this.Top, this.Right, this.Bottom, this.DiagonalUp, this.DiagonalDown });
		}

		internal static BordersInfo GetDefaultValue()
		{
			return new BordersInfo
			{
				Bottom = CellBorder.Default,
				DiagonalDown = CellBorder.Default,
				DiagonalUp = CellBorder.Default,
				Left = CellBorder.Default,
				Right = CellBorder.Default,
				Top = CellBorder.Default
			};
		}
	}
}
