using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Fixed.Model.Internal.Collections;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class RadFixedPageInternal
	{
		internal RadFixedPageInternal(RadFixedDocumentInternal document, RadFixedPage publicPage)
		{
			this.document = document;
			this.PublicPage = publicPage;
		}

		public RadFixedPage PublicPage { get; set; }

		public bool HasContent
		{
			get
			{
				return this.Content != null;
			}
		}

		public RadFixedDocumentInternal Document
		{
			get
			{
				return this.document;
			}
		}

		internal ContentCollection Content { get; set; }

		internal AnnotationCollection Annotations
		{
			get
			{
				return this.PublicPage.Annotations;
			}
		}

		internal Rect CropBox { get; set; }

		internal Matrix TransformMatrix { get; set; }

		internal Rect BoundingRect { get; set; }

		internal void Arrange()
		{
			if (this.Content != null)
			{
				foreach (IContentElement contentElement in this.Content)
				{
					Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement contentElement2 = (Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement)contentElement;
					contentElement2.Arrange(this.TransformMatrix);
				}
			}
			if (this.Annotations != null)
			{
				foreach (Annotation annotation in this.Annotations)
				{
					annotation.Arrange(this.TransformMatrix);
				}
			}
		}

		internal double GetXInDip(double pdfXCoordinate)
		{
			return this.TransformMatrix.Transform(new Point(pdfXCoordinate, 0.0)).X;
		}

		internal double GetYInDip(double pdfYCoordinate)
		{
			return this.TransformMatrix.Transform(new Point(0.0, pdfYCoordinate)).Y;
		}

		readonly RadFixedDocumentInternal document;
	}
}
