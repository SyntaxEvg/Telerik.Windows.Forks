using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class XlsxWorksheetExportContext : XlsxWorksheetContextBase, IXlsxWorksheetExportContext, IOpenXmlExportContext
	{
		public XlsxWorksheetExportContext(XlsxWorkbookExportContext workbookContext, Worksheet worksheet, int sheetNo)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<XlsxWorkbookExportContext>(workbookContext, "workbookContext");
			this.sheetNo = sheetNo;
			this.workbookContext = workbookContext;
			this.worksheetEntityToFontInfoCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<FontInfo>>();
			this.worksheetEntityToFontInfoCompressedList[base.Worksheet.Cells] = new CompressedList<FontInfo>(0L, SpreadsheetDefaultValues.CellCount - 1L, FontInfo.GetDefaultValue());
			this.worksheetEntityToFontInfoCompressedList[base.Worksheet.Rows] = new CompressedList<FontInfo>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), FontInfo.GetDefaultValue());
			this.worksheetEntityToFontInfoCompressedList[base.Worksheet.Columns] = new CompressedList<FontInfo>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1), FontInfo.GetDefaultValue());
			this.worksheetEntityToBordersInfoCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<BordersInfo>>();
			this.worksheetEntityToBordersInfoCompressedList[base.Worksheet.Cells] = new CompressedList<BordersInfo>(0L, SpreadsheetDefaultValues.CellCount - 1L, BordersInfo.GetDefaultValue());
			this.worksheetEntityToBordersInfoCompressedList[base.Worksheet.Rows] = new CompressedList<BordersInfo>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), BordersInfo.GetDefaultValue());
			this.worksheetEntityToBordersInfoCompressedList[base.Worksheet.Columns] = new CompressedList<BordersInfo>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1), BordersInfo.GetDefaultValue());
			this.worksheetEntityToStyleNameCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<string>>();
			this.worksheetEntityToFillCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<IFill>>();
			this.worksheetEntityToCellValueFormatCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<CellValueFormat>>();
			this.worksheetEntityToHorizontalAlignmentCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<RadHorizontalAlignment>>();
			this.worksheetEntityToVerticalAlignmentCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<RadVerticalAlignment>>();
			this.worksheetEntityToIndentCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<int>>();
			this.worksheetEntityToIsWrappedCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<bool>>();
			this.worksheetEntityToIsLockedCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<bool>>();
			this.worksheetEntityToNonDefaultFormattingRecordIndex = new Dictionary<WorksheetEntityBase, ICompressedList<bool>>();
			this.worksheetEntityToNonDefaultFormattingRecordIndex[base.Worksheet.Cells] = new CompressedList<bool>(0L, SpreadsheetDefaultValues.CellCount - 1L, false);
			this.worksheetEntityToNonDefaultFormattingRecordIndex[base.Worksheet.Rows] = new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), false);
			this.worksheetEntityToNonDefaultFormattingRecordIndex[base.Worksheet.Columns] = new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1), false);
			this.worksheetEntryToDirectFormattingCompressedList = new Dictionary<WorksheetEntityBase, ICompressedList<FormattingRecord>>();
			this.worksheetEntryToDirectFormattingCompressedList[base.Worksheet.Cells] = new CompressedList<FormattingRecord>(0L, SpreadsheetDefaultValues.CellCount - 1L, FormattingRecord.Empty);
			this.worksheetEntryToDirectFormattingCompressedList[base.Worksheet.Rows] = new CompressedList<FormattingRecord>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), FormattingRecord.Empty);
			this.worksheetEntryToDirectFormattingCompressedList[base.Worksheet.Columns] = new CompressedList<FormattingRecord>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1), FormattingRecord.Empty);
			this.directFormattingPropertyDataInfo = new CellPropertyDataInfo();
			ColumnInfo defaultValue = new ColumnInfo(XlsxHelper.ConvertColumnPixelWidthToExcelWidth(base.Worksheet.Workbook, SpreadsheetDefaultValues.DefaultColumnWidth));
			this.columnInfoCompressedList = new CompressedList<ColumnInfo>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1), defaultValue);
			this.rowPropertyDataInfosToRespect = new List<CellPropertyDataInfo>();
			this.rowPropertyDataInfosToRespect.Add(WorksheetExportContext.GetValuePropertyDataInfo(base.Worksheet));
			this.rowPropertyDataInfosToRespect.Add(this.directFormattingPropertyDataInfo);
			this.pictureToRelationshipId = new Dictionary<Image, string>();
			this.dataValidationRuleToCellRanges = new Dictionary<IDataValidationRule, IEnumerable<CellRange>>();
			this.SetRowAndColumnPropertiesAsLocalInCellsWhereCellsHasValueProperty();
			this.InitializePropertyToIsDefault();
			this.Initialize();
		}

		public ResourceManager Resources
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IXlsxWorkbookExportContext WorkbookContext
		{
			get
			{
				return this.workbookContext;
			}
		}

		public int SheetNo
		{
			get
			{
				return this.sheetNo;
			}
		}

		public string WorksheetName
		{
			get
			{
				return base.Worksheet.Name;
			}
		}

		public IEnumerable<SpreadsheetHyperlink> Hyperlinks
		{
			get
			{
				return base.Worksheet.Hyperlinks;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<string>> StyleNameCompressedList
		{
			get
			{
				return this.worksheetEntityToStyleNameCompressedList;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<IFill>> FillCompressedList
		{
			get
			{
				return this.worksheetEntityToFillCompressedList;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<CellValueFormat>> CellValueFormatCompressedList
		{
			get
			{
				return this.worksheetEntityToCellValueFormatCompressedList;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<FontInfo>> FontInfoCompressedList
		{
			get
			{
				return this.worksheetEntityToFontInfoCompressedList;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<BordersInfo>> BordersInfoCompressedList
		{
			get
			{
				return this.worksheetEntityToBordersInfoCompressedList;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<FormattingRecord>> DirectFormattingCompressedList
		{
			get
			{
				return this.worksheetEntryToDirectFormattingCompressedList;
			}
		}

		public Dictionary<WorksheetEntityBase, ICompressedList<bool>> NonDefaultFormattingCompressedList
		{
			get
			{
				return this.worksheetEntityToNonDefaultFormattingRecordIndex;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.WorkbookContext.Theme;
			}
		}

		public IEnumerable<FloatingShapeBase> Shapes
		{
			get
			{
				return base.Worksheet.Shapes;
			}
		}

		public IEnumerable<FloatingChartShape> Charts
		{
			get
			{
				return base.Worksheet.Shapes.Charts;
			}
		}

		public WorksheetViewState WorksheetViewState
		{
			get
			{
				return (WorksheetViewState)((ISheet)base.Worksheet).ViewState;
			}
		}

		public void InitDirectFormattingRecords()
		{
			if (this.areDirectFormattingRecordsInitialized)
			{
				return;
			}
			foreach (WorksheetEntityBase tableEntity in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.InitDirectFormattingRecords(tableEntity);
			}
			WorksheetExportContext.CalculateCellPropertyPropertyUsedValueRanges<bool>(this.directFormattingPropertyDataInfo, this.NonDefaultFormattingCompressedList[base.Worksheet.Cells], true);
			this.areDirectFormattingRecordsInitialized = true;
		}

		public void InitDirectFormattingRecords(WorksheetEntityBase tableEntity)
		{
			this.UpdateProperty<IFill, FormattingRecord>(tableEntity, this.worksheetEntityToFillCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, IFill value)
			{
				formattingRecord.FillId = new int?(this.workbookContext.StyleSheet.FillTable.GetIndex(value));
				return formattingRecord;
			});
			this.UpdateProperty<CellValueFormat, FormattingRecord>(tableEntity, this.worksheetEntityToCellValueFormatCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, CellValueFormat value)
			{
				formattingRecord.NumberFormatId = new int?(this.workbookContext.StyleSheet.CellValueFormatTable.GetIndex(value));
				return formattingRecord;
			});
			this.UpdateProperty<FontInfo, FormattingRecord>(tableEntity, this.worksheetEntityToFontInfoCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, FontInfo value)
			{
				formattingRecord.FontInfoId = new int?(this.workbookContext.StyleSheet.FontInfoTable.GetIndex(value));
				return formattingRecord;
			});
			this.UpdateProperty<BordersInfo, FormattingRecord>(tableEntity, this.worksheetEntityToBordersInfoCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, BordersInfo value)
			{
				formattingRecord.BordersInfoId = new int?(this.workbookContext.StyleSheet.BordersInfoTable.GetIndex(value));
				return formattingRecord;
			});
			this.UpdateProperty<RadVerticalAlignment, FormattingRecord>(tableEntity, this.worksheetEntityToVerticalAlignmentCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, RadVerticalAlignment value)
			{
				formattingRecord.VerticalAlignment = new RadVerticalAlignment?(value);
				return formattingRecord;
			});
			this.UpdateProperty<RadHorizontalAlignment, FormattingRecord>(tableEntity, this.worksheetEntityToHorizontalAlignmentCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, RadHorizontalAlignment value)
			{
				formattingRecord.HorizontalAlignment = new RadHorizontalAlignment?(value);
				return formattingRecord;
			});
			this.UpdateProperty<int, FormattingRecord>(tableEntity, this.worksheetEntityToIndentCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, int value)
			{
				formattingRecord.Indent = new int?(value);
				return formattingRecord;
			});
			this.UpdateProperty<bool, FormattingRecord>(tableEntity, this.worksheetEntityToIsWrappedCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, bool value)
			{
				formattingRecord.WrapText = new bool?(value);
				return formattingRecord;
			});
			this.UpdateProperty<bool, FormattingRecord>(tableEntity, this.worksheetEntityToIsLockedCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, bool value)
			{
				formattingRecord.IsLocked = new bool?(value);
				return formattingRecord;
			});
			this.UpdateProperty<string, FormattingRecord>(tableEntity, this.worksheetEntityToStyleNameCompressedList[tableEntity], this.worksheetEntryToDirectFormattingCompressedList[tableEntity], delegate(FormattingRecord formattingRecord, string value)
			{
				formattingRecord.StyleFormattingRecordId = new int?(this.workbookContext.GetStyleFormattingRecordId(value));
				return formattingRecord;
			});
		}

		public int GetSharedStringIndex(TextCellValue textValue)
		{
			return this.workbookContext.SharedStrings.GetIndex(new TextSharedString(textValue.Value));
		}

		public string GetPictureRelationshipId(Image picture)
		{
			return this.pictureToRelationshipId[picture];
		}

		public bool ContainsNonDefaultFormattingRecordIndex(WorksheetEntityBase worksheetEntity, long index)
		{
			return this.worksheetEntityToNonDefaultFormattingRecordIndex[worksheetEntity].GetValue(index);
		}

		public int? GetFormattingRecordIndex(WorksheetEntityBase tableEntity, long index)
		{
			int? result = null;
			FormattingRecord value = this.worksheetEntryToDirectFormattingCompressedList[tableEntity].GetValue(index);
			if (!value.IsEmpty())
			{
				result = new int?(this.workbookContext.StyleSheet.DirectFormattingTable.GetIndex(value));
			}
			return result;
		}

		public string GetRelationshipIdByResource(IResource resource)
		{
			throw new NotImplementedException();
		}

		public void RegisterResource(string relationshipId, IResource resource)
		{
			throw new NotImplementedException();
		}

		public ChartPart GetChartPartForChart(ChartShape chart)
		{
			return this.workbookContext.GetChartPartForChart(chart);
		}

		public ChartShape GetChartForChartPart(ChartPart chartPart)
		{
			return this.workbookContext.GetChartForChartPart(chartPart);
		}

		public IEnumerable<RowInfo> GetNonEmptyRows()
		{
			Rows rows = base.Worksheet.Rows;
			CompressedList<bool> usedRows = new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), false);
			foreach (IProperty property in rows.Properties)
			{
				ICompressedList propertyValueCollection = rows.PropertyBag.GetPropertyValueCollection(property.PropertyDefinition);
				IEnumerable<LongRange> ranges = propertyValueCollection.GetRanges(false);
				foreach (LongRange longRange in ranges)
				{
					usedRows.SetValue(longRange.Start, longRange.End, true);
				}
			}
			CellRange usedCellRange = this.GetUsedCellRange();
			int startRowIndex = usedCellRange.FromIndex.RowIndex;
			int endRowIndex = usedCellRange.ToIndex.RowIndex;
			foreach (IProperty property2 in rows.Properties)
			{
				ICompressedList propertyValueCollection2 = rows.PropertyBag.GetPropertyValueCollection(property2.PropertyDefinition);
				long? lastNonEmptyRangeEnd = propertyValueCollection2.GetLastNonEmptyRangeEnd(0L, (long)(SpreadsheetDefaultValues.RowCount - 1));
				endRowIndex = (int)Math.Max((long)endRowIndex, lastNonEmptyRangeEnd ?? 0L);
			}
			IEnumerable<LongRange> defaultRanges = usedRows.GetRanges(true);
			foreach (LongRange longRange2 in defaultRanges)
			{
				int firstStart = (int)longRange2.Start;
				int firstEnd = (int)longRange2.End;
				Tuple<int, int> intersection = Utils.GetIntersection(firstStart, firstEnd, startRowIndex, endRowIndex);
				if (intersection != null)
				{
					for (int i = intersection.Item1; i <= intersection.Item2; i++)
					{
						foreach (CellPropertyDataInfo cellPropertyDataInfo in this.rowPropertyDataInfosToRespect)
						{
							if (!cellPropertyDataInfo.GetRowIsEmpty(i))
							{
								usedRows.SetValue((long)i, true);
								break;
							}
						}
					}
				}
			}
			foreach (Range<long, bool> range in usedRows.GetNonDefaultRanges())
			{
				int start = (int)range.Start;
				int end = (int)range.End;
				for (int rowIndex = start; rowIndex <= end; rowIndex++)
				{
					yield return new RowInfo(rowIndex, base.Worksheet.Rows);
				}
			}
			yield break;
		}

		public IEnumerable<Range<long, ColumnInfo>> GetNonEmptyColumns()
		{
			foreach (Range<long, ColumnInfo> columnRange in this.columnInfoCompressedList.GetNonDefaultRanges())
			{
				yield return columnRange;
			}
			yield break;
		}

		public IEnumerable<MergedCellInfo> GetMergedCells()
		{
			foreach (CellRange cellRange in base.Worksheet.Cells.MergedCellRanges.Ranges)
			{
				yield return new MergedCellInfo(cellRange);
			}
			yield break;
		}

		public global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts.CellInfo> GetNonEmptyCellsInRow(int rowIndex)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.ICompressedList<bool> usedRangesInRow = new global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.CompressedList<bool>(0L, (long)(global::Telerik.Windows.Documents.Spreadsheet.Model.SpreadsheetDefaultValues.ColumnCount - 1));
			foreach (global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts.CellPropertyDataInfo cellPropertyDataInfo in this.rowPropertyDataInfosToRespect)
			{
				global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.Range rowUsedRange = cellPropertyDataInfo.GetRowUsedRange(rowIndex);
				if (rowUsedRange != null)
				{
					usedRangesInRow.SetValue((long)rowUsedRange.Start, (long)rowUsedRange.End, true);
				}
			}
			global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.ICompressedList<bool> nonDefaultFormattingCompressedList = this.NonDefaultFormattingCompressedList[base.Worksheet.Cells];
			global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.ICompressedList<global::Telerik.Windows.Documents.Spreadsheet.Model.ICellValue> valuesCompressedList = base.Worksheet.Cells.PropertyBag.GetPropertyValueCollection<global::Telerik.Windows.Documents.Spreadsheet.Model.ICellValue>(global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.CellPropertyDefinitions.ValueProperty);
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Core.DataStructures.Range<long, bool> range in usedRangesInRow.GetNonDefaultRanges())
			{
				int columnIndex = (int)range.Start;
				while ((long)columnIndex <= range.End)
				{
					long index = global::Telerik.Windows.Documents.Spreadsheet.PropertySystem.WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
					bool containsFormatting = nonDefaultFormattingCompressedList.ContainsNonDefaultValues(index, index);
					bool containsValues = valuesCompressedList.ContainsNonDefaultValues(index, index);
					if (containsFormatting || containsValues)
					{
						yield return new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts.CellInfo(rowIndex, columnIndex, base.Worksheet.Cells);
					}
					columnIndex++;
				}
			}
			yield break;
		}

		public WorksheetProtectionInfo GetSheetProtectionInfo()
		{
			return new WorksheetProtectionInfo
			{
				Enforced = base.Worksheet.ProtectionData.Enforced,
				AlgorithmName = base.Worksheet.ProtectionData.AlgorithmName,
				HashValue = base.Worksheet.ProtectionData.Hash,
				SaltValue = base.Worksheet.ProtectionData.Salt,
				SpinCount = base.Worksheet.ProtectionData.SpinCount,
				Password = base.Worksheet.ProtectionData.Password,
				InsertColumns = !base.Worksheet.ProtectionOptions.AllowInsertColumns,
				InsertRows = !base.Worksheet.ProtectionOptions.AllowInsertRows,
				DeleteColumns = !base.Worksheet.ProtectionOptions.AllowDeleteColumns,
				DeleteRows = !base.Worksheet.ProtectionOptions.AllowDeleteRows,
				FormatCells = !base.Worksheet.ProtectionOptions.AllowFormatCells,
				FormatColumns = !base.Worksheet.ProtectionOptions.AllowFormatColumns,
				FormatRows = !base.Worksheet.ProtectionOptions.AllowFormatRows,
				AutoFilter = !base.Worksheet.ProtectionOptions.AllowFiltering,
				Sort = !base.Worksheet.ProtectionOptions.AllowSorting
			};
		}

		public PrintOptionsInfo GetPrintOptionsInfo()
		{
			PrintOptionsInfo printOptionsInfo = new PrintOptionsInfo();
			WorksheetPageSetup worksheetPageSetup = base.Worksheet.WorksheetPageSetup;
			printOptionsInfo.HorizontalCentered = worksheetPageSetup.CenterHorizontally;
			printOptionsInfo.VerticalCentered = worksheetPageSetup.CenterVertically;
			printOptionsInfo.Headings = worksheetPageSetup.PrintOptions.PrintRowColumnHeadings;
			printOptionsInfo.GridLines = worksheetPageSetup.PrintOptions.PrintGridlines;
			return printOptionsInfo;
		}

		public PageMarginsInfo GetPageMarginsInfo()
		{
			return new PageMarginsInfo
			{
				Bottom = UnitHelper.DipToInch(base.Worksheet.WorksheetPageSetup.Margins.Bottom),
				Footer = UnitHelper.DipToInch(base.Worksheet.WorksheetPageSetup.Margins.Footer),
				Header = UnitHelper.DipToInch(base.Worksheet.WorksheetPageSetup.Margins.Header),
				Left = UnitHelper.DipToInch(base.Worksheet.WorksheetPageSetup.Margins.Left),
				Right = UnitHelper.DipToInch(base.Worksheet.WorksheetPageSetup.Margins.Right),
				Top = UnitHelper.DipToInch(base.Worksheet.WorksheetPageSetup.Margins.Top)
			};
		}

		public PageSetupInfo GetPageSetupInfo()
		{
			return new PageSetupInfo
			{
				PaperType = base.Worksheet.WorksheetPageSetup.PaperType,
				PageOrientation = base.Worksheet.WorksheetPageSetup.PageOrientation,
				PageOrder = base.Worksheet.WorksheetPageSetup.PageOrder,
				Errors = base.Worksheet.WorksheetPageSetup.PrintOptions.ErrorsPrintStyle,
				FirstPageNumber = base.Worksheet.WorksheetPageSetup.FirstPageNumber,
				FitToHeight = base.Worksheet.WorksheetPageSetup.FitToPagesTall,
				FitToWidth = base.Worksheet.WorksheetPageSetup.FitToPagesWide,
				Scale = SpreadsheetHelper.PercentFromScaleFactor(base.Worksheet.WorksheetPageSetup.ScaleFactor)
			};
		}

		public PageBreaksInfo GetPageBreaksInfo()
		{
			PageBreaksInfo pageBreaksInfo = new PageBreaksInfo();
			List<PageBreakInfo> list = new List<PageBreakInfo>();
			foreach (PageBreak pageBreak in base.Worksheet.WorksheetPageSetup.PageBreaks.SortedHorizontalPageBreaks)
			{
				list.Add(new PageBreakInfo
				{
					Id = pageBreak.Index,
					Manual = true,
					Max = pageBreak.ToIndex,
					Min = pageBreak.FromIndex
				});
			}
			pageBreaksInfo.HorizontalPageBreaks = list;
			List<PageBreakInfo> list2 = new List<PageBreakInfo>();
			foreach (PageBreak pageBreak2 in base.Worksheet.WorksheetPageSetup.PageBreaks.SortedVerticalPageBreaks)
			{
				list2.Add(new PageBreakInfo
				{
					Id = pageBreak2.Index,
					Manual = true,
					Max = pageBreak2.ToIndex,
					Min = pageBreak2.FromIndex
				});
			}
			pageBreaksInfo.VerticalPageBreaks = list2;
			return pageBreaksInfo;
		}

		public CellRefRange GetAutoFilterRange()
		{
			if (base.Worksheet.Filter.FilterRange != null)
			{
				return new CellRefRange(base.Worksheet.Filter.FilterRange);
			}
			return null;
		}

		public IEnumerable<FilterColumnInfo> GetFilterColumnInfos()
		{
			for (int i = 0; i < base.Worksheet.Filter.Filters.Count; i++)
			{
				FilterColumnInfo filterColumnInfo = new FilterColumnInfo(base.Worksheet.Filter.Filters[i].RelativeColumnIndex);
				yield return filterColumnInfo;
			}
			yield break;
		}

		public IFilterInfo GetFilterInfo(int columnId)
		{
			IFilter filter = base.Worksheet.Filter.GetFilter(columnId);
			if (filter is ValuesCollectionFilter)
			{
				return this.GetFiltersInfo(columnId);
			}
			if (filter is CustomFilter)
			{
				return this.GetCustomFilterInfo(columnId);
			}
			if (filter is DynamicFilter)
			{
				return this.GetDynamicFilterInfo(columnId);
			}
			if (filter is TopFilter)
			{
				return this.GetTop10FilterInfo(columnId);
			}
			if (filter is FillColorFilter || filter is ForeColorFilter)
			{
				return this.GetColorFilterInfo(columnId);
			}
			return null;
		}

		public FiltersInfo GetFiltersInfo(int columnId)
		{
			ValuesCollectionFilter valuesCollectionFilter = (ValuesCollectionFilter)base.Worksheet.Filter.GetFilter(columnId);
			FiltersInfo filtersInfo = new FiltersInfo();
			filtersInfo.Blank = valuesCollectionFilter.Blank;
			foreach (string item in valuesCollectionFilter.StringValues)
			{
				filtersInfo.StringFilters.Add(item);
			}
			foreach (DateGroupItem item2 in valuesCollectionFilter.DateItems)
			{
				filtersInfo.DateFilters.Add(new DateGroupItemInfo(item2));
			}
			return filtersInfo;
		}

		public CustomFiltersInfo GetCustomFilterInfo(int columnId)
		{
			CustomFilter customFilter = (CustomFilter)base.Worksheet.Filter.GetFilter(columnId);
			CustomFiltersInfo customFiltersInfo = new CustomFiltersInfo();
			CustomFilterCriteriaInfo item = new CustomFilterCriteriaInfo(new CustomFilterCriteria(customFilter.Criteria1.ComparisonOperator, customFilter.Criteria1.ComparableFilterValue.ToString()));
			customFiltersInfo.CustomFilters.Add(item);
			if (customFilter.Criteria2 != null)
			{
				CustomFilterCriteriaInfo item2 = new CustomFilterCriteriaInfo(new CustomFilterCriteria(customFilter.Criteria2.ComparisonOperator, customFilter.Criteria2.ComparableFilterValue.ToString()));
				customFiltersInfo.CustomFilters.Add(item2);
				customFiltersInfo.IsAnd = customFilter.LogicalOperator == LogicalOperator.And;
			}
			return customFiltersInfo;
		}

		public DynamicFilterInfo GetDynamicFilterInfo(int columnId)
		{
			DynamicFilter dynamicFilter = (DynamicFilter)base.Worksheet.Filter.GetFilter(columnId);
			return new DynamicFilterInfo
			{
				DynamicFilterType = dynamicFilter.DynamicFilterType
			};
		}

		public Top10FilterInfo GetTop10FilterInfo(int columnId)
		{
			TopFilter topFilter = (TopFilter)base.Worksheet.Filter.GetFilter(columnId);
			return new Top10FilterInfo
			{
				Value = topFilter.Value,
				Top = (topFilter.TopFilterType == TopFilterType.TopNumber || topFilter.TopFilterType == TopFilterType.TopPercent),
				Percent = (topFilter.TopFilterType == TopFilterType.BottomPercent || topFilter.TopFilterType == TopFilterType.TopPercent)
			};
		}

		public ColorFilterInfo GetColorFilterInfo(int columnId)
		{
			ColorFilterInfo colorFilterInfo = new ColorFilterInfo();
			FillColorFilter fillColorFilter = base.Worksheet.Filter.GetFilter(columnId) as FillColorFilter;
			if (fillColorFilter != null)
			{
				colorFilterInfo.CellColor = true;
				colorFilterInfo.DxfId = this.WorkbookContext.DifferentialFormatsContext.RegisterFormatAndGetId(fillColorFilter.Fill);
				return colorFilterInfo;
			}
			ForeColorFilter foreColorFilter = (ForeColorFilter)base.Worksheet.Filter.GetFilter(columnId);
			colorFilterInfo.CellColor = false;
			PatternFill fill = new PatternFill(PatternType.Solid, foreColorFilter.Color, ThemableColor.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			colorFilterInfo.DxfId = this.WorkbookContext.DifferentialFormatsContext.RegisterFormatAndGetId(fill);
			return colorFilterInfo;
		}

		public SortStateInfo GetSortStateInfo()
		{
			return new SortStateInfo
			{
				Range = base.Worksheet.SortState.SortRange
			};
		}

		public IEnumerable<SortConditionInfo> GetSortConditionInfos()
		{
			foreach (ISortCondition condition in base.Worksheet.SortState.SortConditions)
			{
				SortConditionInfo info = new SortConditionInfo();
				info.Range = this.CalculateSortRange(base.Worksheet.SortState.SortRange, condition.RelativeIndex, false);
				ValuesSortCondition valuesSort = condition as ValuesSortCondition;
				if (valuesSort != null)
				{
					info.Descending = valuesSort.SortOrder == SortOrder.Descending;
					info.SortBy = SortBy.Value;
				}
				CustomValuesSortCondition customValuesSort = condition as CustomValuesSortCondition;
				if (customValuesSort != null)
				{
					info.CustomList = this.GetCustomListValues(customValuesSort.CustomList);
					info.SortBy = SortBy.Value;
				}
				FillColorSortCondition fillColorSortCondition = condition as FillColorSortCondition;
				if (fillColorSortCondition != null)
				{
					info.Descending = fillColorSortCondition.SortOrder == SortOrder.Descending;
					info.SortBy = SortBy.CellColor;
					info.DxfId = new int?(this.WorkbookContext.DifferentialFormatsContext.RegisterFormatAndGetId(fillColorSortCondition.Fill));
				}
				ForeColorSortCondition foreColorCondition = condition as ForeColorSortCondition;
				if (foreColorCondition != null)
				{
					info.Descending = foreColorCondition.SortOrder == SortOrder.Descending;
					info.SortBy = SortBy.FontColor;
					info.DxfId = new int?(this.WorkbookContext.DifferentialFormatsContext.RegisterFormatAndGetId(foreColorCondition.Color));
				}
				yield return info;
			}
			yield break;
		}

		public double GetDefaultColumnWidth()
		{
			return XlsxHelper.ConvertColumnPixelWidthToExcelWidth(base.Worksheet.Workbook, base.Worksheet.Columns.GetDefaultWidth().Value);
		}

		public RowHeight GetDefaultRowHeight()
		{
			return base.Worksheet.Rows.GetDefaultHeight();
		}

		public void InitFontInfos()
		{
			foreach (WorksheetEntityBase tableEntity in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.InitFontInfos(tableEntity);
			}
		}

		public void InitBordersInfos()
		{
			foreach (WorksheetEntityBase tableEntity in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.InitBordersInfos(tableEntity);
			}
		}

		public void RegisterPictureToIdRelationshipId(Image image, string relationshipId)
		{
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			Guard.ThrowExceptionIfNullOrEmpty(relationshipId, "relationshipId");
			this.pictureToRelationshipId.Add(image, relationshipId);
		}

		public void RegisterWorksheetPart(Worksheet worksheet, WorksheetPart worksheetPart)
		{
			this.workbookContext.RegisterWorksheetPart(worksheet, worksheetPart);
		}

		public int GetDataValidationsCount()
		{
			return this.dataValidationRuleToCellRanges.Count;
		}

		public IEnumerable<IDataValidationRule> GetDataValidationRules()
		{
			return this.dataValidationRuleToCellRanges.Keys;
		}

		public IEnumerable<CellRange> GetDataValidationRuleCellRanges(IDataValidationRule rule)
		{
			return this.dataValidationRuleToCellRanges[rule];
		}

		public CellRange GetUsedCellRange()
		{
			IEnumerable<IPropertyDefinition> propertyDefinitions = (from p in base.Worksheet.Cells.Properties
				select p.PropertyDefinition).Except(new IPropertyDefinition[] { CellPropertyDefinitions.DataValidationRuleProperty });
			return base.Worksheet.GetUsedCellRange(propertyDefinitions);
		}

		void SetRowAndColumnPropertiesAsLocalInCellsWhereCellsHasValueProperty()
		{
			ICompressedList<ICellValue> propertyValueCollection = base.Worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
			List<LongRange> list = new List<LongRange>();
			IEnumerable<Range<long, ICellValue>> nonDefaultRanges = propertyValueCollection.GetNonDefaultRanges();
			Range<long, ICellValue> range = nonDefaultRanges.FirstOrDefault<Range<long, ICellValue>>();
			if (range == null || !nonDefaultRanges.Skip(1).Any<Range<long, ICellValue>>())
			{
				return;
			}
			long start = range.Start;
			long end = range.End;
			foreach (Range<long, ICellValue> range2 in nonDefaultRanges.Skip(1))
			{
				if (end + 1L == range2.Start)
				{
					end = range2.End;
				}
				else
				{
					list.Add(new LongRange(start, end));
					start = range2.Start;
					end = range2.End;
				}
			}
			if (list.Count > 0 && list[list.Count - 1].End != end)
			{
				list.Add(new LongRange(start, end));
			}
			foreach (LongRange longRange in list)
			{
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellBorder>(CellPropertyDefinitions.BottomBorderProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<IFill>(CellPropertyDefinitions.FillProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<double>(CellPropertyDefinitions.FontSizeProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<ThemableColor>(CellPropertyDefinitions.ForeColorProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellValueFormat>(CellPropertyDefinitions.FormatProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<int>(CellPropertyDefinitions.IndentProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<bool>(CellPropertyDefinitions.IsBoldProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<bool>(CellPropertyDefinitions.IsItalicProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<bool>(CellPropertyDefinitions.IsLockedProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<bool>(CellPropertyDefinitions.IsWrappedProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellBorder>(CellPropertyDefinitions.LeftBorderProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellBorder>(CellPropertyDefinitions.RightBorderProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<CellBorder>(CellPropertyDefinitions.TopBorderProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<UnderlineType>(CellPropertyDefinitions.UnderlineProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty, longRange.Start, longRange.End);
				this.SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<string>(CellPropertyDefinitions.StyleNameProperty, longRange.Start, longRange.End);
			}
		}

		void SetRowAndColumnPropertyAsLocalInCellsWhereCellsHasValueProperty<T>(IPropertyDefinition<T> propertyDefinition, long fromIndex, long toIndex)
		{
			ICompressedList<T> propertyValueRespectingStyle = base.Worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<T>(propertyDefinition, base.Worksheet, fromIndex, toIndex);
			base.Worksheet.Cells.PropertyBag.GetPropertyValueCollection<T>(propertyDefinition).SetValue(propertyValueRespectingStyle);
		}

		void InitFontInfos(WorksheetEntityBase tableEntity)
		{
			this.UpdateProperty<bool, FontInfo>(tableEntity, CellPropertyDefinitions.IsBoldProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, bool value)
			{
				fontInfo.Bold = new bool?(value);
				return fontInfo;
			});
			this.UpdateProperty<bool, FontInfo>(tableEntity, CellPropertyDefinitions.IsItalicProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, bool value)
			{
				fontInfo.Italic = new bool?(value);
				return fontInfo;
			});
			this.UpdateProperty<double, FontInfo>(tableEntity, CellPropertyDefinitions.FontSizeProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, double value)
			{
				fontInfo.FontSize = new double?(value);
				return fontInfo;
			});
			this.UpdateProperty<ThemableFontFamily, FontInfo>(tableEntity, CellPropertyDefinitions.FontFamilyProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, ThemableFontFamily value)
			{
				fontInfo.FontFamily = value;
				return fontInfo;
			});
			this.UpdateProperty<ThemableColor, FontInfo>(tableEntity, CellPropertyDefinitions.ForeColorProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, ThemableColor value)
			{
				fontInfo.ForeColor = value;
				return fontInfo;
			});
			this.UpdateProperty<UnderlineType, FontInfo>(tableEntity, CellPropertyDefinitions.UnderlineProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, UnderlineType value)
			{
				fontInfo.UnderlineType = new UnderlineType?(value);
				return fontInfo;
			});
			this.UpdateProperty<string, FontInfo>(tableEntity, CellPropertyDefinitions.StyleNameProperty, this.worksheetEntityToFontInfoCompressedList[tableEntity], delegate(FontInfo fontInfo, string value)
			{
				int styleFormattingRecordId = this.workbookContext.GetStyleFormattingRecordId(value);
				FormattingRecord formattingRecord = this.workbookContext.StyleSheet.StyleFormattingTable[styleFormattingRecordId];
				if (formattingRecord.FontInfoId != null)
				{
					FontInfo other = this.workbookContext.StyleSheet.FontInfoTable[formattingRecord.FontInfoId.Value];
					fontInfo = fontInfo.MergeWith(other);
				}
				return fontInfo;
			});
		}

		void InitBordersInfos(WorksheetEntityBase tableEntity)
		{
			this.UpdateProperty<CellBorder, BordersInfo>(tableEntity, CellPropertyDefinitions.LeftBorderProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, CellBorder value)
			{
				bordersInfo.Left = value;
				return bordersInfo;
			});
			this.UpdateProperty<CellBorder, BordersInfo>(tableEntity, CellPropertyDefinitions.TopBorderProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, CellBorder value)
			{
				bordersInfo.Top = value;
				return bordersInfo;
			});
			this.UpdateProperty<CellBorder, BordersInfo>(tableEntity, CellPropertyDefinitions.RightBorderProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, CellBorder value)
			{
				bordersInfo.Right = value;
				return bordersInfo;
			});
			this.UpdateProperty<CellBorder, BordersInfo>(tableEntity, CellPropertyDefinitions.BottomBorderProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, CellBorder value)
			{
				bordersInfo.Bottom = value;
				return bordersInfo;
			});
			this.UpdateProperty<CellBorder, BordersInfo>(tableEntity, CellPropertyDefinitions.DiagonalUpBorderProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, CellBorder value)
			{
				bordersInfo.DiagonalUp = value;
				return bordersInfo;
			});
			this.UpdateProperty<CellBorder, BordersInfo>(tableEntity, CellPropertyDefinitions.DiagonalDownBorderProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, CellBorder value)
			{
				bordersInfo.DiagonalDown = value;
				return bordersInfo;
			});
			this.UpdateProperty<string, BordersInfo>(tableEntity, CellPropertyDefinitions.StyleNameProperty, this.worksheetEntityToBordersInfoCompressedList[tableEntity], delegate(BordersInfo bordersInfo, string value)
			{
				int styleFormattingRecordId = this.workbookContext.GetStyleFormattingRecordId(value);
				FormattingRecord formattingRecord = this.workbookContext.StyleSheet.StyleFormattingTable[styleFormattingRecordId];
				if (formattingRecord.BordersInfoId != null)
				{
					BordersInfo other = this.workbookContext.StyleSheet.BordersInfoTable[formattingRecord.BordersInfoId.Value];
					bordersInfo = bordersInfo.MergeWith(other);
				}
				return bordersInfo;
			});
		}

		void Initialize()
		{
			this.InitStyleNames();
			this.InitFills();
			this.InitCellValueFormats();
			this.InitAlignments();
			this.InitProtection();
			this.InitDataValidationRules();
			this.InitializeColumnProperties();
		}

		void InitStyleNames()
		{
			foreach (WorksheetEntityBase worksheetEntityBase in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.worksheetEntityToStyleNameCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
			}
		}

		void InitFills()
		{
			foreach (WorksheetEntityBase worksheetEntityBase in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.worksheetEntityToFillCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<IFill>(CellPropertyDefinitions.FillProperty);
			}
		}

		void InitCellValueFormats()
		{
			foreach (WorksheetEntityBase worksheetEntityBase in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.worksheetEntityToCellValueFormatCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<CellValueFormat>(CellPropertyDefinitions.FormatProperty);
			}
		}

		void InitAlignments()
		{
			foreach (WorksheetEntityBase worksheetEntityBase in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.worksheetEntityToHorizontalAlignmentCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty);
				this.worksheetEntityToVerticalAlignmentCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty);
				this.worksheetEntityToIndentCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<int>(CellPropertyDefinitions.IndentProperty);
				this.worksheetEntityToIsWrappedCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<bool>(CellPropertyDefinitions.IsWrappedProperty);
			}
		}

		void InitProtection()
		{
			foreach (WorksheetEntityBase worksheetEntityBase in this.WorkbookContext.GetWorksheetEntitiesFromWorksheet(base.Worksheet))
			{
				this.worksheetEntityToIsLockedCompressedList[worksheetEntityBase] = worksheetEntityBase.PropertyBagBase.GetPropertyValueCollection<bool>(CellPropertyDefinitions.IsLockedProperty);
			}
		}

		void InitDataValidationRules()
		{
			CellsPropertyBag propertyBag = base.Worksheet.Cells.PropertyBag;
			IPropertyDefinition<IDataValidationRule> dataValidationRuleProperty = CellPropertyDefinitions.DataValidationRuleProperty;
			this.dataValidationRulesCompressedList = propertyBag.GetPropertyValueCollection<IDataValidationRule>(dataValidationRuleProperty);
			foreach (Range<long, IDataValidationRule> range in this.dataValidationRulesCompressedList.GetNonDefaultRanges())
			{
				IDataValidationRule value = range.Value;
				if (!this.dataValidationRuleToCellRanges.ContainsKey(value))
				{
					IEnumerable<CellRange> valueContainingCellRanges = propertyBag.GetValueContainingCellRanges<IDataValidationRule>(dataValidationRuleProperty, value);
					this.dataValidationRuleToCellRanges.Add(value, valueContainingCellRanges);
				}
			}
		}

		void InitializeColumnProperties()
		{
			ICompressedList<ColumnWidth> propertyValueCollection = base.Worksheet.Columns.PropertyBag.GetPropertyValueCollection<ColumnWidth>(ColumnsPropertyBag.WidthProperty);
			ICompressedList<bool> propertyValueCollection2 = base.Worksheet.Columns.PropertyBag.GetPropertyValueCollection<bool>(RowColumnPropertyBagBase.HiddenProperty);
			ICompressedList<int> propertyValueCollection3 = base.Worksheet.Columns.PropertyBag.GetPropertyValueCollection<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
			this.UpdateProperty<ColumnWidth, ColumnInfo>(base.Worksheet.Columns, propertyValueCollection, this.columnInfoCompressedList, delegate(ColumnInfo columnInfo, ColumnWidth value)
			{
				columnInfo.IsCustom = value.IsCustom;
				columnInfo.BestFit = value.IsAutoFit;
				columnInfo.Width = XlsxHelper.ConvertColumnPixelWidthToExcelWidth(base.Worksheet.Workbook, value.Value);
				return columnInfo;
			});
			this.UpdateProperty<bool, ColumnInfo>(base.Worksheet.Columns, propertyValueCollection2, this.columnInfoCompressedList, delegate(ColumnInfo columnInfo, bool value)
			{
				columnInfo.Hidden = value;
				return columnInfo;
			});
			this.UpdateProperty<int, ColumnInfo>(base.Worksheet.Columns, propertyValueCollection3, this.columnInfoCompressedList, delegate(ColumnInfo columnInfo, int value)
			{
				columnInfo.OutlineLevel = value;
				return columnInfo;
			});
			this.InitializeColumnProperty<CellBorder>(CellPropertyDefinitions.BottomBorderProperty);
			this.InitializeColumnProperty<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty);
			this.InitializeColumnProperty<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.InitializeColumnProperty<IFill>(CellPropertyDefinitions.FillProperty);
			this.InitializeColumnProperty<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty);
			this.InitializeColumnProperty<double>(CellPropertyDefinitions.FontSizeProperty);
			this.InitializeColumnProperty<ThemableColor>(CellPropertyDefinitions.ForeColorProperty);
			this.InitializeColumnProperty<CellValueFormat>(CellPropertyDefinitions.FormatProperty);
			this.InitializeColumnProperty<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.InitializeColumnProperty<int>(CellPropertyDefinitions.IndentProperty);
			this.InitializeColumnProperty<bool>(CellPropertyDefinitions.IsBoldProperty);
			this.InitializeColumnProperty<bool>(CellPropertyDefinitions.IsItalicProperty);
			this.InitializeColumnProperty<bool>(CellPropertyDefinitions.IsLockedProperty);
			this.InitializeColumnProperty<bool>(CellPropertyDefinitions.IsWrappedProperty);
			this.InitializeColumnProperty<CellBorder>(CellPropertyDefinitions.LeftBorderProperty);
			this.InitializeColumnProperty<CellBorder>(CellPropertyDefinitions.RightBorderProperty);
			this.InitializeColumnProperty<string>(CellPropertyDefinitions.StyleNameProperty);
			this.InitializeColumnProperty<CellBorder>(CellPropertyDefinitions.TopBorderProperty);
			this.InitializeColumnProperty<UnderlineType>(CellPropertyDefinitions.UnderlineProperty);
			this.InitializeColumnProperty<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty);
		}

		void InitializeColumnProperty<T>(IPropertyDefinition<T> propertyDefinition)
		{
			ICompressedList<T> propertyValueCollection = base.Worksheet.Columns.PropertyBag.GetPropertyValueCollection<T>(propertyDefinition);
			this.UpdateProperty<T, ColumnInfo>(base.Worksheet.Columns, propertyValueCollection, this.columnInfoCompressedList, (ColumnInfo columnInfo, T value) => columnInfo);
		}

		void InitializePropertyToIsDefault()
		{
			this.propertyToIsDefaultFunc = new Dictionary<IPropertyDefinition, Func<ICompressedList, bool>>();
			this.AddPropertyToIsDefaultFuncDefault<CellBorder>(CellPropertyDefinitions.BottomBorderProperty);
			this.AddPropertyToIsDefaultFuncDefault<IDataValidationRule>(CellPropertyDefinitions.DataValidationRuleProperty);
			this.AddPropertyToIsDefaultFuncDefault<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty);
			this.AddPropertyToIsDefaultFuncDefault<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.AddPropertyToIsDefaultFuncDefault<IFill>(CellPropertyDefinitions.FillProperty);
			this.AddPropertyToIsDefaultFuncDefault<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty);
			this.AddPropertyToIsDefaultFuncDefault<double>(CellPropertyDefinitions.FontSizeProperty);
			this.AddPropertyToIsDefaultFuncDefault<ThemableColor>(CellPropertyDefinitions.ForeColorProperty);
			this.AddPropertyToIsDefaultFuncDefault<CellValueFormat>(CellPropertyDefinitions.FormatProperty);
			this.AddPropertyToIsDefaultFuncDefault<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.AddPropertyToIsDefaultFuncDefault<int>(CellPropertyDefinitions.IndentProperty);
			this.AddPropertyToIsDefaultFuncDefault<bool>(CellPropertyDefinitions.IsBoldProperty);
			this.AddPropertyToIsDefaultFuncDefault<bool>(CellPropertyDefinitions.IsItalicProperty);
			this.AddPropertyToIsDefaultFuncDefault<bool>(CellPropertyDefinitions.IsLockedProperty);
			this.AddPropertyToIsDefaultFuncDefault<bool>(CellPropertyDefinitions.IsWrappedProperty);
			this.AddPropertyToIsDefaultFuncDefault<CellBorder>(CellPropertyDefinitions.LeftBorderProperty);
			this.AddPropertyToIsDefaultFuncDefault<CellBorder>(CellPropertyDefinitions.RightBorderProperty);
			this.AddPropertyToIsDefaultFuncDefault<string>(CellPropertyDefinitions.StyleNameProperty);
			this.AddPropertyToIsDefaultFuncDefault<CellBorder>(CellPropertyDefinitions.TopBorderProperty);
			this.AddPropertyToIsDefaultFuncDefault<UnderlineType>(CellPropertyDefinitions.UnderlineProperty);
			this.AddPropertyToIsDefaultFuncDefault<ICellValue>(CellPropertyDefinitions.ValueProperty);
			this.AddPropertyToIsDefaultFuncDefault<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty);
			this.propertyToIsDefaultFunc.Add(RowsPropertyBag.HeightProperty, delegate(ICompressedList compressedList)
			{
				CompressedList<RowHeight> compressedList2 = compressedList as CompressedList<RowHeight>;
				ValueBox<RowHeight> valueInternal = compressedList2.GetValueInternal(compressedList2.FromIndex);
				return valueInternal == null || !valueInternal.Value.IsCustom;
			});
			this.AddPropertyToIsDefaultFuncDefault<bool>(RowColumnPropertyBagBase.HiddenProperty);
			this.AddPropertyToIsDefaultFuncDefault<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
			this.AddPropertyToIsDefaultFuncDefault<bool>(RowColumnPropertyBagBase.HiddenProperty);
			this.AddPropertyToIsDefaultFuncDefault<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
			this.AddPropertyToIsDefaultFuncDefault<ColumnWidth>(ColumnsPropertyBag.WidthProperty);
		}

		void AddPropertyToIsDefaultFuncDefault<T>(IPropertyDefinition<T> propertyDefinition)
		{
			if (this.propertyToIsDefaultFunc.ContainsKey(propertyDefinition))
			{
				return;
			}
			this.propertyToIsDefaultFunc.Add(propertyDefinition, delegate(ICompressedList compressedList)
			{
				CompressedList<T> compressedList2 = compressedList as CompressedList<T>;
				ValueBox<T> valueInternal = compressedList2.GetValueInternal(compressedList2.FromIndex);
				return valueInternal == null;
			});
		}

		void UpdateProperty<TSource, TTarget>(WorksheetEntityBase tableEntity, IPropertyDefinition<TSource> sourcePropertyDefinition, ICompressedList<TTarget> targetCompressedList, Func<TTarget, TSource, TTarget> propertySetter) where TTarget : struct
		{
			this.UpdateProperty<TSource, TTarget>(tableEntity, tableEntity.PropertyBagBase.GetPropertyValueCollection<TSource>(sourcePropertyDefinition), targetCompressedList, propertySetter);
		}

		void UpdateProperty<TSource, TTarget>(WorksheetEntityBase tableEntity, ICompressedList<TSource> sourceCompressedList, ICompressedList<TTarget> targetCompressedList, Func<TTarget, TSource, TTarget> propertySetter) where TTarget : struct
		{
			foreach (Range<long, TSource> range in sourceCompressedList.GetNonDefaultRanges())
			{
				this.NonDefaultFormattingCompressedList[tableEntity].SetValue(range.Start, range.End, true);
				if (!TelerikHelper.EqualsOfT<TSource>(range.Value, sourceCompressedList.GetDefaultValue()))
				{
					ICompressedList<TTarget> value = targetCompressedList.GetValue(range.Start, range.End);
					CompressedList<TTarget> compressedList = new CompressedList<TTarget>(value);
					foreach (Range<long, TTarget> range2 in value)
					{
						TTarget value2 = range2.Value;
						TTarget value3 = propertySetter(value2, range.Value);
						compressedList.SetValue(range2.Start, range2.End, value3);
					}
					targetCompressedList.SetValue(compressedList);
				}
			}
		}

		CellRange CalculateSortRange(CellRange sortRange, int index, bool sortByColumns)
		{
			if (sortByColumns)
			{
				int num = sortRange.FromIndex.RowIndex + index;
				return new CellRange(num, sortRange.FromIndex.ColumnIndex, num, sortRange.ToIndex.RowIndex);
			}
			int num2 = sortRange.FromIndex.ColumnIndex + index;
			return new CellRange(sortRange.FromIndex.RowIndex, num2, sortRange.ToIndex.RowIndex, num2);
		}

		string GetCustomListValues(string[] customList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < customList.Length; i++)
			{
				stringBuilder.Append(customList[i]);
				if (i < customList.Length - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder.ToString();
		}

		readonly CellPropertyDataInfo directFormattingPropertyDataInfo;

		readonly ICompressedList<ColumnInfo> columnInfoCompressedList;

		readonly List<CellPropertyDataInfo> rowPropertyDataInfosToRespect;

		readonly XlsxWorkbookExportContext workbookContext;

		readonly int sheetNo;

		readonly Dictionary<Image, string> pictureToRelationshipId;

		readonly Dictionary<IDataValidationRule, IEnumerable<CellRange>> dataValidationRuleToCellRanges;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<FormattingRecord>> worksheetEntryToDirectFormattingCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<FontInfo>> worksheetEntityToFontInfoCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<BordersInfo>> worksheetEntityToBordersInfoCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<string>> worksheetEntityToStyleNameCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<IFill>> worksheetEntityToFillCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<CellValueFormat>> worksheetEntityToCellValueFormatCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<RadHorizontalAlignment>> worksheetEntityToHorizontalAlignmentCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<RadVerticalAlignment>> worksheetEntityToVerticalAlignmentCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<int>> worksheetEntityToIndentCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<bool>> worksheetEntityToIsWrappedCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<bool>> worksheetEntityToIsLockedCompressedList;

		readonly Dictionary<WorksheetEntityBase, ICompressedList<bool>> worksheetEntityToNonDefaultFormattingRecordIndex;

		Dictionary<IPropertyDefinition, Func<ICompressedList, bool>> propertyToIsDefaultFunc;

		bool areDirectFormattingRecordsInitialized;

		ICompressedList<IDataValidationRule> dataValidationRulesCompressedList;
	}
}
