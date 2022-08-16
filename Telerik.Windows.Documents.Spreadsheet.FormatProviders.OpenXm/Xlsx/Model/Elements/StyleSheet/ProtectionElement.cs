using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class ProtectionElement : StyleSheetElementBase
	{
		public ProtectionElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.locked = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("locked"));
		}

		public bool Locked
		{
			get
			{
				return this.locked.Value;
			}
			set
			{
				this.locked.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "protection";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			if (format.IsLocked != null)
			{
				this.Locked = format.IsLocked.Value;
			}
		}

		public void ApplyProtection(IXlsxWorkbookImportContext context, ref FormattingRecord format)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FormattingRecord>(format, "format");
			if (this.locked.HasValue)
			{
				format.IsLocked = new bool?(this.Locked);
			}
		}

		readonly BoolOpenXmlAttribute locked;
	}
}
