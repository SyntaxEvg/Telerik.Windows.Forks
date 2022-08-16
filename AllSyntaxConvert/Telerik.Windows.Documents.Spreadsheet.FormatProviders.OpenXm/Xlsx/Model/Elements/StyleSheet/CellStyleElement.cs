using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class CellStyleElement : StyleSheetElementBase
	{
		public CellStyleElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("name", false));
			this.formatId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("xfId", true));
			this.builtinId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("builtinId", false));
			this.customBuiltin = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customBuiltin"));
		}

		public int BuiltinId
		{
			get
			{
				return this.builtinId.Value;
			}
			set
			{
				this.builtinId.Value = value;
			}
		}

		public bool CustomBuiltin
		{
			get
			{
				return this.customBuiltin.Value;
			}
			set
			{
				this.customBuiltin.Value = value;
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		public int FormatId
		{
			get
			{
				return this.formatId.Value;
			}
			set
			{
				this.formatId.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "cellStyle";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, StyleInfo style)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<StyleInfo>(style, "style");
			this.Name = style.Name;
			this.FormatId = style.FormattingRecordId;
			if (style.BuiltInId != null)
			{
				this.BuiltinId = style.BuiltInId.Value;
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			StyleInfo resource = default(StyleInfo);
			resource.Name = this.Name;
			resource.FormattingRecordId = this.FormatId;
			if (this.builtinId.HasValue)
			{
				resource.BuiltInId = new int?(this.BuiltinId);
			}
			context.StyleSheet.StyleInfoTable.Add(resource);
		}

		readonly IntOpenXmlAttribute builtinId;

		readonly BoolOpenXmlAttribute customBuiltin;

		readonly OpenXmlAttribute<string> name;

		readonly IntOpenXmlAttribute formatId;
	}
}
