using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public abstract class GeometryBase
	{
		internal GeometryBase()
		{
		}

		public Rect Bounds
		{
			get
			{
				return this.GetBounds();
			}
		}

		protected abstract Rect GetBounds();
	}
}
