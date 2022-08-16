using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps
{
	class EncodingPredefined : EncodingBaseObject
	{
		public EncodingPredefined(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
		}

		public override PdfElementType ExportAs
		{
			get
			{
				return PdfElementType.PdfName;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public override IEnumerable<CharCode> BuildCharCodes(PdfString s)
		{
			throw new NotImplementedException();
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.WritePdfName(this.Name);
		}

		readonly string name;
	}
}
