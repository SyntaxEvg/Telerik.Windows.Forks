using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	interface IPdfContentImportContext
	{
		IRadFixedDocumentImportContext Owner { get; }

		IResourceHolder ResourceHolder { get; }

		IContentRootElement ContentRoot { get; }

		Marker CurrentMarker { get; set; }

		FontObject GetFont(PostScriptReader reader, PdfName key);

		XObjectBase GetXObject(PostScriptReader reader, PdfName key);

		ExtGStateObject GetExtGState(PostScriptReader reader, PdfName key);

		PatternColorObject GetPatternColor(PostScriptReader reader, PdfName key);

		ColorSpaceObject GetColorSpace(PostScriptReader reader, PdfName key);
	}
}
