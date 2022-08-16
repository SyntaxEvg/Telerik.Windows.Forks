using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class Cells : WorksheetEntityBase
	{
		public int RowCount
		{
			get
			{
				return SpreadsheetDefaultValues.RowCount;
			}
		}

		public int ColumnCount
		{
			get
			{
				return SpreadsheetDefaultValues.ColumnCount;
			}
		}

		internal override PropertyBagBase PropertyBagBase
		{
			get
			{
				return this.PropertyBag;
			}
		}

		internal CellsPropertyBag PropertyBag
		{
			get
			{
				return this.propertyBag;
			}
		}

		internal MergedCellRanges MergedCellRanges
		{
			get
			{
				return this.mergedCellRanges;
			}
		}

		internal IProperty<ICellValue> ValueProperty
		{
			get
			{
				return this.valueProperty;
			}
		}

		internal IProperty<IDataValidationRule> DataValidationRuleProperty
		{
			get
			{
				return this.dataValidationRuleProperty;
			}
		}

		public CellSelection this[int rowIndex, int columnIndex]
		{
			get
			{
				return this.GetCellSelection(rowIndex, columnIndex);
			}
		}

		public CellSelection this[CellIndex cellIndex]
		{
			get
			{
				return this.GetCellSelection(cellIndex);
			}
		}

		public CellSelection this[int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex]
		{
			get
			{
				return this.GetCellSelection(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
			}
		}

		public CellSelection this[CellIndex fromIndex, CellIndex toIndex]
		{
			get
			{
				return this.GetCellSelection(fromIndex, toIndex);
			}
		}

		public CellSelection this[CellRange cellRange]
		{
			get
			{
				return this.GetCellSelection(cellRange);
			}
		}

		public CellSelection this[IEnumerable<CellRange> cellRanges]
		{
			get
			{
				return this.GetCellSelection(cellRanges);
			}
		}

		internal Cells(Worksheet worksheet)
			: base(worksheet)
		{
			this.propertyBag = new CellsPropertyBag();
			this.propertyBag.PropertyChanged += this.PropertyBag_PropertyChanged;
			this.mergedCellRanges = new MergedCellRanges(this);
			this.mergedCellRanges.Changing += this.MergedCellRanges_Changing;
			this.mergedCellRanges.Changed += this.MergedCellRanges_Changed;
			this.valueProperty = base.CreateProperty<ICellValue>(CellPropertyDefinitions.ValueProperty);
			this.dataValidationRuleProperty = base.CreateProperty<IDataValidationRule>(CellPropertyDefinitions.DataValidationRuleProperty);
		}

		void MergedCellRanges_Changing(object sender, EventArgs e)
		{
			this.OnMergedCellsChanging();
		}

		void MergedCellRanges_Changed(object sender, MergedCellRangesChangedEventArgs args)
		{
			this.OnMergedCellsChanged(args);
		}

		void PropertyBag_PropertyChanged(object sender, CellPropertyChangedEventArgs e)
		{
			this.OnCellPropertyChanged(e);
		}

		internal bool IsDefault(CellIndex index)
		{
			long index2 = WorksheetPropertyBagBase.ConvertCellIndexToLong(index);
			return this.PropertyBag.IsDefault(index2);
		}

		internal override IProperty<T> CreatePropertyOverride<T>(IPropertyDefinition<T> propertyDefinition)
		{
			return new CellProperty<T>(base.Worksheet, propertyDefinition);
		}

		public bool CanInsert(int rowIndex, int columnIndex, int itemCount, InsertShiftType shiftType)
		{
			CellRange cellRange;
			if (shiftType == InsertShiftType.Right)
			{
				int num = columnIndex + itemCount - 1;
				if (!TelerikHelper.IsValidColumnIndex(num))
				{
					return false;
				}
				cellRange = new CellRange(rowIndex, columnIndex, rowIndex, num);
			}
			else
			{
				int num2 = columnIndex + itemCount - 1;
				if (!TelerikHelper.IsValidRowIndex(num2))
				{
					return false;
				}
				cellRange = new CellRange(rowIndex, columnIndex, num2, columnIndex);
			}
			return this.CanInsert(cellRange, shiftType);
		}

		public bool CanInsert(CellRange cellRange, InsertShiftType shiftType)
		{
			CanInsertRemoveResult canInsertRemoveResult = this.CanInsertInternal(cellRange, shiftType);
			return canInsertRemoveResult == CanInsertRemoveResult.Success || canInsertRemoveResult == CanInsertRemoveResult.MergedCells;
		}

		internal CanInsertRemoveResult CanInsertInternal(CellRange cellRange, InsertShiftType shiftType)
		{
			if (!this.CheckForFilterRangeBeforeInsert(cellRange, shiftType))
			{
				return CanInsertRemoveResult.Filtering;
			}
			if (!this.PropertyBag.CanInsertCells(cellRange, shiftType))
			{
				return CanInsertRemoveResult.ValueLoss;
			}
			ShiftType shiftType2 = shiftType.ToShiftType();
			if (!this.MergedCellRanges.CanInsertOrRemove(cellRange, shiftType2))
			{
				return CanInsertRemoveResult.MergedCells;
			}
			return CanInsertRemoveResult.Success;
		}

		internal bool CanRemove(CellRange cellRange, RemoveShiftType shiftType)
		{
			CanInsertRemoveResult canInsertRemoveResult = this.CanRemoveInternal(cellRange, shiftType);
			return canInsertRemoveResult == CanInsertRemoveResult.Success || canInsertRemoveResult == CanInsertRemoveResult.MergedCells;
		}

		internal CanInsertRemoveResult CanRemoveInternal(CellRange cellRange, RemoveShiftType shiftType)
		{
			if (!this.CheckForFilterRangeBeforeRemove(cellRange, shiftType))
			{
				return CanInsertRemoveResult.Filtering;
			}
			ShiftType shiftType2 = shiftType.ToShiftType();
			if (!this.MergedCellRanges.CanInsertOrRemove(cellRange, shiftType2))
			{
				return CanInsertRemoveResult.MergedCells;
			}
			return CanInsertRemoveResult.Success;
		}

		public bool Insert(CellRange cellRange, InsertShiftType shiftType)
		{
			bool flag = false;
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				this.UnmergeBeforeInsert(cellRange, shiftType);
				InsertCellCommandContext context = new InsertCellCommandContext(base.Worksheet, cellRange, shiftType);
				flag = base.Worksheet.ExecuteCommand<InsertCellCommandContext>(WorkbookCommands.InsertCell, context);
				if (flag)
				{
					this.UpdateCellRangeInsertedOrRemovedDependentCollections(cellRange, shiftType.ToRangeType(false), false);
				}
			}
			return flag;
		}

		public bool Insert(int rowIndex, int columnIndex, int itemCount, InsertShiftType shiftType)
		{
			CellRange cellRange;
			if (shiftType == InsertShiftType.Right)
			{
				cellRange = new CellRange(rowIndex, columnIndex, rowIndex, columnIndex + itemCount - 1);
			}
			else
			{
				cellRange = new CellRange(rowIndex, columnIndex, rowIndex + itemCount - 1, columnIndex);
			}
			return this.Insert(cellRange, shiftType);
		}

		public void Remove(CellRange cellRange, RemoveShiftType shiftType)
		{
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				this.UnmergeBeforeRemove(cellRange, shiftType);
				RemoveCellCommandContext context = new RemoveCellCommandContext(base.Worksheet, cellRange, shiftType);
				bool flag = base.Worksheet.ExecuteCommand<RemoveCellCommandContext>(WorkbookCommands.RemoveCell, context);
				if (flag)
				{
					this.UpdateCellRangeInsertedOrRemovedDependentCollections(cellRange, shiftType.ToRangeType(false), true);
				}
			}
		}

		public void Remove(int rowIndex, int columnIndex, int itemCount, RemoveShiftType shiftType)
		{
			CellRange cellRange;
			if (shiftType == RemoveShiftType.Left)
			{
				cellRange = new CellRange(rowIndex, columnIndex, rowIndex, columnIndex + itemCount - 1);
			}
			else
			{
				cellRange = new CellRange(rowIndex, columnIndex, rowIndex + itemCount - 1, columnIndex);
			}
			this.Remove(cellRange, shiftType);
		}

		internal void InsertInternal(CellRange cellRange, InsertShiftType shiftType, CellsPropertyBag oldValues = null)
		{
			if (oldValues == null)
			{
				this.PropertyBag.InsertEmptyCells(cellRange, shiftType);
			}
			else
			{
				this.PropertyBag.InsertCellsAndSetValues(cellRange, shiftType, oldValues);
			}
			RangeType rangeType = shiftType.ToRangeType(false);
			this.OnCellRangeInsertedOrRemoved(cellRange, rangeType, false);
		}

		internal void RemoveInternal(CellRange cellRange, RemoveShiftType shiftType)
		{
			this.PropertyBag.RemoveCells(cellRange, shiftType);
			RangeType rangeType = shiftType.ToRangeType(false);
			this.OnCellRangeInsertedOrRemoved(cellRange, rangeType, true);
		}

		internal void InsertColumn(int index, int itemCount, CellsPropertyBag oldValues = null)
		{
			if (oldValues == null)
			{
				this.PropertyBag.InsertEmptyColumn(index, itemCount);
			}
			else
			{
				this.PropertyBag.InsertColumnAndSetValues(index, itemCount, oldValues);
			}
			CellRange range = CellRange.FromColumnRange(index, index + itemCount - 1);
			this.OnCellRangeInsertedOrRemoved(range, RangeType.Columns, false);
		}

		internal void RemoveColumn(int index, int itemCount)
		{
			this.PropertyBag.RemoveColumn(index, itemCount);
			CellRange range = CellRange.FromColumnRange(index, index + itemCount - 1);
			this.OnCellRangeInsertedOrRemoved(range, RangeType.Columns, true);
		}

		internal void InsertRow(int index, int itemCount, CellsPropertyBag oldValues = null)
		{
			if (oldValues == null)
			{
				this.PropertyBag.InsertEmptyRow(index, itemCount);
			}
			else
			{
				this.PropertyBag.InsertRowAndSetValues(index, itemCount, oldValues);
			}
			CellRange range = CellRange.FromRowRange(index, index + itemCount - 1);
			this.OnCellRangeInsertedOrRemoved(range, RangeType.Rows, false);
		}

		internal void RemoveRow(int index, int itemCount)
		{
			this.PropertyBag.RemoveRow(index, itemCount);
			CellRange range = CellRange.FromRowRange(index, index + itemCount - 1);
			this.OnCellRangeInsertedOrRemoved(range, RangeType.Rows, true);
		}

		internal FontProperties GetFontProperties(int rowIndex, int columnIndex, bool onlyPropertiesAffectingLayout = false)
		{
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			return this.GetFontProperties(index, onlyPropertiesAffectingLayout);
		}

		internal FontProperties GetFontProperties(CellIndex cellIndex, bool onlyPropertiesAffectingLayout = false)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellIndex);
			return this.GetFontProperties(index, onlyPropertiesAffectingLayout);
		}

		internal FontProperties GetFontProperties(long index, bool onlyPropertiesAffectingLayout = false)
		{
			ThemableFontFamily propertyValueRespectingStyle = this.PropertyBag.GetPropertyValueRespectingStyle<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty, base.Worksheet, index);
			double propertyValueRespectingStyle2 = this.PropertyBag.GetPropertyValueRespectingStyle<double>(CellPropertyDefinitions.FontSizeProperty, base.Worksheet, index);
			bool propertyValueRespectingStyle3 = this.PropertyBag.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsBoldProperty, base.Worksheet, index);
			FontProperties result = new FontProperties(propertyValueRespectingStyle.GetActualValue(base.Worksheet.Workbook.Theme), propertyValueRespectingStyle2, propertyValueRespectingStyle3);
			if (!onlyPropertiesAffectingLayout)
			{
				result.IsItalic = this.PropertyBag.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsItalicProperty, base.Worksheet, index);
				result.Underline = this.PropertyBag.GetPropertyValueRespectingStyle<UnderlineType>(CellPropertyDefinitions.UnderlineProperty, base.Worksheet, index);
				result.ForeColor = this.PropertyBag.GetPropertyValueRespectingStyle<ThemableColor>(CellPropertyDefinitions.ForeColorProperty, base.Worksheet, index);
			}
			return result;
		}

		internal CellRangeFontProperties GetFontProperties(CellRange cellRange, bool onlyPropertiesAffectingLayout = false)
		{
			return new CellRangeFontProperties(this, cellRange, onlyPropertiesAffectingLayout);
		}

		internal ICompressedList<T> GetPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, CellRange cellRange)
		{
			return this.propertyBag.GetPropertyValueRespectingStyle<T>(property, base.Worksheet, cellRange);
		}

		internal ICompressedList<T> GetPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, long fromIndex, long toIndex)
		{
			return this.propertyBag.GetPropertyValueRespectingStyle<T>(property, base.Worksheet, fromIndex, toIndex);
		}

		internal void UpdateCellRangeInsertedOrRemovedDependentCollections(CellRange cellRange, RangeType rangeType, bool isRemove)
		{
			CellRangeInsertedOrRemovedEventArgs eventArgs = new CellRangeInsertedOrRemovedEventArgs(cellRange, rangeType, isRemove);
			this.MergedCellRanges.Update(eventArgs);
			base.Worksheet.Hyperlinks.Update(eventArgs);
			base.Worksheet.WorksheetPageSetup.PrintArea.Update(eventArgs);
			base.Worksheet.Filter.Update(eventArgs);
			base.Worksheet.SortState.Update(eventArgs);
			this.UpdateCellsInsertedOrRemovedDependentCellValues(eventArgs);
			this.UpdateCellsInsertedOrRemovedDependentDataValidationRules(eventArgs);
		}

		void UpdateCellsInsertedOrRemovedDependentDataValidationRules(CellRangeInsertedOrRemovedEventArgs eventArgs)
		{
			using (new UpdateScope(new Action(this.PropertyBag.SuspendPropertyChanged), new Action(this.PropertyBag.ResumePropertyChanged)))
			{
				foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(eventArgs.AffectedRange))
				{
					ICompressedList<IDataValidationRule> propertyValue = base.Worksheet.Cells.PropertyBag.GetPropertyValue<IDataValidationRule>(CellPropertyDefinitions.DataValidationRuleProperty, longRange.Start, longRange.End);
					foreach (Range<long, IDataValidationRule> range in propertyValue.GetNonDefaultRanges())
					{
						SingleArgumentDataValidationRuleBase singleArgumentDataValidationRuleBase = range.Value as SingleArgumentDataValidationRuleBase;
						if (singleArgumentDataValidationRuleBase != null)
						{
							CellIndex cellIndex = WorksheetPropertyBagBase.ConvertLongToCellIndex(range.Start);
							CellIndex toIndex = WorksheetPropertyBagBase.ConvertLongToCellIndex(range.End);
							IDataValidationRule dataValidationRule = singleArgumentDataValidationRuleBase.CloneAndTranslate(base.Worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex, false);
							if (!singleArgumentDataValidationRuleBase.Equals(dataValidationRule))
							{
								this[cellIndex, toIndex].SetDataValidationRule(dataValidationRule);
							}
						}
					}
				}
			}
		}

		internal void UpdateCellsInsertedOrRemovedDependentCellValues(CellRangeInsertedOrRemovedEventArgs eventArgs)
		{
			ICompressedList<ICellValue> propertyValueCollection = base.Worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
			using (new UpdateScope(new Action(this.PropertyBag.SuspendPropertyChanged), new Action(this.PropertyBag.ResumePropertyChanged)))
			{
				foreach (Range<long, ICellValue> range in propertyValueCollection.GetNonDefaultRanges())
				{
					FormulaCellValue formulaCellValue = range.Value as FormulaCellValue;
					if (formulaCellValue != null && formulaCellValue.IsCellsNameDependent)
					{
						CellIndex cellIndex = WorksheetPropertyBagBase.ConvertLongToCellIndex(range.Start);
						CellIndex toIndex = WorksheetPropertyBagBase.ConvertLongToCellIndex(range.End);
						ICellValue cellValue = formulaCellValue.CloneAndTranslate(base.Worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex, eventArgs, false);
						if (!formulaCellValue.Equals(cellValue))
						{
							this[cellIndex, toIndex].SetValue(cellValue);
						}
					}
				}
			}
		}

		void UnmergeBeforeInsert(CellRange range, InsertShiftType shiftType)
		{
			switch (shiftType)
			{
			case InsertShiftType.Right:
				this.UnmergeBeforeInsertRemove(range, ShiftType.Right);
				return;
			case InsertShiftType.Down:
				this.UnmergeBeforeInsertRemove(range, ShiftType.Down);
				return;
			default:
				return;
			}
		}

		void UnmergeBeforeRemove(CellRange range, RemoveShiftType shiftType)
		{
			switch (shiftType)
			{
			case RemoveShiftType.Left:
				this.UnmergeBeforeInsertRemove(range, ShiftType.Left);
				return;
			case RemoveShiftType.Up:
				this.UnmergeBeforeInsertRemove(range, ShiftType.Up);
				return;
			default:
				return;
			}
		}

		void UnmergeBeforeInsertRemove(CellRange range, ShiftType shiftType)
		{
			List<CellRange> list = new List<CellRange>(this.MergedCellRanges);
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.MergedCellRanges.CanInsertOrRemoveInternal(shiftType, list[i], range))
				{
					base.Worksheet.Cells[list[i]].Unmerge();
				}
			}
		}

		bool CheckForFilterRangeBeforeInsert(CellRange cellRange, InsertShiftType shiftType)
		{
			switch (shiftType)
			{
			case InsertShiftType.Right:
				return base.Worksheet.Filter.CanInsertOrRemove(ShiftType.Right, cellRange);
			case InsertShiftType.Down:
				return base.Worksheet.Filter.CanInsertOrRemove(ShiftType.Down, cellRange);
			default:
				throw new NotSupportedException("Invalid insert shift type.");
			}
		}

		bool CheckForFilterRangeBeforeRemove(CellRange cellRange, RemoveShiftType shiftType)
		{
			switch (shiftType)
			{
			case RemoveShiftType.Left:
				return base.Worksheet.Filter.CanInsertOrRemove(ShiftType.Left, cellRange);
			case RemoveShiftType.Up:
				return base.Worksheet.Filter.CanInsertOrRemove(ShiftType.Up, cellRange);
			default:
				throw new NotSupportedException("Invalid remove shift type.");
			}
		}

		internal void UpdateSpreadsheetNameDependentCellValues(string oldName, string newName)
		{
			this.UpdateFormulaCellValues((FormulaCellValue value) => value.ContainsSpreadsheetName, delegate(FormulaCellValue value)
			{
				value.Translate(oldName, newName);
			});
		}

		internal void UpdateSheetNameDependentCellValues(Worksheet renamedWorksheet)
		{
			this.UpdateFormulaCellValues((FormulaCellValue value) => value.IsSheetNameDependent, delegate(FormulaCellValue value)
			{
				value.Translate(renamedWorksheet);
			});
		}

		internal void UpdateWorkbookNameDependentCellValues(Workbook renamedWorkbook)
		{
			this.UpdateFormulaCellValues((FormulaCellValue value) => value.IsWorkbookNameDependent, delegate(FormulaCellValue value)
			{
				value.Translate(renamedWorkbook);
			});
		}

		void UpdateFormulaCellValues(Func<FormulaCellValue, bool> condition, Action<FormulaCellValue> action)
		{
			ICompressedList<ICellValue> propertyValueCollection = base.Worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
			using (new UpdateScope(new Action(this.PropertyBag.SuspendPropertyChanged), new Action(this.PropertyBag.ResumePropertyChanged)))
			{
				foreach (Range<long, ICellValue> range in propertyValueCollection.GetNonDefaultRanges())
				{
					FormulaCellValue formulaCellValue = range.Value as FormulaCellValue;
					if (formulaCellValue != null && condition(formulaCellValue))
					{
						action(formulaCellValue);
					}
				}
			}
		}

		internal override void CopyFromOverride(WorksheetEntityBase fromWorksheetEntity, CopyContext context, IEnumerable<IProperty> properties)
		{
			base.CopyFromOverride(fromWorksheetEntity, context, properties.Except(new IProperty[] { this.ValueProperty }));
			IPropertyDefinition<string> styleNameProperty = CellPropertyDefinitions.StyleNameProperty;
			ICompressedList<string> propertyValueCollection = this.PropertyBag.GetPropertyValueCollection<string>(styleNameProperty);
			Range<long, string>[] array = propertyValueCollection.GetNonDefaultRanges().ToArray<Range<long, string>>();
			for (long num = 0L; num < (long)array.Length; num += 1L)
			{
				Range<long, string> range = array[(int)(checked((IntPtr)num))];
				string value;
				if (context.OriginalToRenamedStyleNames.TryGetValue(range.Value, out value))
				{
					propertyValueCollection.SetValue(range.Start, range.End, value);
				}
			}
			IPropertyDefinition<ICellValue> property = CellPropertyDefinitions.ValueProperty;
			ICompressedList<ICellValue> propertyValueCollection2 = fromWorksheetEntity.PropertyBagBase.GetPropertyValueCollection<ICellValue>(property);
			ICompressedList<ICellValue> propertyValueCollection3 = context.TargetWorksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(property);
			foreach (Range<long, ICellValue> range2 in propertyValueCollection2.GetNonDefaultRanges())
			{
				if (range2.Value.ValueType == CellValueType.Formula)
				{
					FormulaCellValue formulaCellValue = (FormulaCellValue)range2.Value;
					for (long num2 = range2.Start; num2 <= range2.End; num2 += 1L)
					{
						int num3;
						int num4;
						WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(num2, out num3, out num4);
						FormulaCellValue value2 = ((ICopyable<FormulaCellValue>)formulaCellValue).Copy(context);
						propertyValueCollection3.SetValue(num2, value2);
					}
				}
				else
				{
					propertyValueCollection3.SetValue(range2.Start, range2.End, range2.Value);
				}
			}
			this.MergedCellRanges.CopyFrom(((Cells)fromWorksheetEntity).MergedCellRanges);
		}

		internal override void Clear()
		{
			base.Clear();
			this.MergedCellRanges.RemoveMergedRanges(this.MergedCellRanges.Ranges.ToList<CellRange>());
		}

		public CellSelection GetCellSelection(int rowIndex, int columnIndex)
		{
			return this.GetCellSelection(new CellIndex(rowIndex, columnIndex));
		}

		public CellSelection GetCellSelection(CellIndex cellIndex)
		{
			return this.GetCellSelection(cellIndex, cellIndex);
		}

		public CellSelection GetCellSelection(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			return this.GetCellSelection(new CellRange(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex));
		}

		public CellSelection GetCellSelection(CellIndex fromIndex, CellIndex toIndex)
		{
			return this.GetCellSelection(new CellRange(fromIndex, toIndex));
		}

		public CellSelection GetCellSelection(CellRange cellRange)
		{
			return this.GetCellSelection(new CellRange[] { cellRange });
		}

		public CellSelection GetCellSelection(IEnumerable<CellRange> cellRanges)
		{
			return new CellSelection(base.Worksheet, cellRanges);
		}

		public bool TryGetContainingMergedRange(CellIndex cellIndex, out CellRange mergedRange)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return this.MergedCellRanges.TryGetContainingMergedRange(cellIndex, out mergedRange);
		}

		internal bool TryGetContainingMergedRange(int rowIndex, int columnIndex, out CellRange mergedRange)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			return this.MergedCellRanges.TryGetContainingMergedRange(rowIndex, columnIndex, out mergedRange);
		}

		public bool GetIsMerged(CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			CellRange cellRange;
			return this.TryGetContainingMergedRange(cellIndex, out cellRange);
		}

		public CellMergeState GetMergeState(CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return this.GetMergeState(cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public CellMergeState GetMergeState(int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			CellMergeState result = CellMergeState.NotMerged;
			CellRange cellRange;
			if (base.Worksheet.Cells.TryGetContainingMergedRange(rowIndex, columnIndex, out cellRange))
			{
				if (cellRange.FromIndex.RowIndex == rowIndex && cellRange.FromIndex.ColumnIndex == columnIndex)
				{
					result = CellMergeState.TopLeftCellInMergedRange;
				}
				else
				{
					result = CellMergeState.NonTopLeftCellInMergedRange;
				}
			}
			return result;
		}

		public IEnumerable<CellRange> GetMergedCellRanges()
		{
			return this.MergedCellRanges.Ranges;
		}

		public IEnumerable<CellRange> GetContainingMergedRanges(CellRange range)
		{
			ISet<CellRange> ranges = this.mergedCellRanges.GetIntersectingMergedRanges(range);
			foreach (CellRange mergedRange in ranges)
			{
				if (range.Contains(mergedRange))
				{
					yield return mergedRange;
				}
			}
			yield break;
		}

		internal void OnCellRangeInsertedOrRemoved(CellRange range, RangeType rangeType, bool isRemove)
		{
			this.OnCellRangeInsertedOrRemoved(new CellRangeInsertedOrRemovedEventArgs(range, rangeType, isRemove));
		}

		public event EventHandler<CellRangeInsertedOrRemovedEventArgs> CellRangeInsertedOrRemoved;

		protected virtual void OnCellRangeInsertedOrRemoved(CellRangeInsertedOrRemovedEventArgs args)
		{
			if (this.CellRangeInsertedOrRemoved != null)
			{
				this.CellRangeInsertedOrRemoved(this, args);
			}
		}

		internal event EventHandler MergedCellsChanging;

		void OnMergedCellsChanging()
		{
			if (this.MergedCellsChanging != null)
			{
				this.MergedCellsChanging(this, EventArgs.Empty);
			}
		}

		public event EventHandler<MergedCellRangesChangedEventArgs> MergedCellsChanged;

		protected virtual void OnMergedCellsChanged(MergedCellRangesChangedEventArgs args)
		{
			if (this.MergedCellsChanged != null)
			{
				this.MergedCellsChanged(this, args);
			}
		}

		public event EventHandler<CellPropertyChangedEventArgs> CellPropertyChanged;

		protected virtual void OnCellPropertyChanged(CellPropertyChangedEventArgs args)
		{
			if (this.CellPropertyChanged != null)
			{
				this.CellPropertyChanged(this, args);
			}
		}

		readonly CellsPropertyBag propertyBag;

		readonly MergedCellRanges mergedCellRanges;

		readonly IProperty<ICellValue> valueProperty;

		readonly IProperty<IDataValidationRule> dataValidationRuleProperty;
	}
}
