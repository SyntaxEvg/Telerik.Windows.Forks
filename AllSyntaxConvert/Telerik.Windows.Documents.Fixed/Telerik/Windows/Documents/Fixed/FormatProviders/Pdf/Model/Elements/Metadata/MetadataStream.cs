using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata
{
	abstract class MetadataStream : PdfStreamObjectBase
	{
		public abstract IEnumerable<DescriptionElement> Descriptions { get; }

		public override bool ShouldEncryptData
		{
			get
			{
				return false;
			}
		}

		protected override PdfArray GetExportFilters(IPdfExportContext context)
		{
			if (context.Settings.ComplianceLevel == PdfComplianceLevel.None)
			{
				return base.GetExportFilters(context);
			}
			return new PdfArray(new PdfPrimitive[0]);
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			byte[] result;
			using (Stream stream = new MemoryStream())
			{
				using (XmpWriter xmpWriter = new XmpWriter(stream))
				{
					new HeaderElement().Write(xmpWriter);
					new XmpMetaElement(this.Descriptions).Write(xmpWriter);
					new PaddingElement().Write(xmpWriter);
					new TrailerElement().Write(xmpWriter);
				}
				result = stream.ReadAllBytes();
			}
			return result;
		}
	}
}
