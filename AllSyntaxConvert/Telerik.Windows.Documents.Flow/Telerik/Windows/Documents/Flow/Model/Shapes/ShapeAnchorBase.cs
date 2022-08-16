using System;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public abstract class ShapeAnchorBase : InlineBase
	{
		internal ShapeAnchorBase(RadFlowDocument document)
			: base(document)
		{
			this.HorizontalPosition = new HorizontalPosition();
			this.VerticalPosition = new VerticalPosition();
			this.Wrapping = new ShapeWrapping();
			this.Margin = DocumentDefaultStyleSettings.FloatingImageMargin;
			this.LayoutInCell = true;
			this.AllowOverlap = true;
		}

		public bool AllowOverlap { get; set; }

		public bool IsLocked { get; set; }

		public ShapeWrapping Wrapping
		{
			get
			{
				return this.wrapping;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ShapeWrapping>(value, "wrapping");
				this.wrapping = value;
			}
		}

		public bool LayoutInCell { get; set; }

		public bool IsBehindDocument { get; set; }

		public int ZIndex { get; set; }

		public Padding Margin
		{
			get
			{
				return this.margin;
			}
			set
			{
				Guard.ThrowExceptionIfNull<Padding>(value, "margin");
				this.margin = value;
			}
		}

		public HorizontalPosition HorizontalPosition
		{
			get
			{
				return this.horizontalPosition;
			}
			set
			{
				Guard.ThrowExceptionIfNull<HorizontalPosition>(value, "horizontalPosition");
				this.horizontalPosition = value;
			}
		}

		public VerticalPosition VerticalPosition
		{
			get
			{
				return this.verticalPosition;
			}
			set
			{
				Guard.ThrowExceptionIfNull<VerticalPosition>(value, "verticalPosition");
				this.verticalPosition = value;
			}
		}

		internal abstract ShapeBase Shape { get; }

		protected void ClonePropertiesFrom(ShapeAnchorBase fromShapeAnchor)
		{
			this.AllowOverlap = fromShapeAnchor.AllowOverlap;
			this.IsLocked = fromShapeAnchor.IsLocked;
			this.Wrapping = fromShapeAnchor.Wrapping.Clone();
			this.LayoutInCell = fromShapeAnchor.LayoutInCell;
			this.IsBehindDocument = fromShapeAnchor.IsBehindDocument;
			this.ZIndex = fromShapeAnchor.ZIndex;
			this.Margin = fromShapeAnchor.Margin.Clone();
			this.HorizontalPosition = fromShapeAnchor.HorizontalPosition.Clone();
			this.VerticalPosition = fromShapeAnchor.VerticalPosition.Clone();
		}

		ShapeWrapping wrapping;

		Padding margin;

		HorizontalPosition horizontalPosition;

		VerticalPosition verticalPosition;
	}
}
