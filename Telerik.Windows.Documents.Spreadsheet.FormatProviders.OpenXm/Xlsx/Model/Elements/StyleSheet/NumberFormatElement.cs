using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class NumberFormatElement : StyleSheetElementBase
	{
		public NumberFormatElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.formatCode = base.RegisterAttribute<string>("formatCode", true);
			this.formatId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("numFmtId", true));
		}

		public string FormatCode
		{
			get
			{
				return this.formatCode.Value;
			}
			set
			{
				this.formatCode.Value = value;
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

		public bool HasFormatId
		{
			get
			{
				return this.formatId.HasValue;
			}
		}

		public override string ElementName
		{
			get
			{
				return "numFmt";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, CellValueFormat cellValueFormat)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<CellValueFormat>(cellValueFormat, "cellValueFormat");
			if (!NumberFormatTypes.IsBuiltInFormat(cellValueFormat.FormatString))
			{
				this.FormatCode = cellValueFormat.InvariantFormatString;
			}
			this.FormatId = context.StyleSheet.CellValueFormatTable.GetIndex(cellValueFormat);
		}

		readonly OpenXmlAttribute<string> formatCode;

		readonly IntOpenXmlAttribute formatId;
	}
}
