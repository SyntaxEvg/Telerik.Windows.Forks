using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	interface IResourceRenamingExportContext
	{
		IndirectObject CreateIndirectObject(PdfPrimitive primitive);

		bool TryGetContextIndirectReference(PdfFileSource source, IndirectReference sourceReference, out IndirectReference contextReference);

		void AddSourceToContextReferenceMapping(PdfFileSource source, IndirectReference sourceReference, IndirectReference contextReference);
	}
}
