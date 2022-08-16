using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Fixed.Model.Internal.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class Container : Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement
	{
		public Container()
		{
			this.Content = new ContentCollection();
		}

		public override ContentElementTypeOld Type
		{
			get
			{
				return ContentElementTypeOld.Container;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return true;
			}
		}

		public ContentCollection Content { get; set; }

		Rect CalculateContentsBoundingRect()
		{
			if (this.Content.Count == 0)
			{
				return Rect.Empty;
			}
			Rect boundingRect = this.Content[0].BoundingRect;
			for (int i = 1; i < this.Content.Count; i++)
			{
				boundingRect.Union(this.Content[i].BoundingRect);
			}
			return boundingRect;
		}

		public override IContentElement Clone()
		{
			Container container = new Container();
			container.TransformMatrix = base.TransformMatrix;
			container.ZIndex = base.ZIndex;
			container.Size = base.Size;
			foreach (IContentElement contentElement in this.Content)
			{
				Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement contentElement2 = (Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement)contentElement;
				container.Content.AddChild(contentElement2.Clone());
			}
			if (base.Clip != null)
			{
				container.Clip = base.Clip.Clone();
			}
			return container;
		}

		public override Rect Arrange(Matrix transformMatrix)
		{
			foreach (IContentElement contentElement in this.Content)
			{
				Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement contentElement2 = (Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement)contentElement;
				contentElement2.Arrange(base.TransformMatrix * transformMatrix);
			}
			base.BoundingRect = ((base.Clip == null) ? this.CalculateContentsBoundingRect() : Helper.GetBoundingRect(base.Clip.GetBoundingRect(), base.TransformMatrix * base.Clip.TransformMatrix * transformMatrix));
			return base.BoundingRect;
		}

		public override IEnumerable<IContentElement> Children
		{
			get
			{
				return this.Content;
			}
		}
	}
}
