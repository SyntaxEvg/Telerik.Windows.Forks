using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.Writers
{
	static class PdfElementWriters
	{
		public static CMapWriter CMapWriter
		{
			get
			{
				return PdfElementWriters.cmapWriter;
			}
		}

		static readonly CMapWriter cmapWriter = new CMapWriter();
	}
}
