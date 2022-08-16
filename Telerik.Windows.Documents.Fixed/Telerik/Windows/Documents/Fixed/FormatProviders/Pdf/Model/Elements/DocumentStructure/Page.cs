using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class Page : PageTreeNode
	{
		public Page()
		{
			this.contentsProperty = base.RegisterReferenceProperty<ContentStream>(new PdfPropertyDescriptor("Contents"));
			this.annotsProperty = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Annots"));
		}

		public override PageTreeNodeType PageTreeNodeType
		{
			get
			{
				return PageTreeNodeType.Page;
			}
		}

		public ContentStream Contents
		{
			get
			{
				return this.contentsProperty.GetValue();
			}
			set
			{
				this.contentsProperty.SetValue(value);
			}
		}

		public PdfArray Annots
		{
			get
			{
				return this.annotsProperty.GetValue();
			}
			set
			{
				this.annotsProperty.SetValue(value);
			}
		}

		public Rotation Rotation
		{
			get
			{
				return Page.GetRotation(base.Rotate);
			}
		}

		public static Rotation GetRotation(PdfInt rotate)
		{
			int value = rotate.Value;
			return Page.GetRotation(value);
		}

		public static Rotation GetRotation(int value)
		{
			return (Rotation)(value / 90 % 4 * 90);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			base.Resources = new PdfResource();
			base.MediaBox = fixedPage.ToBottomLeftCoordinateSystem(fixedPage.MediaBox);
			base.CropBox = fixedPage.ToBottomLeftCoordinateSystem(fixedPage.CropBox);
			this.Contents = new ContentStream();
			this.Contents.CopyPropertiesFrom(context, this, fixedPage);
			if (fixedPage.Annotations != null && fixedPage.Annotations.Count > 0)
			{
				this.Annots = new PdfArray(new PdfPrimitive[0]);
				foreach (Annotation annotation in fixedPage.Annotations)
				{
					AnnotationObject primitive = AnnotationFactory.CreatePdfAnnotation(context, annotation, fixedPage);
					IndirectObject indirectObject = context.CreateIndirectObject(primitive);
					this.Annots.Add(indirectObject.Reference);
				}
			}
			base.Rotate = Page.GetRotate(fixedPage.Rotation);
		}

		public void CopyPropertiesTo(PostScriptReader reader, IRadFixedDocumentImportContext context, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			fixedPage.MediaBox = this.ConvertToTopLeftBox(base.MediaBox, reader, context);
			fixedPage.CropBox = this.ConvertToTopLeftBox(base.CropBox, reader, context);
			fixedPage.Rotation = this.Rotation;
		}

		internal void CopyPageContentTo(IRadFixedDocumentImportContext context, RadFixedPage fixedPage)
		{
			if (this.Contents != null)
			{
				this.Contents.CopyPropertiesTo(context, this, fixedPage);
			}
		}

		internal void CopyPageAnnotationsTo(IRadFixedDocumentImportContext context, RadFixedPage fixedPage)
		{
			if (this.Annots != null)
			{
				for (int i = 0; i < this.Annots.Count; i++)
				{
					AnnotationObject annotationObject;
					this.Annots.TryGetElement<AnnotationObject>(context.Reader, context, i, out annotationObject);
					if (annotationObject.IsSupported)
					{
						Annotation annotation = annotationObject.ToAnnotation(context.Reader, context, fixedPage.Size.Height);
						if (annotation != null)
						{
							fixedPage.Annotations.Add(annotation);
						}
					}
				}
			}
		}

		internal static PdfInt GetRotate(Rotation rotation)
		{
			return ((int)rotation).ToPdfInt();
		}

		internal Rect ConvertToTopLeftBox(PdfArray pdfArray, PostScriptReader reader, IPdfImportContext context)
		{
			double height = base.MediaBox.ToRect(reader, context).Height;
			Rect rect = pdfArray.ToRect(reader, context);
			return rect.ToTopLeftCoordinateSystem(height);
		}

		public const string ContentsName = "Contents";

		public const string AnnotsName = "Annots";

		const int RotationMultiplier = 90;

		readonly ReferenceProperty<ContentStream> contentsProperty;

		readonly DirectProperty<PdfArray> annotsProperty;
	}
}
