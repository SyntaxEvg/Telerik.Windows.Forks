using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class RowElement : WorksheetElementBase
	{
		public RowElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.rowIndex = base.RegisterAttribute<ConvertedOpenXmlAttribute<int?>>(new ConvertedOpenXmlAttribute<int?>("r", Converters.NullableIntValueConverter, false));
			this.rowHeight = base.RegisterAttribute<double>("ht", false);
			this.styleIndex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("s", 0, false));
			this.customFormat = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customFormat"));
			this.customHeight = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("customHeight"));
			this.hidden = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("hidden"));
			this.outlineLevel = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("outlineLevel", false));
		}

		public double RowHeight
		{
			get
			{
				return this.rowHeight.Value;
			}
			set
			{
				this.rowHeight.Value = value;
			}
		}

		public int? RowIndex
		{
			get
			{
				return this.rowIndex.Value;
			}
			set
			{
				this.rowIndex.Value = value;
			}
		}

		public bool CustomFormat
		{
			get
			{
				return this.customFormat.Value;
			}
			set
			{
				this.customFormat.Value = value;
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
				return "row";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, RowInfo row)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<RowInfo>(row, "row");
			this.RowIndex = row.RowIndex + 1;
			if (row.IsCustom && !row.IsDefault && row.Height != null)
			{
				this.CustomHeight = true;
				this.RowHeight = UnitHelper.DipToPoint(row.Height.Value);
			}
			if (row.OutlineLevel != this.outlineLevel.DefaultValue)
			{
				this.OutlineLevel = row.OutlineLevel;
			}
			if (row.Hidden != this.hidden.DefaultValue)
			{
				this.Hidden = row.Hidden;
			}
			int? formattingRecordIndex = context.GetFormattingRecordIndex(context.Worksheet.Rows, (long)row.RowIndex.Value);
			if (formattingRecordIndex != null)
			{
				this.StyleIndex = formattingRecordIndex.Value;
				this.CustomFormat = true;
			}
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (CellInfo cell in context.GetNonEmptyCellsInRow(this.RowIndex.Value - 1))
			{
				CellElement cellElement = base.CreateElement<CellElement>("c");
				cellElement.CopyPropertiesFrom(context, cell);
				yield return cellElement;
			}
			yield break;
		}

		protected override void OnAfterReadAttributes(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			RowInfo rowInfo = new RowInfo();
			rowInfo.RowIndex = this.RowIndex - 1;
			rowInfo.IsCustom = this.CustomHeight;
			rowInfo.Hidden = this.Hidden;
			rowInfo.OutlineLevel = this.OutlineLevel;
			rowInfo.StyleIndex = this.StyleIndex;
			if (rowInfo.IsCustom)
			{
				rowInfo.Height = new double?(UnitHelper.PointToDip(this.RowHeight));
			}
			context.ApplyRowInfo(rowInfo);
		}

		readonly OpenXmlAttribute<double> rowHeight;

		readonly ConvertedOpenXmlAttribute<int?> rowIndex;

		readonly BoolOpenXmlAttribute customFormat;

		readonly BoolOpenXmlAttribute customHeight;

		readonly IntOpenXmlAttribute styleIndex;

		readonly BoolOpenXmlAttribute hidden;

		readonly IntOpenXmlAttribute outlineLevel;
	}
}
