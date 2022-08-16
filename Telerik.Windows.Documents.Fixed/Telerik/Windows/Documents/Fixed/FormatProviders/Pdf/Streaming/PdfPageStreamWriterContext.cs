using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	class PdfPageStreamWriterContext
	{
		public Page PageObject { get; set; }

		public Rect MediaBox { get; set; }

		public Rect CropBox { get; set; }

		public Rotation PageRotation { get; set; }

		public PdfStreamWriterSettings Settings { get; set; }

		public PdfFileStreamExportContext ExportContext { get; set; }

		public Action DoOnPageWritingEndedAction { get; set; }
	}
}
