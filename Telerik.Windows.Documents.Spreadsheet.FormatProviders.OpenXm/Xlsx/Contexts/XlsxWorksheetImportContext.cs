using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Model.Protection;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class XlsxWorksheetImportContext : XlsxWorksheetContextBase, IXlsxWorksheetImportContext, IOpenXmlImportContext
	{
		public XlsxWorksheetImportContext(XlsxWorkbookImportContext workbookContext, Worksheet worksheet)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<XlsxWorkbookImportContext>(workbookContext, "workbookContext");
			this.workbookContext = workbookContext;
			this.cellStyleIndexCompressedList = new CompressedList<int>(0L, SpreadsheetDefaultValues.CellCount - 1L);
			this.rowStyleIndexCompressedList = new CompressedList<int>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1));
			this.columnStyleIndexCompressedList = new CompressedList<int>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1));
			this.sharedFormulas = new Dictionary<int, SharedFormulaInfo>();
			this.shapesIndices = new Dictionary<FloatingShapeBase, TwoCellAnchorInfo>();
			this.referenceCounter = new ReferenceCounter();
		}

		public AutoFilterInfo AutoFilterInfo { get; set; }

		public Dictionary<FloatingShapeBase, TwoCellAnchorInfo> ShapesAnchorsWaitingSize
		{
			get
			{
				return this.shapesIndices;
			}
		}

		public IXlsxWorkbookImportContext WorkbookContext
		{
			get
			{
				return this.workbookContext;
			}
		}

		public ResourceManager Resources
		{
			get
			{
				return this.workbookContext.Resources;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.workbookContext.Theme;
			}
			set
			{
				this.workbookContext.Theme = value;
			}
		}

		public bool IsImportSuspended
		{
			get
			{
				return false;
			}
		}

		public void RegisterSharedFormula(int index, SharedFormulaInfo sharedFormula)
		{
			this.sharedFormulas[index] = sharedFormula;
		}

		public SharedFormulaInfo GetSharedFormula(int index)
		{
			SharedFormulaInfo result;
			if (this.sharedFormulas.TryGetValue(index, out result))
			{
				return result;
			}
			return null;
		}

		public void ApplyRowInfo(RowInfo rowInfo)
		{
			Guard.ThrowExceptionIfNull<RowInfo>(rowInfo, "rowInfo");
			int row = this.referenceCounter.GetRow(rowInfo);
			if (rowInfo.Hidden)
			{
				base.Worksheet.Rows[row].SetHidden(rowInfo.Hidden);
			}
			if (rowInfo.Height != null)
			{
				RowHeight height = new RowHeight(rowInfo.Height.Value, rowInfo.IsCustom);
				base.Worksheet.Rows[row].SetHeight(height);
			}
			if (rowInfo.OutlineLevel != 0)
			{
				base.Worksheet.Rows[row].SetOutlineLevel(rowInfo.OutlineLevel);
			}
			if (rowInfo.StyleIndex != this.rowStyleIndexCompressedList.GetDefaultValue())
			{
				this.rowStyleIndexCompressedList.SetValue((long)row, rowInfo.StyleIndex);
			}
		}

		public void ApplyColumnInfo(Range<long, ColumnInfo> columnRange)
		{
			Guard.ThrowExceptionIfNull<Range<long, ColumnInfo>>(columnRange, "columnRange");
			ColumnInfo value = columnRange.Value;
			if (value.OutlineLevel != 0)
			{
				base.Worksheet.Columns[(int)columnRange.Start, (int)columnRange.End].SetOutlineLevel(value.OutlineLevel);
			}
			if (value.Hidden)
			{
				base.Worksheet.Columns[(int)columnRange.Start, (int)columnRange.End].SetHidden(value.Hidden);
			}
			ColumnWidth width = new ColumnWidth(value.Width, value.IsCustom);
			base.Worksheet.Columns[(int)columnRange.Start, (int)columnRange.End].SetWidth(width);
			if (value.StyleIndex != this.columnStyleIndexCompressedList.GetDefaultValue())
			{
				this.columnStyleIndexCompressedList.SetValue(columnRange.Start, columnRange.End, value.StyleIndex);
			}
		}

		public void ApplyCellInfo(CellInfo cellInfo)
		{
			Guard.ThrowExceptionIfNull<CellInfo>(cellInfo, "cellInfo");
			int rowIndex;
			int columnIndex;
			this.referenceCounter.GetCellIndex(cellInfo, out rowIndex, out columnIndex);
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			if (cellInfo.CellValue != null)
			{
				CellsPropertyBag propertyBag = base.Worksheet.Cells.PropertyBag;
				ICompressedList<ICellValue> propertyValueCollection = propertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
				propertyValueCollection.SetValue(index, cellInfo.CellValue);
			}
			this.cellStyleIndexCompressedList.SetValue(index, cellInfo.StyleIndex);
		}

		public void ApplyMergeCellInfo(MergedCellInfo mergeCellInfo)
		{
			if (mergeCellInfo.CellRange.ColumnCount > 1 || mergeCellInfo.CellRange.RowCount > 1)
			{
				base.Worksheet.Cells.MergedCellRanges.AddMergedRange(mergeCellInfo.CellRange);
			}
		}

		public void ApplySheetProtectionInfo(WorksheetProtectionInfo info)
		{
			Guard.ThrowExceptionIfNull<WorksheetProtectionInfo>(info, "sheetProtectionInfo");
			base.Worksheet.ProtectionData.Enforced = info.Enforced;
			base.Worksheet.ProtectionData.AlgorithmName = info.AlgorithmName;
			base.Worksheet.ProtectionData.Hash = info.HashValue;
			base.Worksheet.ProtectionData.Salt = info.SaltValue;
			base.Worksheet.ProtectionData.SpinCount = info.SpinCount;
			base.Worksheet.ProtectionData.Password = info.Password;
			Worksheet worksheet = base.Worksheet;
			bool allowDeleteColumns = !info.DeleteColumns;
			bool allowInsertRows = !info.InsertRows;
			worksheet.ProtectionOptions = new WorksheetProtectionOptions(!info.DeleteRows, allowInsertRows, allowDeleteColumns, !info.InsertColumns, !info.FormatCells, !info.FormatColumns, !info.FormatRows, !info.AutoFilter, !info.Sort);
		}

		public void ApplyDefaultColumnWidth(ColumnWidth width)
		{
			Guard.ThrowExceptionIfNull<ColumnWidth>(width, "width");
			base.Worksheet.Columns.SetDefaultWidth(width);
		}

		public void ApplyDefaultColumnWidthDefaultRowHeight(RowHeight height)
		{
			Guard.ThrowExceptionIfNull<RowHeight>(height, "height");
			base.Worksheet.Rows.SetDefaultHeight(height);
		}

		public void BeginImport()
		{
			throw new NotImplementedException();
		}

		public void EndImport()
		{
			throw new NotImplementedException();
		}

		public void EndImport(XlsxWorkbookImportContext workbookContext)
		{
			Guard.ThrowExceptionIfNull<XlsxWorkbookImportContext>(workbookContext, "workbookContext");
			foreach (Range<long, int> range in this.cellStyleIndexCompressedList.GetNonDefaultRanges())
			{
				FormattingRecord directFormatting = workbookContext.StyleSheet.DirectFormattingTable[range.Value];
				this.ImportDirectFormattingInRange(workbookContext, directFormatting, base.Worksheet.Cells.PropertyBag, range.Start, range.End);
			}
			foreach (Range<long, int> range2 in this.columnStyleIndexCompressedList.GetNonDefaultRanges())
			{
				FormattingRecord directFormatting2 = workbookContext.StyleSheet.DirectFormattingTable[range2.Value];
				this.ImportDirectFormattingInRange(workbookContext, directFormatting2, base.Worksheet.Columns.PropertyBag, range2.Start, range2.End);
			}
			foreach (Range<long, int> range3 in this.rowStyleIndexCompressedList.GetNonDefaultRanges())
			{
				FormattingRecord directFormatting3 = workbookContext.StyleSheet.DirectFormattingTable[range3.Value];
				this.ImportDirectFormattingInRange(workbookContext, directFormatting3, base.Worksheet.Rows.PropertyBag, range3.Start, range3.End);
			}
			this.ImportShapesSizes();
			this.ApplyAutoFilter();
		}

		public void RegisterResource(string relationshipId, IResource resource)
		{
			this.workbookContext.RegisterResource(relationshipId, resource);
		}

		public ChartShape GetChartForChartPart(ChartPart chartPart)
		{
			return this.workbookContext.GetChartForChartPart(chartPart);
		}

		public void RegisterChartPartForChart(ChartShape chart, ChartPart chartPart)
		{
			this.workbookContext.RegisterChartPartForChart(chart, chartPart);
		}

		public FormulaChartData GetFormulaChartData(string formula)
		{
			return this.workbookContext.GetFormulaChartData(formula);
		}

		public void RegisterSeriesGroupAwaitingAxisGroupName(ISupportAxes seriesGroup, int catAxisId, int valAxisId)
		{
			throw new NotImplementedException();
		}

		public void RegisterAxisGroup(AxisGroupName groupName, int thisId, int otherId)
		{
			throw new NotImplementedException();
		}

		public void PairSeriesGroupsWithAxes()
		{
			throw new NotImplementedException();
		}

		public string GetSharedStringValue(int index)
		{
			SharedStringType type = this.workbookContext.SharedStrings[index].Type;
			string result = string.Empty;
			switch (type)
			{
			case SharedStringType.Text:
			{
				TextSharedString textSharedString = this.workbookContext.SharedStrings[index] as TextSharedString;
				if (textSharedString != null)
				{
					result = textSharedString.Text;
				}
				break;
			}
			case SharedStringType.RichText:
			{
				RichTextRunSharedString richTextRunSharedString = this.workbookContext.SharedStrings[index] as RichTextRunSharedString;
				if (richTextRunSharedString != null)
				{
					result = richTextRunSharedString.Text;
				}
				break;
			}
			default:
				result = string.Empty;
				break;
			}
			return result;
		}

		public IResource GetResourceByResourceKey(string relationshipId)
		{
			return this.workbookContext.GetResourceByResourceKey(relationshipId);
		}

		public void ApplyPrintOptions(PrintOptionsInfo printOptionsInfo)
		{
			Guard.ThrowExceptionIfNull<PrintOptionsInfo>(printOptionsInfo, "printOptionsInfo");
			WorksheetPageSetup worksheetPageSetup = base.Worksheet.WorksheetPageSetup;
			worksheetPageSetup.CenterHorizontally = printOptionsInfo.HorizontalCentered;
			worksheetPageSetup.CenterVertically = printOptionsInfo.VerticalCentered;
			worksheetPageSetup.PrintOptions.PrintGridlines = printOptionsInfo.GridLines;
			worksheetPageSetup.PrintOptions.PrintRowColumnHeadings = printOptionsInfo.Headings;
		}

		public void ApplyPageMargins(PageMarginsInfo pageMarginsInfo)
		{
			Guard.ThrowExceptionIfNull<PageMarginsInfo>(pageMarginsInfo, "pageMarginsInfo");
			base.Worksheet.WorksheetPageSetup.Margins = new PageMargins(UnitHelper.InchToDip(pageMarginsInfo.Left), UnitHelper.InchToDip(pageMarginsInfo.Top), UnitHelper.InchToDip(pageMarginsInfo.Right), UnitHelper.InchToDip(pageMarginsInfo.Bottom), UnitHelper.InchToDip(pageMarginsInfo.Header), UnitHelper.InchToDip(pageMarginsInfo.Footer));
		}

		public void ApplyPageSetup(PageSetupInfo pageSetupInfo)
		{
			Guard.ThrowExceptionIfNull<PageSetupInfo>(pageSetupInfo, "pageSetupInfo");
			base.Worksheet.WorksheetPageSetup.PaperType = pageSetupInfo.PaperType;
			base.Worksheet.WorksheetPageSetup.PageOrientation = pageSetupInfo.PageOrientation;
			base.Worksheet.WorksheetPageSetup.PageOrder = pageSetupInfo.PageOrder;
			base.Worksheet.WorksheetPageSetup.PrintOptions.ErrorsPrintStyle = pageSetupInfo.Errors;
			base.Worksheet.WorksheetPageSetup.FirstPageNumber = pageSetupInfo.FirstPageNumber;
			base.Worksheet.WorksheetPageSetup.FitToPagesTall = pageSetupInfo.FitToHeight;
			base.Worksheet.WorksheetPageSetup.FitToPagesWide = pageSetupInfo.FitToWidth;
			base.Worksheet.WorksheetPageSetup.ScaleFactor = SpreadsheetHelper.ScaleFactorFromPercent(new int?(pageSetupInfo.Scale));
		}

		public void ApplyHorizontalPageBreakInfo(PageBreakInfo pageBreakInfo)
		{
			Guard.ThrowExceptionIfNull<PageBreakInfo>(pageBreakInfo, "pageBreakInfo");
			base.Worksheet.WorksheetPageSetup.PageBreaks.AddPageBreakInternal(new PageBreak(PageBreakType.Horizontal, pageBreakInfo.Id, pageBreakInfo.Min, pageBreakInfo.Max));
		}

		public void ApplyVerticalPageBreakInfo(PageBreakInfo pageBreakInfo)
		{
			Guard.ThrowExceptionIfNull<PageBreakInfo>(pageBreakInfo, "pageBreakInfo");
			base.Worksheet.WorksheetPageSetup.PageBreaks.AddPageBreakInternal(new PageBreak(PageBreakType.Vertical, pageBreakInfo.Id, pageBreakInfo.Min, pageBreakInfo.Max));
		}

		public void ApplyAutoFilter()
		{
			if (this.AutoFilterInfo == null)
			{
				return;
			}
			base.Worksheet.Filter.FilterRange = this.AutoFilterInfo.Range;
			if (this.AutoFilterInfo.FilterColumnInfos == null)
			{
				return;
			}
			for (int i = 0; i < this.AutoFilterInfo.FilterColumnInfos.Count; i++)
			{
				FilterColumnInfo filterColumnInfo = this.AutoFilterInfo.FilterColumnInfos[i];
				if (filterColumnInfo != null)
				{
					IFilter filter = this.GetFilter(filterColumnInfo);
					if (filter != null)
					{
						base.Worksheet.Filter.SetFilter(filter);
					}
				}
			}
		}

		public void ApplySortState(SortStateInfo sortStateInfo)
		{
			Guard.ThrowExceptionIfNull<SortStateInfo>(sortStateInfo, "sortStateInfo");
			List<ISortCondition> list = new List<ISortCondition>();
			for (int i = 0; i < sortStateInfo.Conditions.Count; i++)
			{
				ISortCondition sortCondition = this.GetSortCondition(sortStateInfo.Range, sortStateInfo.Conditions[i]);
				if (sortCondition != null)
				{
					list.Add(sortCondition);
				}
			}
			if (list.Count > 0)
			{
				base.Worksheet.SortState.SetInternal(sortStateInfo.Range, list.ToArray());
			}
		}

		ISortCondition GetSortCondition(CellRange sortRange, SortConditionInfo conditionInfo)
		{
			Guard.ThrowExceptionIfNull<SortConditionInfo>(conditionInfo, "conditionInfo");
			int relativeIndex = conditionInfo.Range.FromIndex.ColumnIndex - sortRange.FromIndex.ColumnIndex;
			SortOrder sortOrder = (conditionInfo.Descending ? SortOrder.Descending : SortOrder.Ascending);
			if (!(conditionInfo.SortBy == SortBy.Value))
			{
				if (conditionInfo.SortBy == SortBy.CellColor)
				{
					IFill fillById = this.WorkbookContext.DifferentialFormatsContext.GetFillById(conditionInfo.DxfId.Value);
					if (fillById != null)
					{
						return new FillColorSortCondition(relativeIndex, fillById, sortOrder);
					}
				}
				if (conditionInfo.SortBy == SortBy.FontColor)
				{
					ThemableColor colorById = this.WorkbookContext.DifferentialFormatsContext.GetColorById(conditionInfo.DxfId.Value);
					if (colorById != null)
					{
						return new ForeColorSortCondition(relativeIndex, colorById, sortOrder);
					}
				}
				return null;
			}
			if (!string.IsNullOrEmpty(conditionInfo.CustomList))
			{
				string[] customList = conditionInfo.CustomList.Split(new char[] { ","[0] });
				return new CustomValuesSortCondition(relativeIndex, customList, SortOrder.Ascending);
			}
			return new ValuesSortCondition(relativeIndex, sortOrder);
		}

		IFilter GetFilter(FilterColumnInfo filterColumnInfo)
		{
			Guard.ThrowExceptionIfNull<FilterColumnInfo>(filterColumnInfo, "filterColumnInfo");
			FiltersInfo filtersInfo = filterColumnInfo.FilterInfo as FiltersInfo;
			if (filtersInfo != null)
			{
				return this.GetValuesCollectionFilter(filterColumnInfo.ColumnId, filtersInfo);
			}
			CustomFiltersInfo customFiltersInfo = filterColumnInfo.FilterInfo as CustomFiltersInfo;
			if (customFiltersInfo != null)
			{
				return this.GetCustomFilter(filterColumnInfo.ColumnId, customFiltersInfo);
			}
			DynamicFilterInfo dynamicFilterInfo = filterColumnInfo.FilterInfo as DynamicFilterInfo;
			if (dynamicFilterInfo != null)
			{
				return this.GetDynamicFilter(filterColumnInfo.ColumnId, dynamicFilterInfo);
			}
			Top10FilterInfo top10FilterInfo = filterColumnInfo.FilterInfo as Top10FilterInfo;
			if (top10FilterInfo != null)
			{
				return this.GetTop10Filter(filterColumnInfo.ColumnId, top10FilterInfo);
			}
			ColorFilterInfo colorFilterInfo = filterColumnInfo.FilterInfo as ColorFilterInfo;
			if (colorFilterInfo != null)
			{
				return this.GetColorFilter(filterColumnInfo.ColumnId, colorFilterInfo);
			}
			return null;
		}

		ValuesCollectionFilter GetValuesCollectionFilter(int columnId, FiltersInfo filtersInfo)
		{
			Guard.ThrowExceptionIfNull<FiltersInfo>(filtersInfo, "filtersInfo");
			List<DateGroupItem> list = new List<DateGroupItem>();
			for (int i = 0; i < filtersInfo.DateFilters.Count; i++)
			{
				DateGroupItemInfo dateGroupItemInfo = filtersInfo.DateFilters[i];
				DateGroupItem item = new DateGroupItem(dateGroupItemInfo.DateTimeGroupingType, dateGroupItemInfo.Year, dateGroupItemInfo.Month, dateGroupItemInfo.Day, dateGroupItemInfo.Hour, dateGroupItemInfo.Minute, dateGroupItemInfo.Second);
				list.Add(item);
			}
			return new ValuesCollectionFilter(columnId, filtersInfo.StringFilters, list, filtersInfo.Blank);
		}

		CustomFilter GetCustomFilter(int columnId, CustomFiltersInfo customFiltersInfo)
		{
			Guard.ThrowExceptionIfNull<CustomFiltersInfo>(customFiltersInfo, "customFiltersInfo");
			CustomFilterCriteriaInfo customFilterCriteriaInfo = customFiltersInfo.CustomFilters[0];
			if (customFiltersInfo.CustomFilters.Count == 1)
			{
				return new CustomFilter(columnId, customFilterCriteriaInfo.ToCriteria());
			}
			LogicalOperator logicalOperator = (customFiltersInfo.IsAnd ? LogicalOperator.And : LogicalOperator.Or);
			CustomFilterCriteriaInfo customFilterCriteriaInfo2 = customFiltersInfo.CustomFilters[1];
			return new CustomFilter(columnId, customFilterCriteriaInfo.ToCriteria(), logicalOperator, customFilterCriteriaInfo2.ToCriteria());
		}

		DynamicFilter GetDynamicFilter(int columnId, DynamicFilterInfo dynamicFilterInfo)
		{
			Guard.ThrowExceptionIfNull<DynamicFilterInfo>(dynamicFilterInfo, "dynamicFilterInfo");
			return new DynamicFilter(columnId, dynamicFilterInfo.DynamicFilterType);
		}

		TopFilter GetTop10Filter(int columnId, Top10FilterInfo top10FilterInfo)
		{
			Guard.ThrowExceptionIfNull<Top10FilterInfo>(top10FilterInfo, "top10FilterInfo");
			TopFilterType topFilterType;
			if (top10FilterInfo.Top)
			{
				topFilterType = (top10FilterInfo.Percent ? TopFilterType.TopPercent : TopFilterType.TopNumber);
			}
			else
			{
				topFilterType = (top10FilterInfo.Percent ? TopFilterType.BottomPercent : TopFilterType.BottomNumber);
			}
			return new TopFilter(columnId, topFilterType, top10FilterInfo.Value);
		}

		IFilter GetColorFilter(int columnId, ColorFilterInfo colorFilterInfo)
		{
			IFill fillById = this.WorkbookContext.DifferentialFormatsContext.GetFillById(colorFilterInfo.DxfId);
			if (fillById != null)
			{
				if (colorFilterInfo.CellColor)
				{
					return new FillColorFilter(columnId, fillById);
				}
				PatternFill patternFill = fillById as PatternFill;
				if (patternFill != null)
				{
					return new ForeColorFilter(columnId, patternFill.PatternColor);
				}
			}
			return null;
		}

		void ImportShapesSizes()
		{
			if (this.ShapesAnchorsWaitingSize.Keys.Count == 0)
			{
				return;
			}
			RadWorksheetLayout worksheetLayout = base.Worksheet.Workbook.GetWorksheetLayout(base.Worksheet, false);
			foreach (FloatingShapeBase floatingShapeBase in this.ShapesAnchorsWaitingSize.Keys)
			{
				if (base.Worksheet.Shapes.Contains(floatingShapeBase))
				{
					TwoCellAnchorInfo twoCellAnchorInfo = this.ShapesAnchorsWaitingSize[floatingShapeBase];
					Point topLeftPointFromCellIndex = worksheetLayout.GetTopLeftPointFromCellIndex(twoCellAnchorInfo.FromIndex);
					Point topLeftPointFromCellIndex2 = worksheetLayout.GetTopLeftPointFromCellIndex(twoCellAnchorInfo.ToIndex);
					double num = SpreadsheetHelper.RestrictRotationAngle(floatingShapeBase.RotationAngle);
					bool flag = (num >= 45.0 && num < 135.0) || (num > 225.0 && num < 315.0);
					double width;
					double height;
					if (flag)
					{
						width = topLeftPointFromCellIndex2.Y + twoCellAnchorInfo.ToOffsetY - (topLeftPointFromCellIndex.Y + twoCellAnchorInfo.FromOffsetY);
						height = topLeftPointFromCellIndex2.X + twoCellAnchorInfo.ToOffsetX - (topLeftPointFromCellIndex.X + twoCellAnchorInfo.FromOffsetX);
					}
					else
					{
						width = topLeftPointFromCellIndex2.X + twoCellAnchorInfo.ToOffsetX - (topLeftPointFromCellIndex.X + twoCellAnchorInfo.FromOffsetX);
						height = topLeftPointFromCellIndex2.Y + twoCellAnchorInfo.ToOffsetY - (topLeftPointFromCellIndex.Y + twoCellAnchorInfo.FromOffsetY);
					}
					floatingShapeBase.Width = width;
					floatingShapeBase.Height = height;
				}
			}
		}

		void ImportDirectFormattingInRange(XlsxWorkbookImportContext workbookContext, FormattingRecord directFormatting, PropertyBagBase propertyBag, long start, long end)
		{
			if (directFormatting.FillId != null)
			{
				IFill fill = workbookContext.StyleSheet.FillTable[directFormatting.FillId.Value];
				if (!(fill is NoneFill))
				{
					this.UpdatePropertyInRangeIfNotNull<IFill>(propertyBag, CellPropertyDefinitions.FillProperty, start, end, fill);
				}
			}
			if (directFormatting.NumberFormatId != null)
			{
				CellValueFormat value = workbookContext.StyleSheet.CellValueFormatTable[directFormatting.NumberFormatId.Value];
				this.UpdatePropertyInRangeIfNotNull<CellValueFormat>(propertyBag, CellPropertyDefinitions.FormatProperty, start, end, value);
			}
			if (directFormatting.FontInfoId != null)
			{
				FontInfo fontInfo = workbookContext.StyleSheet.FontInfoTable[directFormatting.FontInfoId.Value];
				this.ImportFontInRange(propertyBag, fontInfo, start, end);
			}
			if (directFormatting.BordersInfoId != null)
			{
				BordersInfo bordersInfo = workbookContext.StyleSheet.BordersInfoTable[directFormatting.BordersInfoId.Value];
				this.ImportBordersInRange(propertyBag, bordersInfo, start, end);
			}
			this.UpdatePropertyInRangeIfNotNull<RadHorizontalAlignment>(propertyBag, CellPropertyDefinitions.HorizontalAlignmentProperty, start, end, directFormatting.HorizontalAlignment);
			this.UpdatePropertyInRangeIfNotNull<RadVerticalAlignment>(propertyBag, CellPropertyDefinitions.VerticalAlignmentProperty, start, end, directFormatting.VerticalAlignment);
			this.UpdatePropertyInRangeIfNotNull<int>(propertyBag, CellPropertyDefinitions.IndentProperty, start, end, directFormatting.Indent);
			this.UpdatePropertyInRangeIfNotNull<bool>(propertyBag, CellPropertyDefinitions.IsWrappedProperty, start, end, directFormatting.WrapText);
			this.UpdatePropertyInRangeIfNotNull<bool>(propertyBag, CellPropertyDefinitions.IsLockedProperty, start, end, directFormatting.IsLocked);
			if (directFormatting.StyleFormattingRecordId != null)
			{
				StyleInfo? styleInfoByStyleFormattingRecordId = workbookContext.StyleSheet.GetStyleInfoByStyleFormattingRecordId(directFormatting.StyleFormattingRecordId.Value);
				if (styleInfoByStyleFormattingRecordId != null)
				{
					this.UpdatePropertyInRangeIfNotNull<string>(propertyBag, CellPropertyDefinitions.StyleNameProperty, start, end, styleInfoByStyleFormattingRecordId.Value.Name);
				}
			}
		}

		void ImportFontInRange(PropertyBagBase propertyBag, FontInfo fontInfo, long start, long end)
		{
			Guard.ThrowExceptionIfNull<FontInfo>(fontInfo, "fontInfo");
			this.UpdatePropertyInRangeIfNotNull<bool>(propertyBag, CellPropertyDefinitions.IsBoldProperty, start, end, fontInfo.Bold);
			this.UpdatePropertyInRangeIfNotNull<bool>(propertyBag, CellPropertyDefinitions.IsItalicProperty, start, end, fontInfo.Italic);
			this.UpdatePropertyInRangeIfNotNull<double>(propertyBag, CellPropertyDefinitions.FontSizeProperty, start, end, fontInfo.FontSize);
			this.UpdatePropertyInRangeIfNotNull<ThemableFontFamily>(propertyBag, CellPropertyDefinitions.FontFamilyProperty, start, end, fontInfo.FontFamily);
			this.UpdatePropertyInRangeIfNotNull<ThemableColor>(propertyBag, CellPropertyDefinitions.ForeColorProperty, start, end, fontInfo.ForeColor);
			this.UpdatePropertyInRangeIfNotNull<UnderlineType>(propertyBag, CellPropertyDefinitions.UnderlineProperty, start, end, fontInfo.UnderlineType);
		}

		void ImportBordersInRange(PropertyBagBase propertyBag, BordersInfo bordersInfo, long start, long end)
		{
			Guard.ThrowExceptionIfNull<BordersInfo>(bordersInfo, "bordersInfo");
			this.UpdatePropertyInRangeIfNotNull<CellBorder>(propertyBag, CellPropertyDefinitions.LeftBorderProperty, start, end, bordersInfo.Left);
			this.UpdatePropertyInRangeIfNotNull<CellBorder>(propertyBag, CellPropertyDefinitions.TopBorderProperty, start, end, bordersInfo.Top);
			this.UpdatePropertyInRangeIfNotNull<CellBorder>(propertyBag, CellPropertyDefinitions.RightBorderProperty, start, end, bordersInfo.Right);
			this.UpdatePropertyInRangeIfNotNull<CellBorder>(propertyBag, CellPropertyDefinitions.BottomBorderProperty, start, end, bordersInfo.Bottom);
			this.UpdatePropertyInRangeIfNotNull<CellBorder>(propertyBag, CellPropertyDefinitions.DiagonalUpBorderProperty, start, end, bordersInfo.DiagonalUp);
			this.UpdatePropertyInRangeIfNotNull<CellBorder>(propertyBag, CellPropertyDefinitions.DiagonalDownBorderProperty, start, end, bordersInfo.DiagonalDown);
		}

		void UpdatePropertyInRangeIfNotNull<T>(PropertyBagBase propertyBag, IPropertyDefinition<T> propertyDefinition, long start, long end, T? value) where T : struct
		{
			if (value != null)
			{
				this.UpdatePropertyInRangeInternal<T>(propertyBag, propertyDefinition, start, end, value.Value);
			}
		}

		void UpdatePropertyInRangeIfNotNull<T>(PropertyBagBase propertyBag, IPropertyDefinition<T> propertyDefinition, long start, long end, T value) where T : class
		{
			if (value != null)
			{
				this.UpdatePropertyInRangeInternal<T>(propertyBag, propertyDefinition, start, end, value);
			}
		}

		void UpdatePropertyInRangeInternal<T>(PropertyBagBase propertyBag, IPropertyDefinition<T> propertyDefinition, long start, long end, T value)
		{
			propertyBag.GetPropertyValueCollection<T>(propertyDefinition).SetValue(start, end, value);
		}

		readonly ICompressedList<int> cellStyleIndexCompressedList;

		readonly ICompressedList<int> rowStyleIndexCompressedList;

		readonly ICompressedList<int> columnStyleIndexCompressedList;

		readonly XlsxWorkbookImportContext workbookContext;

		readonly Dictionary<int, SharedFormulaInfo> sharedFormulas;

		readonly Dictionary<FloatingShapeBase, TwoCellAnchorInfo> shapesIndices;

		readonly ReferenceCounter referenceCounter;
	}
}
