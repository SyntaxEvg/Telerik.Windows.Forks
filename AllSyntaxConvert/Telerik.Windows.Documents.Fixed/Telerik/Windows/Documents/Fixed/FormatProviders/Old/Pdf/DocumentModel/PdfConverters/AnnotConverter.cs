using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class AnnotConverter : IndirectReferenceConverterBase
	{
		public AnnotConverter()
		{
			this.nameToAnnotationType = new Dictionary<string, AnnotationType>();
			this.nameToAnnotationType["Text"] = AnnotationType.Text;
			this.nameToAnnotationType["FreeText"] = AnnotationType.FreeText;
			this.nameToAnnotationType["Line"] = AnnotationType.Line;
			this.nameToAnnotationType["Square"] = AnnotationType.Square;
			this.nameToAnnotationType["Circle"] = AnnotationType.Circle;
			this.nameToAnnotationType["Polygon"] = AnnotationType.Polygon;
			this.nameToAnnotationType["PolyLine"] = AnnotationType.PolyLine;
			this.nameToAnnotationType["Highlight"] = AnnotationType.Highlight;
			this.nameToAnnotationType["Underline"] = AnnotationType.Underline;
			this.nameToAnnotationType["Squiggly"] = AnnotationType.Squiggly;
			this.nameToAnnotationType["StrikeOut"] = AnnotationType.StrikeOut;
			this.nameToAnnotationType["Stamp"] = AnnotationType.Stamp;
			this.nameToAnnotationType["Caret"] = AnnotationType.Caret;
			this.nameToAnnotationType["Ink"] = AnnotationType.Ink;
			this.nameToAnnotationType["Popup"] = AnnotationType.Popup;
			this.nameToAnnotationType["FileAttachment"] = AnnotationType.FileAttachment;
			this.nameToAnnotationType["Sound"] = AnnotationType.Sound;
			this.nameToAnnotationType["Movie"] = AnnotationType.Movie;
			this.nameToAnnotationType["Screen"] = AnnotationType.Screen;
			this.nameToAnnotationType["PrinterMark"] = AnnotationType.PrinterMark;
			this.nameToAnnotationType["TrapNet"] = AnnotationType.TrapNet;
			this.nameToAnnotationType["Watermark"] = AnnotationType.Watermark;
			this.nameToAnnotationType["3D"] = AnnotationType.ThreeD;
			this.nameToAnnotationType["Redact"] = AnnotationType.Redact;
		}

		protected override object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			AnnotationOld annotationOld = null;
			PdfNameOld element = dictionary.GetElement<PdfNameOld>("Subtype");
			if (element != null)
			{
				string value;
				if ((value = element.Value) != null)
				{
					if (value == "Link")
					{
						annotationOld = new LinkOld(contentManager);
						goto IL_7C;
					}
					if (value == "Widget")
					{
						annotationOld = new WidgetOld(contentManager);
						goto IL_7C;
					}
				}
				AnnotationType type2;
				if (this.nameToAnnotationType.TryGetValue(element.Value, out type2))
				{
					annotationOld = new UnsupportedAnnotationOld(contentManager, type2);
				}
			}
			IL_7C:
			if (annotationOld != null)
			{
				annotationOld.Load(dictionary);
			}
			return annotationOld;
		}

		readonly Dictionary<string, AnnotationType> nameToAnnotationType;
	}
}
