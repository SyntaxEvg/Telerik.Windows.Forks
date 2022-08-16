using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class Tiling : TilingBase
	{
		public Tiling()
			: this(FixedDocumentDefaults.TilingBoundingBox, SimplePosition.Default)
		{
		}

		public Tiling(Rect boundingBox)
			: this(boundingBox, SimplePosition.Default)
		{
		}

		public Tiling(IPosition position)
			: this(FixedDocumentDefaults.TilingBoundingBox, position)
		{
		}

		public Tiling(Rect boundingBox, IPosition position)
		{
			this.TilingType = TilingType.NoDistortion;
			this.content = new ContentElementCollection(this);
			this.Position = position;
			this.BoundingBox = boundingBox;
		}

		public override Size Size
		{
			get
			{
				return new Size(this.BoundingBox.Width, this.BoundingBox.Height);
			}
		}

		public override ContentElementCollection Content
		{
			get
			{
				return this.content;
			}
		}

		public override double VerticalSpacing
		{
			get
			{
				if (this.verticalSpacing != null)
				{
					return this.verticalSpacing.Value;
				}
				return this.Size.Height;
			}
			set
			{
				this.verticalSpacing = new double?(value);
			}
		}

		public override double HorizontalSpacing
		{
			get
			{
				if (this.horizontalSpacing != null)
				{
					return this.horizontalSpacing.Value;
				}
				return this.Size.Width;
			}
			set
			{
				this.horizontalSpacing = new double?(value);
			}
		}

		public override Rect BoundingBox
		{
			get
			{
				return this.boundingBox;
			}
			set
			{
				this.boundingBox = value;
			}
		}

		public override IPosition Position { get; set; }

		public override TilingType TilingType { get; set; }

		public override bool Equals(ColorBase other)
		{
			return this == other;
		}

		internal override PaintType PaintType
		{
			get
			{
				return PaintType.Colored;
			}
		}

		readonly ContentElementCollection content;

		Rect boundingBox;

		double? verticalSpacing;

		double? horizontalSpacing;
	}
}
