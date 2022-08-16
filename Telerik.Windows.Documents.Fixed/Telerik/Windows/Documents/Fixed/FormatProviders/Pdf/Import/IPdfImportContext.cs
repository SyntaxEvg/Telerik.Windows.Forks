using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	interface IPdfImportContext
	{
		PdfImportSettings ImportSettings { get; }

		CrossReferenceCollection CrossReferences { get; }

		ReferenceProperty<DocumentCatalog> Root { get; set; }

		PostScriptReader Reader { get; }

		List<Page> Pages { get; }

		byte[] DocumentId { get; set; }

		Encrypt Encryption { get; set; }

		IndirectReference CurrentIndirectReference { get; }

		void BeginImport(Stream pdfFileStream);

		IDisposable BeginImportOfStreamInnerContent();

		void RegisterIndirectObject(IndirectReference reference, PdfPrimitive primitive);

		bool TryGetIndirectObject(IndirectReference reference, out PdfPrimitive primitive);

		IndirectObject ReadIndirectObject(IndirectReference reference);

		byte[] DecryptStream(IndirectReference reference, byte[] data);

		byte[] DecryptString(byte[] data);

		void AddWidgetParent(WidgetObject widgetObject, FormFieldNode parent);

		IEnumerable<WidgetObject> GetChildWidgets(FormFieldNode parent);

		void MapWidgets(WidgetObject widgetObject, Widget widget);

		bool TryGetWidget(WidgetObject widgetObject, out Widget widget);

		void MapFields(FormFieldNode node, FormField field);

		bool TryGetField(FormFieldNode node, out FormField field);

		void MapPageToPdfDictionary(PageTreeNode page, PdfDictionary dictionary);

		void UnmapPdfDictionary(PageTreeNode page);

		bool TryGetPdfDictionary(PageTreeNode page, out PdfDictionary dictionary);
	}
}
