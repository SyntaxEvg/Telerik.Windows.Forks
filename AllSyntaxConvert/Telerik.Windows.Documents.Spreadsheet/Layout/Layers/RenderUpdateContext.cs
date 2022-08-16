using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class RenderUpdateContext
	{
		public RenderUpdateContext(SheetViewport sheetViewport, Size scaleFactor)
		{
			this.sheetViewport = sheetViewport;
			this.scaleFactor = scaleFactor;
		}

		public SheetViewport SheetViewport
		{
			get
			{
				return this.sheetViewport;
			}
		}

		public Size ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
		}

		public ScaleTransform ScaleTransform
		{
			get
			{
				if (this.scaleTransform == null)
				{
					this.scaleTransform = new ScaleTransform
					{
						ScaleX = this.ScaleFactor.Width,
						ScaleY = this.ScaleFactor.Height
					};
				}
				return this.scaleTransform;
			}
		}

		public Point Translate(Point point, ViewportPaneType viewportPaneType)
		{
			return this.SheetViewport.Translate(point, viewportPaneType);
		}

		public Rect Translate(Rect rect, ViewportPaneType viewportPaneType)
		{
			Point point = new Point(rect.Right, rect.Bottom);
			Point point2 = this.Translate(point, viewportPaneType);
			Point point3 = new Point(point2.X - rect.Width, point2.Y - rect.Height);
			return new Rect(point3, point2);
		}

		public Rect TranslateAndScale(Rect rect, ViewportPaneType viewportPaneType)
		{
			Rect rect2 = this.Translate(rect, viewportPaneType);
			ScaleTransform scaleTransform = new ScaleTransform
			{
				ScaleX = this.ScaleFactor.Width,
				ScaleY = this.ScaleFactor.Height
			};
			Point point = scaleTransform.Transform(new Point(rect2.Left, rect2.Top));
			Point point2 = scaleTransform.Transform(new Point(rect2.Width, rect2.Height));
			return new Rect(point.X, point.Y, point2.X, point2.Y);
		}

		readonly SheetViewport sheetViewport;

		readonly Size scaleFactor;

		ScaleTransform scaleTransform;
	}
}
