using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Shapes;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Classes
{
	abstract class ContentElement : IContentElement
	{
		public ContentElement()
		{
			this.TransformMatrix = Matrix.Identity;
		}

		public int ZIndex { get; set; }

		public PathGeometry Clip { get; set; }

		public virtual bool HasChildren
		{
			get
			{
				return false;
			}
		}

		public virtual IEnumerable<IContentElement> Children
		{
			get
			{
				return null;
			}
		}

		public abstract Rect Arrange(Matrix transformMatrix);

		public abstract IContentElement Clone();

		public Matrix TransformMatrix { get; set; }

		public Size Size { get; set; }

		public Rect BoundingRect { get; set; }

		public abstract ContentElementTypeOld Type { get; }
	}
}
