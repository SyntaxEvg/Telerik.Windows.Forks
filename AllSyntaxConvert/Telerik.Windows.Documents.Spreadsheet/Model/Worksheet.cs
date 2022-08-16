using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.History;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Model.Protection;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class Worksheet : Sheet
	{
		public Cells Cells
		{
			get
			{
				return this.cells;
			}
		}

		public Rows Rows
		{
			get
			{
				return this.rows;
			}
		}

		public Columns Columns
		{
			get
			{
				return this.columns;
			}
		}

		public override SheetType Type
		{
			get
			{
				return SheetType.Worksheet;
			}
		}

		public ColumnWidth DefaultColumnWidth
		{
			get
			{
				return this.Columns.GetDefaultWidth();
			}
			set
			{
				this.Columns.SetDefaultWidth(value);
			}
		}

		public RowHeight DefaultRowHeight
		{
			get
			{
				return this.Rows.GetDefaultHeight();
			}
			set
			{
				this.Rows.SetDefaultHeight(value);
			}
		}

		public CellRange UsedCellRange
		{
			get
			{
				return this.cells.PropertyBag.GetUsedCellRange();
			}
		}

		public HyperlinkCollection Hyperlinks
		{
			get
			{
				return this.hyperlinks;
			}
		}

		public NameCollection Names
		{
			get
			{
				return this.names;
			}
		}

		public ShapeCollection Shapes
		{
			get
			{
				return this.shapes;
			}
		}

		public WorksheetPageSetup WorksheetPageSetup
		{
			get
			{
				return this.worksheetPageSetup;
			}
		}

		protected sealed override SheetPageSetupBase SheetPageSetup
		{
			get
			{
				return this.sheetPageSetup;
			}
		}

		public SortState SortState
		{
			get
			{
				return this.sortState;
			}
		}

		public WorksheetProtectionOptions ProtectionOptions
		{
			get
			{
				return this.protectionOptions;
			}
			internal set
			{
				this.protectionOptions = value;
			}
		}

		public AutoFilter Filter
		{
			get
			{
				return this.filter;
			}
		}

		public WorksheetViewState ViewState
		{
			get
			{
				return (WorksheetViewState)((ISheet)this).ViewState;
			}
		}

		public GroupingProperties GroupingProperties
		{
			get
			{
				return this.groupingProperties;
			}
		}

		internal CellRange ShapeUsedCellRange
		{
			get
			{
				return this.shapes.UsedCellRange;
			}
		}

		internal CellRange TotalUsedCellRange
		{
			get
			{
				CellRange usedCellRange = this.UsedCellRange;
				CellRange shapeUsedCellRange = this.ShapeUsedCellRange;
				int fromRowIndex = System.Math.Min(usedCellRange.FromIndex.RowIndex, shapeUsedCellRange.FromIndex.RowIndex);
				int fromColumnIndex = System.Math.Min(usedCellRange.FromIndex.ColumnIndex, shapeUsedCellRange.FromIndex.ColumnIndex);
				int toRowIndex = Math.Max(usedCellRange.ToIndex.RowIndex, shapeUsedCellRange.ToIndex.RowIndex);
				int toColumnIndex = Math.Max(usedCellRange.ToIndex.ColumnIndex, shapeUsedCellRange.ToIndex.ColumnIndex);
				return new CellRange(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
			}
		}

		public HeaderNameRenderingConverterBase HeaderNameRenderingConverter
		{
			get
			{
				if (this.headerNameRenderingConverter == null)
				{
					this.headerNameRenderingConverter = new DefaultHeaderRenderingNameConverter();
				}
				return this.headerNameRenderingConverter;
			}
			set
			{
				if (this.headerNameRenderingConverter != value)
				{
					this.headerNameRenderingConverter = value;
					base.InvalidateLayout();
				}
			}
		}

		internal Worksheet(string name, Workbook workbook)
			: base(name, workbook)
		{
			this.cells = new Cells(this);
			this.rows = new Rows(this);
			this.columns = new Columns(this);
			this.hyperlinks = new HyperlinkCollection(this);
			this.names = base.Workbook.NameManager.CreateCollection(this);
			this.protectionOptions = WorksheetProtectionOptions.Default;
			this.filter = new AutoFilter(this);
			this.expandToFitCache = new Dictionary<int, CellRange>();
			this.worksheetPageSetup = new WorksheetPageSetup(this);
			this.sheetPageSetup = this.WorksheetPageSetup;
			this.shapes = new ShapeCollection(this);
			this.sortState = new SortState(this);
			this.groupingProperties = new GroupingProperties();
			this.Cells.PropertyBag.Sorted += this.PropertyBag_Sorted;
		}

		void PropertyBag_Sorted(object sender, SortedEventArgs e)
		{
			CellRange range = e.Range;
			int[] sortedIndexes = e.SortedIndexes;
			this.Hyperlinks.Sort(range, sortedIndexes);
			this.Rows.PropertyBag.Sort(range, sortedIndexes);
		}

		protected override ISheetViewState CreateViewState()
		{
			return new WorksheetViewState();
		}

		public FindResult Find(FindOptions findOptions)
		{
			IEnumerable<FindResult> enumerable = this.FindAll(findOptions);
			enumerable = FindAndReplaceHelper.OrderResults(findOptions, enumerable);
			return enumerable.FirstOrDefault<FindResult>();
		}

		public IEnumerable<FindResult> FindAll(FindOptions findOptions)
		{
			if (!string.IsNullOrEmpty(findOptions.FindWhat))
			{
				if (findOptions.FindWithin == FindWithin.Workbook)
				{
					findOptions.SearchRanges = null;
				}
				ICompressedList<ICellValue> cellProperyValueCompressedList = this.Cells.GetPropertyValueRespectingStyle<ICellValue>(CellPropertyDefinitions.ValueProperty, this.UsedCellRange);
				ICompressedList<bool> hiddenRows = this.Rows.PropertyBag.GetPropertyValueCollection<bool>(RowColumnPropertyBagBase.HiddenProperty);
				IEnumerable<Range<long, ICellValue>> nonDefaultRanges = cellProperyValueCompressedList.GetNonDefaultRanges();
				IEnumerable<Range<CellIndex, ICellValue>> singleCellNonDefaultRanges = this.SplitToSingleCellRanges(nonDefaultRanges);
				IEnumerable<Range<CellIndex, ICellValue>> findResults = singleCellNonDefaultRanges.Where(delegate(Range<CellIndex, ICellValue> range)
				{
					bool result = false;
					CellIndex index = range.Start;
					if ((findOptions.SearchRanges == null || (from p in findOptions.SearchRanges
						where p.Contains(index)
						select p).Count<CellRange>() > 0) && !hiddenRows.GetValue((long)index.RowIndex))
					{
						switch (findOptions.FindIn)
						{
						case FindInContentType.Formulas:
							result = FindAndReplaceHelper.FindInRawValue(range, findOptions) || FindAndReplaceHelper.FindInResultValue(this.Cells, range, findOptions);
							break;
						case FindInContentType.Values:
							result = FindAndReplaceHelper.FindInResultValue(this.Cells, range, findOptions);
							break;
						}
					}
					return result;
				});
				if (findOptions.FindBy == FindBy.Rows)
				{
					findResults = from p in findResults
						orderby p.Start.RowIndex, p.Start.ColumnIndex
						select p;
				}
				foreach (Range<CellIndex, ICellValue> range2 in findResults)
				{
					yield return new FindResult(new WorksheetCellIndex(this, range2.Start));
				}
			}
			yield break;
		}

		IEnumerable<Range<CellIndex, ICellValue>> SplitToSingleCellRanges(IEnumerable<Range<long, ICellValue>> nonDefaultRanges)
		{
			foreach (Range<long, ICellValue> range in nonDefaultRanges)
			{
				for (long i = range.Start; i <= range.End; i += 1L)
				{
					CellIndex index = WorksheetPropertyBagBase.ConvertLongToCellIndex(i);
					yield return new Range<CellIndex, ICellValue>(index, index, range.IsDefault, range.Value);
				}
			}
			yield break;
		}

		public bool Replace(ReplaceOptions replaceOptions)
		{
			if (!this.CheckIfCurrentCellContainsSearchedValue(replaceOptions))
			{
				return false;
			}
			CellSelection cellSelection = this.Cells[replaceOptions.StartCell.CellIndex];
			string rawValue = cellSelection.GetValue().Value.RawValue;
			string value = Regex.Replace(rawValue, replaceOptions.FindWhatRegex, delegate(Match m)
			{
				if (string.IsNullOrEmpty(m.Value))
				{
					return string.Empty;
				}
				return replaceOptions.ReplaceWith;
			}, replaceOptions.FindWhatRegexOptions);
			cellSelection.SetValueIgnoreErrorsInternal(value, true);
			return true;
		}

		bool CheckIfCurrentCellContainsSearchedValue(ReplaceOptions replaceOptions)
		{
			string rawValue = this.Cells[replaceOptions.StartCell.CellIndex].GetValue().Value.RawValue;
			return FindAndReplaceHelper.FindInString(rawValue, replaceOptions);
		}

		public int ReplaceAll(ReplaceOptions replaceOptions)
		{
			int replacesCount = 0;
			using (new UpdateScope(new Action(base.Workbook.History.BeginUndoGroup), new Action(base.Workbook.History.EndUndoGroup)))
			{
				List<FindResult> list = this.FindAll(replaceOptions).ToList<FindResult>();
				foreach (FindResult findResult in list)
				{
					CellSelection cellSelection = this.Cells[findResult.FoundCell.CellIndex];
					string rawValue = cellSelection.GetValue().Value.RawValue;
					string value = Regex.Replace(rawValue, replaceOptions.FindWhatRegex, delegate(Match m)
					{
						if (string.IsNullOrEmpty(m.Value))
						{
							return string.Empty;
						}
						replacesCount++;
						return replaceOptions.ReplaceWith;
					}, replaceOptions.FindWhatRegexOptions);
					cellSelection.SetValueIgnoreErrorsInternal(value, true);
				}
			}
			return replacesCount;
		}

		internal override void OnLayoutUpdateResumed()
		{
			this.ExpandCachedColumnWidths();
		}

		internal void ExpandCachedColumnWidths()
		{
			if (this.expandToFitCache.Count > 0)
			{
				CellRange[] array = this.expandToFitCache.Values.ToArray<CellRange>();
				this.expandToFitCache.Clear();
				List<CellRange> list = new List<CellRange>();
				CellRange cellRange = array[0];
				for (int i = 1; i < array.Length; i++)
				{
					int columnIndex = array[i].FromIndex.ColumnIndex;
					int rowIndex = array[i].FromIndex.RowIndex;
					int rowIndex2 = array[i].ToIndex.RowIndex;
					if (cellRange.FromIndex.ColumnIndex > columnIndex)
					{
						cellRange = new CellRange(rowIndex, columnIndex, rowIndex2, cellRange.ToIndex.ColumnIndex);
					}
					else if (cellRange.ToIndex.ColumnIndex < columnIndex)
					{
						cellRange = new CellRange(rowIndex, cellRange.FromIndex.ColumnIndex, rowIndex2, columnIndex);
					}
					else if (!cellRange.Contains(rowIndex, columnIndex) || !cellRange.Contains(rowIndex2, columnIndex))
					{
						list.Add(cellRange);
						i++;
						if (i < array.Length)
						{
							columnIndex = array[i].FromIndex.ColumnIndex;
							rowIndex = array[i].FromIndex.RowIndex;
							rowIndex2 = array[i].ToIndex.RowIndex;
							cellRange = new CellRange(rowIndex, columnIndex, rowIndex2, columnIndex);
						}
					}
				}
				if (list.Count == 0)
				{
					list.Add(cellRange);
				}
				foreach (CellRange cellRange2 in list)
				{
					this.Columns[cellRange2].AutoFitWidth(true, false);
				}
			}
		}

		internal void CacheExpandToFitColumns(IEnumerable<CellRange> cellRanges)
		{
			if (cellRanges != null)
			{
				foreach (CellRange cellRange in cellRanges)
				{
					for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
					{
						if (!this.expandToFitCache.ContainsKey(i))
						{
							this.expandToFitCache.Add(i, new CellRange(cellRange.FromIndex.RowIndex, i, cellRange.ToIndex.RowIndex, i));
						}
						else
						{
							CellRange cellRange2 = this.expandToFitCache[i];
							CellRange value = new CellRange(Math.Min(cellRange2.FromIndex.RowIndex, cellRange.FromIndex.RowIndex), i, Math.Max(cellRange2.ToIndex.RowIndex, cellRange.ToIndex.RowIndex), i);
							this.expandToFitCache[i] = value;
						}
					}
				}
			}
		}

		public void Protect(string password, WorksheetProtectionOptions options)
		{
			base.EnsureSheetNotProtected();
			this.protectionOptions = options ?? WorksheetProtectionOptions.Default;
			base.ProtectSheet(password);
		}

		public bool Unprotect(string password)
		{
			return base.UnprotectSheet(password);
		}

		public void CopyFrom(Worksheet sourceWorksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(sourceWorksheet, "sourceWorksheet");
			if (!this.Equals(sourceWorksheet))
			{
				CopyContext copyContext = new CopyContext(this, sourceWorksheet);
				this.Clear();
				if (sourceWorksheet.Workbook != base.Workbook)
				{
					CellStyleCollectionsMerger cellStyleCollectionsMerger = new CellStyleCollectionsMerger(base.Workbook.Styles, sourceWorksheet.Workbook.Styles, copyContext);
					cellStyleCollectionsMerger.Merge();
					base.Workbook.Names.CopyFrom(sourceWorksheet.Workbook.Names, copyContext);
				}
				this.Names.CopyFrom(sourceWorksheet.Names, copyContext);
				this.Shapes.CopyFrom(sourceWorksheet.Shapes, copyContext);
				this.Hyperlinks.CopyFrom(sourceWorksheet.Hyperlinks);
				this.Cells.CopyFrom(sourceWorksheet.Cells, copyContext);
				this.Rows.CopyFrom(sourceWorksheet.Rows, copyContext);
				this.Columns.CopyFrom(sourceWorksheet.Columns, copyContext);
				base.ProtectionData.CopyFrom(sourceWorksheet.ProtectionData);
				this.ProtectionOptions = sourceWorksheet.ProtectionOptions.Clone();
				this.WorksheetPageSetup.CopyFrom(sourceWorksheet.WorksheetPageSetup);
				this.ViewState.CopyFrom(sourceWorksheet.ViewState);
				this.SortState.CopyFrom(sourceWorksheet.SortState, copyContext);
				this.Filter.CopyFrom(sourceWorksheet.Filter, copyContext);
				this.GroupingProperties.CopyFrom(sourceWorksheet.GroupingProperties);
				this.Columns.PropertyBag.SetDefaultPropertyValue<ColumnWidth>(ColumnsPropertyBag.WidthProperty, sourceWorksheet.DefaultColumnWidth);
				this.Rows.PropertyBag.SetDefaultPropertyValue<RowHeight>(RowsPropertyBag.HeightProperty, sourceWorksheet.DefaultRowHeight);
				this.HeaderNameRenderingConverter = sourceWorksheet.HeaderNameRenderingConverter;
				base.Visibility = sourceWorksheet.Visibility;
				this.OnCopied();
				base.Workbook.InvalidateLayout();
			}
		}

		void Clear()
		{
			WorkbookHistory history = base.Workbook.History;
			bool isHistoryEnabled = history.IsEnabled;
			using (new UpdateScope(delegate()
			{
				history.IsEnabled = false;
			}, delegate()
			{
				history.IsEnabled = isHistoryEnabled;
			}))
			{
				this.Names.Clear();
				this.Shapes.Clear();
				this.Hyperlinks.Clear();
				this.Cells.Clear();
				this.Rows.Clear();
				this.Columns.Clear();
				this.WorksheetPageSetup.Clear();
				this.SortState.ClearInternal();
				this.Filter.ClearFilters();
			}
		}

		internal bool CanInsertRows
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowInsertRows);
			}
		}

		internal bool CanDeleteRows
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowDeleteRows);
			}
		}

		internal bool CanInsertColumns
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowInsertColumns);
			}
		}

		internal bool CanDeleteColumns
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowDeleteColumns);
			}
		}

		internal bool CanFormatCells
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowFormatCells);
			}
		}

		internal bool CanFormatColumns
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowFormatColumns);
			}
		}

		internal bool CanFormatRows
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowFormatRows);
			}
		}

		internal bool CanFilter
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowFiltering);
			}
		}

		internal bool CanSort
		{
			get
			{
				return this.ShouldEnforceProtectionOption(this.ProtectionOptions.AllowSorting);
			}
		}

		bool ShouldEnforceProtectionOption(bool protectionOption)
		{
			return !base.IsProtected || protectionOption;
		}

		public CellRange GetUsedCellRange(IEnumerable<IPropertyDefinition> propertyDefinitions)
		{
			return this.Cells.PropertyBag.GetUsedCellRange(propertyDefinitions);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.alreadyDisposed)
			{
				if (disposing)
				{
					base.Workbook.NameManager.RemoveOwner(this);
					this.names = null;
					this.hyperlinks = null;
				}
				this.alreadyDisposed = true;
			}
			base.Dispose(disposing);
		}

		internal event EventHandler Copied;

		void OnCopied()
		{
			if (this.Copied != null)
			{
				this.Copied(this, EventArgs.Empty);
			}
		}

		readonly Cells cells;

		readonly Rows rows;

		readonly Columns columns;

		HyperlinkCollection hyperlinks;

		NameCollection names;

		readonly ShapeCollection shapes;

		WorksheetProtectionOptions protectionOptions;

		readonly AutoFilter filter;

		readonly GroupingProperties groupingProperties;

		bool alreadyDisposed;

		readonly Dictionary<int, CellRange> expandToFitCache;

		readonly WorksheetPageSetup worksheetPageSetup;

		readonly SheetPageSetupBase sheetPageSetup;

		readonly SortState sortState;

		HeaderNameRenderingConverterBase headerNameRenderingConverter;
	}
}
