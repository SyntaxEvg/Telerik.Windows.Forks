using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureByteRange : PrimitiveWrapper
	{
		public SignatureByteRange(PdfArray array)
			: base(array)
		{
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			context.SignatureExportInfo.AddByteRangeStartPosition((int)writer.Position);
			base.Write(writer, context);
			context.SignatureExportInfo.AddByteRangeEndPosition((int)writer.Position);
		}

		public static SignatureByteRange ToSignatureByteRange(PdfProperty<int[]> collection)
		{
			if (collection == null || !collection.HasValue)
			{
				return null;
			}
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (int defaultValue in collection.Value)
			{
				pdfArray.Add(new PdfInt(defaultValue));
			}
			return new SignatureByteRange(pdfArray);
		}
	}
}
