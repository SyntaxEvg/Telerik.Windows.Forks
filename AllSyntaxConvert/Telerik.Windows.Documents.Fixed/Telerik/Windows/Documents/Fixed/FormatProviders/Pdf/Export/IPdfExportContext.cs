using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	interface IPdfExportContext
	{
		byte[] DocumentId { get; }

		RadFixedDocumentInfo DocumentInfo { get; }

		bool CanWriteColors { get; set; }

		Encrypt Encryption { get; set; }

		IPdfContentExportContext AcroFormContentExportContext { get; set; }

		PdfExportSettings Settings { get; }

		CrossReferenceCollection CrossReferenceCollection { get; }

		Queue<IndirectObject> IndirectObjectsQueue { get; }

		IEnumerable<FontBase> FontResources { get; }

		SignatureExportInfo SignatureExportInfo { get; }

		IndirectObject CreateIndirectObject(PdfPrimitive primitive);

		FormFieldNode CreateFormFieldObject(FormField field, bool includeInIndirectObjectQueue);

		void BeginExportIndirectObject(IndirectObject indirectObject, long offset);

		void EndExportIndirectObject();

		ResourceEntry GetResource(FontBase resource);

		ResourceEntry GetResource(FormSource form);

		ResourceEntry GetResource(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource image);

		ResourceEntry GetResource(ExtGState state);

		ResourceEntry GetResource(PatternColor pattern, IPdfContentExportContext context);

		ResourceEntry GetResource(ColorSpaceBase first, ColorSpaceBase second);

		IPdfContentExportContext CreateContentExportContext(IResourceHolder resourceHolder, IContentRootElement contentRoot);

		Matrix GetDipToPdfPointTransformation(IContentRootElement contentRoot);

		void SetUsedCharacters(FontBase font, TextCollection glyphs);

		IEnumerable<CharInfo> GetUsedCharacters(FontBase font);

		byte[] EncryptStream(byte[] data);

		byte[] EncryptString(byte[] data);

		void MapPages(RadFixedPage fixedPage, Page page);

		bool TryGetPage(RadFixedPage fixedPage, out Page page);

		void MapWidgets(Widget widgetModel, WidgetObject widgetObject);

		bool TryGetWidget(Widget widgetModel, out WidgetObject widgetObject);
	}
}
