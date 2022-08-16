using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Actions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf
{
	static class PdfElementToFixedElementTranslator
	{
		public static void ToSLCoordinates(Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement element)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement>(element, "element");
			Image image = element as Image;
			if (image != null)
			{
				PdfElementToFixedElementTranslator.ImageToSLCoordinates(image);
				return;
			}
			Container container = element as Container;
			if (container != null)
			{
				PdfElementToFixedElementTranslator.ContainerToSLCoordinates(container);
			}
		}

		public static RadFixedPageInternal CreateFixedPage(RadFixedDocumentInternal doc, PageOld source)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocumentInternal>(doc, "doc");
			Guard.ThrowExceptionIfNull<PageOld>(source, "source");
			RadFixedPage radFixedPage = doc.PublicDocument.Pages.AddPage();
			RadFixedPageInternal radFixedPageInternal = new RadFixedPageInternal(doc, radFixedPage);
			radFixedPage.InternalRadFixedPage = radFixedPageInternal;
			radFixedPageInternal.CropBox = source.CropBox.ToRect();
			int value = ((source.Rotate == null) ? 0 : (source.Rotate.Value % 360));
			radFixedPage.Rotation = Page.GetRotation(value);
			radFixedPage.CropBox = new Rect(0.0, 0.0, radFixedPageInternal.CropBox.Width * PdfElementToFixedElementTranslator.PointToDip, radFixedPageInternal.CropBox.Height * PdfElementToFixedElementTranslator.PointToDip);
			radFixedPage.MediaBox = radFixedPage.CropBox;
			radFixedPageInternal.TransformMatrix = new Telerik.Windows.Documents.Core.Data.Matrix(PageLayoutHelper.CalculatePointToDipPageTransformation(radFixedPage));
			radFixedPageInternal.BoundingRect = PageLayoutHelper.CalculatePageBoundingRectangle(radFixedPage);
			return radFixedPageInternal;
		}

		public static Annotation CreateAnnotation(AnnotationOld source, PageOld page, PdfFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<AnnotationOld>(source, "source");
			Guard.ThrowExceptionIfNull<PageOld>(page, "page");
			Guard.ThrowExceptionIfNull<PdfFormatProvider>(provider, "provider");
			bool flag = true;
			Annotation annotation;
			switch (source.Type)
			{
			case AnnotationType.Link:
				annotation = PdfElementToFixedElementTranslator.CreateLinkAnnotation((LinkOld)source, provider);
				break;
			case AnnotationType.Widget:
			{
				Widget widget;
				flag = !provider.TryGetWidgetFromWidgetObject((WidgetOld)source, out widget);
				annotation = widget;
				break;
			}
			default:
				annotation = new Telerik.Windows.Documents.Fixed.Model.Annotations.UnsupportedAnnotation(source.Type);
				break;
			}
			if (flag && annotation != null)
			{
				PdfElementToFixedElementTranslator.ApplyCommonAnnotationProperties(source, page, annotation);
			}
			return annotation;
		}

		internal static void ApplyCommonAnnotationProperties(AnnotationOld source, PageOld page, Annotation res)
		{
			if (page != null)
			{
				res.Rect = PdfElementToFixedElementTranslator.CalculateAnnotationRect(source, page);
			}
			res.AnnotationOldRect = source.Rect.ToRect();
			AnnotationObject.ImportFlagsToAnnotation(res, source.Flags.Value);
			AnnotationAppearancesOld appearanceOld;
			if (source.TryGetAppearanceContent(out appearanceOld))
			{
				res.AppearanceOld = appearanceOld;
			}
		}

		internal static BookmarkItem CreateBookmarkItemHierarchy(OutlineItemOld outline, PdfFormatProvider provider)
		{
			BookmarkItem bookmarkItem = PdfElementToFixedElementTranslator.CreateBookmarkItem(outline, provider);
			for (OutlineItemOld outlineItemOld = outline.First; outlineItemOld != null; outlineItemOld = outlineItemOld.Next)
			{
				BookmarkItem item = PdfElementToFixedElementTranslator.CreateBookmarkItemHierarchy(outlineItemOld, provider);
				bookmarkItem.Children.Add(item);
			}
			return bookmarkItem;
		}

		internal static PageMode CreatePageMode(PdfNameOld pageModeName)
		{
			string text = ((pageModeName != null) ? pageModeName.Value : string.Empty);
			string a;
			PageMode result;
			if ((a = text) != null && a == "UseOutlines")
			{
				result = PageMode.UseBookmarks;
			}
			else
			{
				result = PageMode.UseNone;
			}
			return result;
		}

		static Rect CalculateAnnotationRect(AnnotationOld source, PageOld page)
		{
			Rect rect = source.Rect.ToRect();
			Rect rect2 = AnnotationObject.ConvertRect(rect, page.MediaBox.ToRect().Height);
			double x = Unit.PointToDip(rect2.X);
			double y = Unit.PointToDip(rect2.Y);
			double width = Unit.PointToDip(rect2.Width);
			double height = Unit.PointToDip(rect2.Height);
			return new Rect(x, y, width, height);
		}

		static void ImageToSLCoordinates(Image img)
		{
			Guard.ThrowExceptionIfNull<Image>(img, "img");
			Matrix transformMatrix = img.TransformMatrix;
			transformMatrix.Scale(1.0, 1.0, 0.0, 1.0);
			img.TransformMatrix = transformMatrix;
		}

		static void ContainerToSLCoordinates(Container container)
		{
			Guard.ThrowExceptionIfNull<Container>(container, "container");
			foreach (IContentElement contentElement in container.Content)
			{
				Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement element = (Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement)contentElement;
				PdfElementToFixedElementTranslator.ToSLCoordinates(element);
			}
		}

		static UriAction CreateUriAction(UriActionOld source)
		{
			Guard.ThrowExceptionIfNull<UriActionOld>(source, "source");
			UriAction uriAction = new UriAction();
			try
			{
				uriAction.Uri = new Uri(source.Uri.ToString(), UriKind.RelativeOrAbsolute);
				uriAction.IncludeMouseCoordinates = new bool?(source.IsMap != null && source.IsMap.Value);
			}
			catch
			{
			}
			return uriAction;
		}

		static GoToAction CreateGoToAction(GoToActionOld source, PdfFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<GoToActionOld>(source, "source");
			return new GoToAction
			{
				Destination = PdfElementToFixedElementTranslator.CreateDestination(source.Destination, provider)
			};
		}

		static Telerik.Windows.Documents.Fixed.Model.Actions.Action CreateAction(ActionOld source, PdfFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<ActionOld>(source, "source");
			UriActionOld uriActionOld = source as UriActionOld;
			if (uriActionOld != null)
			{
				return PdfElementToFixedElementTranslator.CreateUriAction(uriActionOld);
			}
			GoToActionOld goToActionOld = source as GoToActionOld;
			if (goToActionOld != null)
			{
				return PdfElementToFixedElementTranslator.CreateGoToAction(goToActionOld, provider);
			}
			return null;
		}

		static Destination CreateDestination(DestinationOld source, PdfFormatProvider provider)
		{
			Guard.ThrowExceptionIfNull<DestinationOld>(source, "source");
			DestinationType type = (DestinationType)source.Type;
			RadFixedPage radFixedPage = null;
			double? left = null;
			double? right = null;
			double? top = null;
			double? bottom = null;
			double? zoom = null;
			if (source.Page != null)
			{
				radFixedPage = provider.GetRadFixedPageFromPage(source.Page).PublicPage;
				left = ((source.Left != null) ? new double?(radFixedPage.InternalRadFixedPage.GetXInDip(source.Left.Value)) : null);
				right = ((source.Right != null) ? new double?(radFixedPage.InternalRadFixedPage.GetXInDip(source.Right.Value)) : null);
				top = ((source.Top != null) ? new double?(radFixedPage.InternalRadFixedPage.GetYInDip(source.Top.Value)) : null);
				bottom = ((source.Bottom != null) ? new double?(radFixedPage.InternalRadFixedPage.GetYInDip(source.Bottom.Value)) : null);
				zoom = ((source.Zoom != null && source.Zoom.Value > 0.0) ? new double?(source.Zoom.Value) : null);
			}
			switch (type)
			{
			case DestinationType.Location:
				return new Location
				{
					Page = radFixedPage,
					Left = left,
					Top = top,
					Zoom = zoom
				};
			case DestinationType.PageFit:
				return new PageFit
				{
					Page = radFixedPage
				};
			case DestinationType.PageHorizontalFit:
				return new PageHorizontalFit
				{
					Page = radFixedPage,
					Top = top
				};
			case DestinationType.PageVerticalFit:
				return new PageVerticalFit
				{
					Page = radFixedPage,
					Left = left
				};
			case DestinationType.RectangleFit:
				return new RectangleFit
				{
					Page = radFixedPage,
					Left = left,
					Right = right,
					Bottom = bottom,
					Top = top
				};
			case DestinationType.BoundingRectangleFit:
				return new BoundingRectangleFit
				{
					Page = radFixedPage
				};
			case DestinationType.BoundingRectangleHorizontalFit:
				return new BoundingRectangleHorizontalFit
				{
					Page = radFixedPage,
					Top = top
				};
			case DestinationType.BoundingRectangleVerticalFit:
				return new BoundingRectangleVerticalFit
				{
					Page = radFixedPage,
					Left = left
				};
			default:
				return Destination.CreateDestination(type, radFixedPage);
			}
		}

		static Link CreateLinkAnnotation(LinkOld source, PdfFormatProvider provider)
		{
			Link link = new Link();
			if (source.Action != null)
			{
				link.Action = PdfElementToFixedElementTranslator.CreateAction(source.Action, provider);
			}
			else if (source.Destination != null)
			{
				link.Destination = PdfElementToFixedElementTranslator.CreateDestination(source.Destination, provider);
			}
			return link;
		}

		internal static void InitializeSignatureField(SignatureField field, PdfContentManager contentManager, FormFieldNodeOld terminalFormField)
		{
			IndirectObjectOld indirectObjectOld;
			if (terminalFormField.FieldValue != null && contentManager.TryGetIndirectObject(terminalFormField.FieldValue.Reference, out indirectObjectOld))
			{
				PdfDictionaryOld pdfDictionaryOld = indirectObjectOld.Value as PdfDictionaryOld;
				if (pdfDictionaryOld != null)
				{
					SignatureElementOld signatureElementOld = new SignatureElementOld(contentManager);
					signatureElementOld.Load(indirectObjectOld);
					field.Signature = new Signature();
					signatureElementOld.CopyPropertiesTo(field.Signature);
				}
			}
		}

		static BookmarkItem CreateBookmarkItem(OutlineItemOld outline, PdfFormatProvider provider)
		{
			BookmarkItem bookmarkItem = new BookmarkItem();
			bookmarkItem.Title = ((outline.Title != null) ? outline.Title.ToString() : string.Empty);
			bookmarkItem.IsExpanded = outline.Count != null && outline.Count.Value > 0;
			bookmarkItem.TextStyle = (BookmarkItemStyle)((outline.Flag != null) ? outline.Flag.Value : 0);
			if (outline.Destination != null)
			{
				bookmarkItem.Destination = PdfElementToFixedElementTranslator.CreateDestination(outline.Destination, provider);
			}
			if (outline.Action != null)
			{
				bookmarkItem.Action = PdfElementToFixedElementTranslator.CreateAction(outline.Action, provider);
			}
			return bookmarkItem;
		}

		const string UseOutlines = "UseOutlines";

		public static double PointToDip = Unit.PointToDip(1.0);
	}
}
