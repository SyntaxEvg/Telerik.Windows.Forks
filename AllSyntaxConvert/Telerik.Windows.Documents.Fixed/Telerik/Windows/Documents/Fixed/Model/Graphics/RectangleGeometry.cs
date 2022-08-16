using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class RectangleGeometry : GeometryBase
	{
		public RectangleGeometry()
		{
		}

		public RectangleGeometry(Rect rect)
		{
			this.Rect = rect;
		}

		public Rect Rect { get; set; }

		protected override Rect GetBounds()
		{
			return this.Rect;
		}
	}
}
