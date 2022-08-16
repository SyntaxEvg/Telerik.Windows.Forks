using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	interface IRadFixedDocumentImportContext : IPdfImportContext
	{
		RadFixedDocument Document { get; }

		void MapPages(Page page, RadFixedPage fixedPage);

		RadFixedPage GetFixedPage(Page page);

		Page GetPage(RadFixedPage fixedPage);

		IPdfContentImportContext GetAcroFormContentImportContext();

		Matrix GetPdfPointToDipTransformation(Page fixedPage);

		FontBase GetFont(PostScriptReader reader, FontObject font);

		Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource GetImageSource(PostScriptReader reader, ImageXObject image);

		FormSource GetFormSource(PostScriptReader reader, FormXObject form);

		ExtGState GetExtGState(PostScriptReader reader, ExtGStateObject extGState);
	}
}
