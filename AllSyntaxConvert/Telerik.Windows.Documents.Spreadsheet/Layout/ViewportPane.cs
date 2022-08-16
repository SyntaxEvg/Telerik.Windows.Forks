using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	public class ViewportPane
	{
		public Rect Rect
		{
			get
			{
				return this.rect;
			}
			internal set
			{
				if (this.rect != value)
				{
					this.rect = value;
				}
			}
		}

		public Rect BoundingRect { get; internal set; }

		public CellRange VisibleRange
		{
			get
			{
				if (this.visibleRange == null)
				{
					this.visibleRange = CellRange.Empty;
				}
				return this.visibleRange;
			}
			internal set
			{
				if (this.visibleRange != value)
				{
					this.visibleRange = value;
				}
			}
		}

		public ViewportPaneType ViewportPaneType
		{
			get
			{
				return this.viewportPaneType;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.rect.Width == 0.0 || this.rect.Height == 0.0;
			}
		}

		internal Point TopLeftPoint { get; set; }

		public ViewportPane(ViewportPaneType viewportPaneType)
		{
			this.viewportPaneType = viewportPaneType;
		}

		public ViewportPane(Rect rect, ViewportPaneType viewportPaneType)
			: this(viewportPaneType)
		{
			this.rect = rect;
		}

		internal void SetX(double newX)
		{
			this.rect = new Rect(newX, this.rect.Y, this.rect.Width, this.rect.Height);
		}

		internal void SetY(double newY)
		{
			this.rect = new Rect(this.rect.X, newY, this.rect.Width, this.rect.Height);
		}

		public override bool Equals(object obj)
		{
			ViewportPane viewportPane = obj as ViewportPane;
			return viewportPane != null && (this.ViewportPaneType.Equals(viewportPane.ViewportPaneType) && this.Rect.Equals(viewportPane.Rect)) && this.VisibleRange.Equals(viewportPane.VisibleRange);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Rect.GetHashCodeOrZero(), this.ViewportPaneType.GetHashCodeOrZero());
		}

		Rect rect;

		CellRange visibleRange;

		readonly ViewportPaneType viewportPaneType;
	}
}
