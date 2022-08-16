using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class ColumnElement : WorksheetElementBase
	{
		public ColumnElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.min = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("min", true));
			this.max = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("max", true));
			this.bestFit = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("bestFit"));
			this.customWidth = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customWidth"));
			this.styleIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("style", 0, false));
			this.width = base.RegisterAttribute<double>("width", false);
			this.hidden = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("hidden"));
			this.outlineLevel = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("outlineLevel", false));
		}

		public int Min
		{
			get
			{
				return this.min.Value;
			}
			set
			{
				this.min.Value = value;
			}
		}

		public int Max
		{
			get
			{
				return this.max.Value;
			}
			set
			{
				this.max.Value = value;
			}
		}

		public bool BestFit
		{
			get
			{
				return this.bestFit.Value;
			}
			set
			{
				this.bestFit.Value = value;
			}
		}

		public bool CustomWidth
		{
			get
			{
				return this.customWidth.Value;
			}
			set
			{
				this.customWidth.Value = value;
			}
		}

		public int StyleIndex
		{
			get
			{
				return this.styleIndex.Value;
			}
			set
			{
				this.styleIndex.Value = value;
			}
		}

		public double Width
		{
			get
			{
				return this.width.Value;
			}
			set
			{
				this.width.Value = value;
			}
		}

		public bool Hidden
		{
			get
			{
				return this.hidden.Value;
			}
			set
			{
				this.hidden.Value = value;
			}
		}

		public int OutlineLevel
		{
			get
			{
				return this.outlineLevel.Value;
			}
			set
			{
				this.outlineLevel.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "col";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, Range<long, ColumnInfo> columnRange)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Range<long, ColumnInfo>>(columnRange, "columnRange");
			ColumnInfo value = columnRange.Value;
			Guard.ThrowExceptionIfNull<ColumnInfo>(value, "column");
			this.Min = (int)columnRange.Start + 1;
			this.Max = (int)columnRange.End + 1;
			this.BestFit = value.BestFit;
			this.Width = value.Width;
			this.OutlineLevel = value.OutlineLevel;
			this.Hidden = value.Hidden;
			int? formattingRecordIndex = context.GetFormattingRecordIndex(context.Worksheet.Columns, columnRange.Start);
			if (formattingRecordIndex != null)
			{
				this.StyleIndex = formattingRecordIndex.Value;
			}
			if (value.IsCustom)
			{
				this.CustomWidth = value.IsCustom;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			ColumnInfo value = default(ColumnInfo);
			value.BestFit = this.BestFit;
			value.IsCustom = this.CustomWidth;
			value.Hidden = this.Hidden;
			value.OutlineLevel = this.OutlineLevel;
			value.Width = XlsxHelper.ConvertColumnExcelWidthToPixelWidth(context.Worksheet.Workbook, this.Width);
			value.StyleIndex = this.StyleIndex;
			Range<long, ColumnInfo> columnInfo = new Range<long, ColumnInfo>((long)(this.Min - 1), (long)(this.Max - 1), true, value);
			context.ApplyColumnInfo(columnInfo);
		}

		readonly IntOpenXmlAttribute min;

		readonly IntOpenXmlAttribute max;

		readonly BoolOpenXmlAttribute bestFit;

		readonly BoolOpenXmlAttribute customWidth;

		readonly IntOpenXmlAttribute styleIndex;

		readonly OpenXmlAttribute<double> width;

		readonly BoolOpenXmlAttribute hidden;

		readonly IntOpenXmlAttribute outlineLevel;
	}
}
