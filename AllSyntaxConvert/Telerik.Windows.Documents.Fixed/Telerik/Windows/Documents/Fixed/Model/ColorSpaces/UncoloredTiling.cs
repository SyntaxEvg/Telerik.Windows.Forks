using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class UncoloredTiling : TilingBase
	{
		public UncoloredTiling(Tiling tiling, SimpleColor color)
		{
			Guard.ThrowExceptionIfNull<Tiling>(tiling, "tiling");
			Guard.ThrowExceptionIfNull<SimpleColor>(color, "color");
			this.tiling = tiling;
			this.color = color;
		}

		public Tiling Tiling
		{
			get
			{
				return this.tiling;
			}
		}

		public SimpleColor Color
		{
			get
			{
				return this.color;
			}
		}

		public override ContentElementCollection Content
		{
			get
			{
				return this.Tiling.Content;
			}
		}

		public override Size Size
		{
			get
			{
				return this.Tiling.Size;
			}
		}

		public override Rect BoundingBox
		{
			get
			{
				return this.Tiling.BoundingBox;
			}
			set
			{
				this.Tiling.BoundingBox = value;
			}
		}

		public override IPosition Position
		{
			get
			{
				return this.Tiling.Position;
			}
			set
			{
				this.Tiling.Position = value;
			}
		}

		public override double HorizontalSpacing
		{
			get
			{
				return this.Tiling.HorizontalSpacing;
			}
			set
			{
				this.Tiling.HorizontalSpacing = value;
			}
		}

		public override double VerticalSpacing
		{
			get
			{
				return this.Tiling.VerticalSpacing;
			}
			set
			{
				this.Tiling.VerticalSpacing = value;
			}
		}

		public override TilingType TilingType
		{
			get
			{
				return this.Tiling.TilingType;
			}
			set
			{
				this.Tiling.TilingType = value;
			}
		}

		internal override PaintType PaintType
		{
			get
			{
				return PaintType.Uncolored;
			}
		}

		public override bool Equals(ColorBase other)
		{
			UncoloredTiling uncoloredTiling = other as UncoloredTiling;
			return uncoloredTiling != null && this.Color == uncoloredTiling.Color && this.Tiling == uncoloredTiling.Tiling;
		}

		readonly Tiling tiling;

		readonly SimpleColor color;
	}
}
