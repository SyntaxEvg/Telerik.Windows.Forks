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
	class CidSystemInfo : global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfObject
	{
		public CidSystemInfo()
		{
			this.registry = base.RegisterDirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString>(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.PdfPropertyDescriptor("Registry", true));
			this.ordering = base.RegisterDirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString>(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.PdfPropertyDescriptor("Ordering", true));
			this.supplement = base.RegisterDirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt>(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.PdfPropertyDescriptor("Supplement", true));
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString Registry
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

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString Ordering
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

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt Supplement
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

		public void CopyPropertiesFrom(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context, global::Telerik.Windows.Documents.Fixed.Model.Fonts.CidFontBase font)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext>(context, "context");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Fonts.CidFontBase>(font, "font");
			global::Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding.CidSystemInfo value = font.CidSystemInfo.Value;
			this.Registry = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfLiteralString(value.Registry, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.StringType.ASCII);
			this.Ordering = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfLiteralString(value.Ordering, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.StringType.ASCII);
			this.Supplement = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt(value.Supplement);
		}

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.DirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString> registry;

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.DirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString> ordering;

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.DirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt> supplement;
	}
}
