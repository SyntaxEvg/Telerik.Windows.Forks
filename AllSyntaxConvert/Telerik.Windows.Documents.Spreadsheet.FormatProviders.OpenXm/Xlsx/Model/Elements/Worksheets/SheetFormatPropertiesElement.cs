using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SheetFormatPropertiesElement : WorksheetElementBase
	{
		public SheetFormatPropertiesElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.customHeight = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customHeight"));
			this.defaultColumnWidth = base.RegisterAttribute<double>("defaultColWidth", false);
			this.defaultRowHeight = base.RegisterAttribute<double>("defaultRowHeight", false);
		}

		public override string ElementName
		{
			get
			{
				return "sheetFormatPr";
			}
		}

		public bool CustomHeight
		{
			get
			{
				return this.customHeight.Value;
			}
			set
			{
				this.customHeight.Value = value;
			}
		}

		public double DefaultColumnWidth
		{
			get
			{
				return this.defaultColumnWidth.Value;
			}
			set
			{
				this.defaultColumnWidth.Value = value;
			}
		}

		public double DefaultRowHeight
		{
			get
			{
				return this.defaultRowHeight.Value;
			}
			set
			{
				this.defaultRowHeight.Value = value;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			this.DefaultColumnWidth = context.GetDefaultColumnWidth();
			RowHeight rowHeight = context.GetDefaultRowHeight();
			this.CustomHeight = rowHeight.IsCustom;
			this.DefaultRowHeight = UnitHelper.DipToPoint(rowHeight.Value);
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			if (this.defaultColumnWidth.HasValue)
			{
				context.ApplyDefaultColumnWidth(new ColumnWidth(XlsxHelper.ConvertColumnExcelWidthToPixelWidth(context.Worksheet.Workbook, this.DefaultColumnWidth), false));
			}
			if (this.defaultRowHeight.HasValue)
			{
				context.ApplyDefaultColumnWidthDefaultRowHeight(new RowHeight(UnitHelper.PointToDip(this.DefaultRowHeight), this.CustomHeight));
			}
		}

		readonly BoolOpenXmlAttribute customHeight;

		readonly OpenXmlAttribute<double> defaultColumnWidth;

		readonly OpenXmlAttribute<double> defaultRowHeight;
	}
}
