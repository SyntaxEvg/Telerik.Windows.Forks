using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureContents : PdfHexString
	{
		public SignatureContents(byte[] initialValue)
			: base(initialValue)
		{
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			context.SignatureExportInfo.AddContentStartPosition((int)writer.Position);
			base.Write(writer, context);
			context.SignatureExportInfo.AddContentEndPosition((int)writer.Position - 1);
		}
	}
}
