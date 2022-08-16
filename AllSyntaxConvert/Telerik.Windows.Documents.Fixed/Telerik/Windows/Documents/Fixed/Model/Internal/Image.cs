using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class Image : Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement
	{
		public override ContentElementTypeOld Type
		{
			get
			{
				return ContentElementTypeOld.Image;
			}
		}

		public ImageResourceKey ImageSourceKey { get; set; }

		public override Rect Arrange(Matrix transformMatrix)
		{
			base.BoundingRect = Helper.GetBoundingRect(new Rect(new Point(0.0, 0.0), new Size(1.0, 1.0)), base.TransformMatrix * transformMatrix);
			return base.BoundingRect;
		}

		public override IContentElement Clone()
		{
			return new Image
			{
				ImageSourceKey = this.ImageSourceKey,
				Size = base.Size,
				TransformMatrix = base.TransformMatrix,
				ZIndex = base.ZIndex
			};
		}
	}
}
