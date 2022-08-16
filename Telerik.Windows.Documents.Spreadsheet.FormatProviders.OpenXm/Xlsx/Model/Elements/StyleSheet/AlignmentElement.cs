using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class AlignmentElement : StyleSheetElementBase
	{
		public AlignmentElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.horizontalAlignment = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("horizontal", false));
			this.verticalAlignment = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("vertical", false));
			this.indent = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("indent", false));
			this.wrapText = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("wrapText"));
		}

		public string HorizontalAlignment
		{
			get
			{
				return this.horizontalAlignment.Value;
			}
			set
			{
				this.horizontalAlignment.Value = value;
			}
		}

		public string VerticalAlignment
		{
			get
			{
				return this.verticalAlignment.Value;
			}
			set
			{
				this.verticalAlignment.Value = value;
			}
		}

		public bool WrapText
		{
			get
			{
				return this.wrapText.Value;
			}
			set
			{
				this.wrapText.Value = value;
			}
		}

		public int Indent
		{
			get
			{
				return this.indent.Value;
			}
			set
			{
				this.indent.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "alignment";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			if (format.HorizontalAlignment != null)
			{
				this.HorizontalAlignment = HorizontalAlignments.GetHorizontalAlignmentName(format.HorizontalAlignment.Value);
			}
			if (format.VerticalAlignment != null)
			{
				this.VerticalAlignment = VerticalAlignments.GetVerticalAlignmentName(format.VerticalAlignment.Value);
			}
			if (format.Indent != null)
			{
				this.Indent = format.Indent.Value;
			}
			if (format.WrapText != null)
			{
				this.WrapText = format.WrapText.Value;
			}
		}

		public void ApplyAllignment(IXlsxWorkbookImportContext context, ref FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			if (this.horizontalAlignment.HasValue)
			{
				format.HorizontalAlignment = new RadHorizontalAlignment?(HorizontalAlignments.GetHorizontalAlignment(this.HorizontalAlignment));
			}
			if (this.verticalAlignment.HasValue)
			{
				format.VerticalAlignment = new RadVerticalAlignment?(VerticalAlignments.GetVerticalAlignment(this.VerticalAlignment));
			}
			if (this.indent.HasValue)
			{
				format.Indent = new int?(this.Indent);
			}
			if (this.wrapText.HasValue)
			{
				format.WrapText = new bool?(this.WrapText);
			}
		}

		readonly OpenXmlAttribute<string> horizontalAlignment;

		readonly OpenXmlAttribute<string> verticalAlignment;

		readonly BoolOpenXmlAttribute wrapText;

		readonly IntOpenXmlAttribute indent;
	}
}
