using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class IndexedLookupStream : PdfStreamObjectBase
	{
		public IndexedLookupStream()
		{
		}

		public IndexedLookupStream(byte[] data)
		{
			this.data = data;
		}

		public IEnumerable<byte> Data
		{
			get
			{
				return this.data;
			}
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			this.data = stream.ReadDecodedPdfData();
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			return this.data;
		}

		public override bool Equals(object obj)
		{
			IndexedLookupStream indexedLookupStream = obj as IndexedLookupStream;
			return indexedLookupStream != null && this.data.Length == indexedLookupStream.data.Length && ObjectExtensions.ArrayValuesEquals<byte>(this.data, indexedLookupStream.data);
		}

		public override int GetHashCode()
		{
			return this.data.GetHashCodeOrZero();
		}

		byte[] data;
	}
}
