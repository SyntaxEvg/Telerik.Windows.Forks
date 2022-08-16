using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfHexString : PdfString
	{
		public PdfHexString()
		{
		}

		public PdfHexString(byte[] initialValue)
			: base(initialValue)
		{
		}

		public override StringObjectType StringObjectType
		{
			get
			{
				return StringObjectType.Hex;
			}
		}

		public override string ToString()
		{
			return PdfString.GetString(base.Value);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			string arg = BytesHelper.ToHexString(context.EncryptString(base.Value));
			string value = string.Format("{0}{1}{2}", PdfNames.HexadecimalStringStart, arg, PdfNames.HexadecimalStringEnd);
			writer.Write(value);
		}
	}
}
