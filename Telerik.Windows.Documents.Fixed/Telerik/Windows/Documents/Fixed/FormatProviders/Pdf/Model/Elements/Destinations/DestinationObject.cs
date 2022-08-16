using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Destinations
{
	class DestinationObject : PdfArrayObject
	{
		public DestinationObject()
		{
			this.page = base.RegisterReferenceProperty<Page>(new PdfPropertyDescriptor("Page", true, PdfPropertyRestrictions.MustBeIndirectReference));
			this.destinationType = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("DestinationType", true));
			this.left = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Left"));
			this.bottom = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Bottom"));
			this.right = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Right"));
			this.top = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Top"));
			this.zoom = base.RegisterDirectProperty<PdfReal>(new PdfPropertyDescriptor("Zoom"));
		}

		public Page Page
		{
			get
			{
				return this.page.GetValue();
			}
			set
			{
				this.page.SetValue(value);
			}
		}

		public PdfName DestinationType
		{
			get
			{
				return this.destinationType.GetValue();
			}
			set
			{
				this.destinationType.SetValue(value);
			}
		}

		public PdfReal Left
		{
			get
			{
				return this.left.GetValue();
			}
			set
			{
				this.left.SetValue(value);
			}
		}

		public PdfReal Bottom
		{
			get
			{
				return this.bottom.GetValue();
			}
			set
			{
				this.bottom.SetValue(value);
			}
		}

		public PdfReal Right
		{
			get
			{
				return this.right.GetValue();
			}
			set
			{
				this.right.SetValue(value);
			}
		}

		public PdfReal Top
		{
			get
			{
				return this.top.GetValue();
			}
			set
			{
				this.top.SetValue(value);
			}
		}

		public PdfReal Zoom
		{
			get
			{
				return this.zoom.GetValue();
			}
			set
			{
				this.zoom.SetValue(value);
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, Destination destination)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Destination>(destination, "destination");
			RadFixedPage radFixedPage = destination.Page;
			Page page;
			if (context.TryGetPage(radFixedPage, out page))
			{
				this.Page = page;
			}
			Matrix dipToPdfPointTransformation = context.GetDipToPdfPointTransformation(radFixedPage);
			this.DestinationType = new PdfName(DestinationTypes.GetDestinationType(destination.DestinationType));
			switch (destination.DestinationType)
			{
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.Location:
			{
				Location location = destination as Location;
				this.Left = DestinationObject.GetXInPdfPoint(dipToPdfPointTransformation, location.Left);
				this.Top = DestinationObject.GetYInPdfPoint(dipToPdfPointTransformation, location.Top);
				this.Zoom = ((location.Zoom != null) ? location.Zoom.Value.ToPdfReal() : null);
				base.IgnoreProperties(new IPdfProperty[] { this.right, this.bottom });
				return;
			}
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.PageFit:
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.BoundingRectangleFit:
				break;
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.PageHorizontalFit:
			{
				PageHorizontalFit pageHorizontalFit = destination as PageHorizontalFit;
				this.Top = DestinationObject.GetYInPdfPoint(dipToPdfPointTransformation, pageHorizontalFit.Top);
				base.IgnoreProperties(new IPdfProperty[] { this.left, this.bottom, this.right, this.zoom });
				return;
			}
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.PageVerticalFit:
			{
				PageVerticalFit pageVerticalFit = destination as PageVerticalFit;
				this.Left = DestinationObject.GetXInPdfPoint(dipToPdfPointTransformation, pageVerticalFit.Left);
				base.IgnoreProperties(new IPdfProperty[] { this.top, this.bottom, this.right, this.zoom });
				return;
			}
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.RectangleFit:
			{
				RectangleFit rectangleFit = destination as RectangleFit;
				this.Left = DestinationObject.GetXInPdfPoint(dipToPdfPointTransformation, rectangleFit.Left);
				this.Bottom = DestinationObject.GetYInPdfPoint(dipToPdfPointTransformation, rectangleFit.Bottom);
				this.Right = DestinationObject.GetXInPdfPoint(dipToPdfPointTransformation, rectangleFit.Right);
				this.Top = DestinationObject.GetYInPdfPoint(dipToPdfPointTransformation, rectangleFit.Top);
				base.IgnoreProperties(new IPdfProperty[] { this.zoom });
				return;
			}
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.BoundingRectangleHorizontalFit:
			{
				BoundingRectangleHorizontalFit boundingRectangleHorizontalFit = destination as BoundingRectangleHorizontalFit;
				this.Top = DestinationObject.GetYInPdfPoint(dipToPdfPointTransformation, boundingRectangleHorizontalFit.Top);
				base.IgnoreProperties(new IPdfProperty[] { this.left, this.right, this.bottom, this.zoom });
				return;
			}
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.BoundingRectangleVerticalFit:
			{
				BoundingRectangleVerticalFit boundingRectangleVerticalFit = destination as BoundingRectangleVerticalFit;
				this.Left = DestinationObject.GetXInPdfPoint(dipToPdfPointTransformation, boundingRectangleVerticalFit.Left);
				base.IgnoreProperties(new IPdfProperty[] { this.top, this.right, this.bottom, this.zoom });
				break;
			}
			default:
				return;
			}
		}

		public Destination ToDestination(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			DestinationType type = DestinationTypes.GetDestinationType(this.DestinationType.Value);
			RadFixedPage fixedPage = context.GetFixedPage(this.Page);
			Matrix pdfPointToDipTransformation = context.GetPdfPointToDipTransformation(this.Page);
			switch (type)
			{
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.Location:
				return new Location
				{
					Page = fixedPage,
					Left = DestinationObject.GetXInDip(pdfPointToDipTransformation, this.Left),
					Top = DestinationObject.GetYInDip(pdfPointToDipTransformation, this.Top),
					Zoom = ((this.Zoom != null && this.Zoom.Value > 0.0) ? new double?(this.Zoom.Value) : null)
				};
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.PageHorizontalFit:
				return new PageHorizontalFit
				{
					Page = fixedPage,
					Top = DestinationObject.GetYInDip(pdfPointToDipTransformation, this.Top)
				};
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.PageVerticalFit:
				return new PageVerticalFit
				{
					Page = fixedPage,
					Left = DestinationObject.GetXInDip(pdfPointToDipTransformation, this.Left)
				};
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.RectangleFit:
				return new RectangleFit
				{
					Page = fixedPage,
					Left = DestinationObject.GetXInDip(pdfPointToDipTransformation, this.Left),
					Bottom = DestinationObject.GetYInDip(pdfPointToDipTransformation, this.Bottom),
					Right = DestinationObject.GetXInDip(pdfPointToDipTransformation, this.Right),
					Top = DestinationObject.GetYInDip(pdfPointToDipTransformation, this.Top)
				};
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.BoundingRectangleHorizontalFit:
				return new BoundingRectangleHorizontalFit
				{
					Page = fixedPage,
					Top = DestinationObject.GetYInDip(pdfPointToDipTransformation, this.Top)
				};
			case Telerik.Windows.Documents.Fixed.Model.Navigation.DestinationType.BoundingRectangleVerticalFit:
				return new BoundingRectangleVerticalFit
				{
					Page = fixedPage,
					Left = DestinationObject.GetXInDip(pdfPointToDipTransformation, this.Left)
				};
			}
			return Destination.CreateDestination(type, fixedPage);
		}

		static double? GetXInDip(Matrix pointToDipMatrix, PdfReal xCoordinateInPdfPoint)
		{
			if (xCoordinateInPdfPoint != null)
			{
				return new double?(pointToDipMatrix.Transform(new Point(xCoordinateInPdfPoint.Value, 0.0)).X);
			}
			return null;
		}

		static double? GetYInDip(Matrix pointToDipMatrix, PdfReal yCoordinateInPdfPoint)
		{
			if (yCoordinateInPdfPoint != null)
			{
				return new double?(pointToDipMatrix.Transform(new Point(0.0, yCoordinateInPdfPoint.Value)).Y);
			}
			return null;
		}

		static PdfReal GetXInPdfPoint(Matrix dipToPointMatrix, double? xCoordinateInDip)
		{
			if (xCoordinateInDip != null)
			{
				return dipToPointMatrix.Transform(new Point(xCoordinateInDip.Value, 0.0)).X.ToPdfReal();
			}
			return null;
		}

		static PdfReal GetYInPdfPoint(Matrix dipToPointMatrix, double? yCoordinateInDip)
		{
			if (yCoordinateInDip != null)
			{
				return dipToPointMatrix.Transform(new Point(0.0, yCoordinateInDip.Value)).Y.ToPdfReal();
			}
			return null;
		}

		readonly ReferenceProperty<Page> page;

		readonly DirectProperty<PdfName> destinationType;

		readonly DirectProperty<PdfReal> left;

		readonly DirectProperty<PdfReal> top;

		readonly DirectProperty<PdfReal> bottom;

		readonly DirectProperty<PdfReal> right;

		readonly DirectProperty<PdfReal> zoom;
	}
}
