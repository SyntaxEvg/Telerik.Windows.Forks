using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	static class AnnotationFactory
	{
		public static AnnotationObject CreateInstance(PdfName type)
		{
			Guard.ThrowExceptionIfNull<PdfName>(type, "type");
			string value;
			if ((value = type.Value) != null)
			{
				if (value == "Link")
				{
					return new LinkObject();
				}
				if (value == "Widget")
				{
					return new WidgetObject();
				}
			}
			return new UnsupportedAnnotation();
		}

		public static AnnotationObject CreatePdfAnnotation(IPdfExportContext context, Annotation annotation, RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Annotation>(annotation, "annotation");
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			switch (annotation.Type)
			{
			case AnnotationType.Link:
			{
				LinkObject linkObject = new LinkObject();
				Link annotation2 = (Link)annotation;
				linkObject.CopyPropertiesFrom(context, annotation2, fixedPage);
				return linkObject;
			}
			case AnnotationType.Widget:
			{
				WidgetObject widgetObject = new WidgetObject();
				Widget widget = (Widget)annotation;
				widgetObject.CopyPropertiesFrom(context, widget, fixedPage);
				context.MapWidgets(widget, widgetObject);
				return widgetObject;
			}
			default:
				throw new NotSupportedException("Annotation type is not supported");
			}
		}
	}
}
