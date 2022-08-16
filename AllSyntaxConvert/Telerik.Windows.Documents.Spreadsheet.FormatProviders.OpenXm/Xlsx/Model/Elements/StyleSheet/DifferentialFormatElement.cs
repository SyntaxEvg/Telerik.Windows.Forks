using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class DifferentialFormatElement : StyleSheetElementBase
	{
		public DifferentialFormatElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.font = base.RegisterChildElement<FontElement>("font");
			this.fill = base.RegisterChildElement<FillElement>("fill");
		}

		public FontElement Font
		{
			get
			{
				return this.font.Element;
			}
			set
			{
				this.font.Element = value;
			}
		}

		public FillElement Fill
		{
			get
			{
				return this.fill.Element;
			}
			set
			{
				this.fill.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "dxf";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, DifferentialFormatInfo info)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DifferentialFormatInfo>(info, "info");
			if (info.FontInfo != null)
			{
				base.CreateElement(this.font);
				this.Font.CopyPropertiesFrom(context, info.FontInfo.Value);
			}
			if (info.Fill != null)
			{
				base.CreateElement(this.fill);
				this.Fill.CopyPropertiesFrom(context, info.Fill);
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			if (this.Font != null)
			{
				FontInfo value = this.Font.FontInfo.Value;
				context.DifferentialFormatsContext.RegisterFont(value);
				base.ReleaseElement(this.font);
			}
			if (this.Fill != null)
			{
				IFill fill = this.Fill.Fill;
				context.DifferentialFormatsContext.RegisterFill(fill);
				base.ReleaseElement(this.fill);
			}
		}

		readonly OpenXmlChildElement<FontElement> font;

		readonly OpenXmlChildElement<FillElement> fill;
	}
}
