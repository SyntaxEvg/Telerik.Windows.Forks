using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class CidSystemInfo : PdfObject
	{
		public CidSystemInfo()
		{
			this.registry = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("Registry", true));
			this.ordering = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("Ordering", true));
			this.supplement = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Supplement", true));
		}

		public PdfString Registry
		{
			get
			{
				return this.registry.GetValue();
			}
			set
			{
				this.registry.SetValue(value);
			}
		}

		public PdfString Ordering
		{
			get
			{
				return this.ordering.GetValue();
			}
			set
			{
				this.ordering.SetValue(value);
			}
		}

		public PdfInt Supplement
		{
			get
			{
				return this.supplement.GetValue();
			}
			set
			{
				this.supplement.SetValue(value);
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, CidFontBase font)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<CidFontBase>(font, "font");
			CidSystemInfo value = font.CidSystemInfo.Value;
			this.Registry = new PdfLiteralString(value.Registry, StringType.ASCII);
			this.Ordering = new PdfLiteralString(value.Ordering, StringType.ASCII);
			this.Supplement = new PdfInt(value.Supplement);
		}

		readonly DirectProperty<PdfString> registry;

		readonly DirectProperty<PdfString> ordering;

		readonly DirectProperty<PdfInt> supplement;
	}
}
