using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class CellsPropertyBag : WorksheetPropertyBagBase
	{
		internal bool IsSortInProgress
		{
			get
			{
				return this.sortCounter.IsUpdateInProgress;
			}
		}

		protected override long ToItemIndex
		{
			get
			{
				return SpreadsheetDefaultValues.CellCount - 1L;
			}
		}

		public CellsPropertyBag()
		{
			this.suspendPropertyChangedCounter = new BeginEndCounter();
			this.changedProperties = new HashSet<IPropertyDefinition>();
			this.sortCounter = new BeginEndCounter(new Action(this.OnSortCompleted));
			this.sortedRanges = new HashSet<CellRange>();
			this.RegisterProperty<ICellValue>(CellPropertyDefinitions.ValueProperty);
			this.RegisterProperty<IDataValidationRule>(CellPropertyDefinitions.DataValidationRuleProperty);
		}

		internal bool CanInsertRow(int itemCount)
		{
			return this.EndLostRowRangeIsNonEmpty(itemCount);
		}

		bool EndLostRowRangeIsNonEmpty(int itemCount)
		{
			CellRange cellRange = new CellRange(SpreadsheetDefaultValues.RowCount - itemCount, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
			return this.IsCellRangeEmpty(cellRange);
		}

		void InsertRowInternal(int index, int itemCount)
		{
			this.TranslateRow(index, itemCount, true);
			base.InvalidateAllPropertiesUsedCellRange();
		}

		internal void InsertEmptyRow(int index, int itemCount)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			CellRange cellRange = new CellRange(index, usedCellRange.FromIndex.ColumnIndex, Math.Min(usedCellRange.ToIndex.RowIndex + itemCount, SpreadsheetDefaultValues.RowCount - 1), usedCellRange.ToIndex.ColumnIndex);
			this.InsertRowInternal(index, itemCount);
			if (index <= usedCellRange.ToIndex.RowIndex)
			{
				this.RaisePropertyChangedForRegisteredProperties(cellRange);
			}
		}

		internal void InsertRowAndSetValues(int index, int itemCount, CellsPropertyBag oldValues)
		{
			this.InsertRowInternal(index, itemCount);
			CellRange cellRange = CellRange.FromRowRange(index, index + itemCount - 1);
			this.CopyFromWithoutPropertyChanged(cellRange, oldValues);
			CellRange usedCellRange = base.GetUsedCellRange();
			CellRange cellRange2 = new CellRange(index, usedCellRange.FromIndex.ColumnIndex, Math.Min(usedCellRange.ToIndex.RowIndex + itemCount, SpreadsheetDefaultValues.RowCount - 1), usedCellRange.ToIndex.ColumnIndex);
			if (index <= usedCellRange.ToIndex.RowIndex)
			{
				this.RaisePropertyChangedForRegisteredProperties(cellRange2);
			}
		}

		internal void RemoveRow(int index, int itemCount)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			Dictionary<IPropertyDefinition, CellRange> dictionary = null;
			if (index <= usedCellRange.ToIndex.RowIndex)
			{
				CellRange cellRange = CellRange.FromRowRange(index, index + itemCount - 1);
				dictionary = this.GetChangedProperties((CellRange propertyUsedCellRange) => new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, propertyUsedCellRange.ToIndex.RowIndex, Math.Min(cellRange.ToIndex.ColumnIndex, propertyUsedCellRange.ToIndex.ColumnIndex)));
			}
			this.TranslateRow(index, -itemCount, false);
			this.Clear(new CellRange(SpreadsheetDefaultValues.RowCount - itemCount, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1));
			this.RaisePropertyChangedForRegisteredProperties(dictionary);
			base.InvalidateAllPropertiesUsedCellRange();
		}

		internal bool CanInsertColumn(int itemCount)
		{
			return this.EndLostColumnRangeIsNonEmpty(itemCount);
		}

		bool EndLostColumnRangeIsNonEmpty(int itemCount)
		{
			CellRange cellRange = new CellRange(0, SpreadsheetDefaultValues.ColumnCount - itemCount, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
			return this.IsCellRangeEmpty(cellRange);
		}

		void InsertColumnInternal(int index, int itemCount)
		{
			this.TranslateColumn(index, itemCount);
			if (index > 0)
			{
				CellRange cellRange = new CellRange(0, index - 1, SpreadsheetDefaultValues.RowCount - 1, index - 1);
				long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.FromIndex);
				for (int i = index; i < index + itemCount; i++)
				{
					long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(0, i);
					base.Copy(fromIndex, (long)SpreadsheetDefaultValues.RowCount, toIndex, true);
				}
			}
			base.InvalidateAllPropertiesUsedCellRange();
		}

		internal void InsertEmptyColumn(int index, int itemCount)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			CellRange cellRange = new CellRange(usedCellRange.FromIndex.RowIndex, index, usedCellRange.ToIndex.RowIndex, Math.Min(usedCellRange.ToIndex.ColumnIndex + itemCount, SpreadsheetDefaultValues.ColumnCount - 1));
			this.InsertColumnInternal(index, itemCount);
			if (index <= usedCellRange.ToIndex.ColumnIndex)
			{
				this.RaisePropertyChangedForRegisteredProperties(cellRange);
			}
		}

		internal void InsertColumnAndSetValues(int index, int itemCount, CellsPropertyBag oldValues)
		{
			this.InsertColumnInternal(index, itemCount);
			CellRange cellRange = CellRange.FromColumnRange(index, index + itemCount - 1);
			this.CopyFromWithoutPropertyChanged(cellRange, oldValues);
			CellRange usedCellRange = base.GetUsedCellRange();
			CellRange cellRange2 = new CellRange(usedCellRange.FromIndex.RowIndex, index, usedCellRange.ToIndex.RowIndex, Math.Min(usedCellRange.ToIndex.ColumnIndex + itemCount, SpreadsheetDefaultValues.ColumnCount - 1));
			if (index <= usedCellRange.ToIndex.ColumnIndex)
			{
				this.RaisePropertyChangedForRegisteredProperties(cellRange2);
			}
		}

		internal void RemoveColumn(int index, int itemCount)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			Dictionary<IPropertyDefinition, CellRange> dictionary = null;
			if (index <= usedCellRange.ToIndex.ColumnIndex)
			{
				CellRange cellRange = CellRange.FromColumnRange(index, index + itemCount - 1);
				dictionary = this.GetChangedProperties((CellRange propertyUsedCellRange) => new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, Math.Min(cellRange.ToIndex.RowIndex, propertyUsedCellRange.ToIndex.RowIndex), propertyUsedCellRange.ToIndex.ColumnIndex));
			}
			this.TranslateColumn(index, -itemCount);
			this.RaisePropertyChangedForRegisteredProperties(dictionary);
			base.InvalidateAllPropertiesUsedCellRange();
		}

		internal bool CanInsertCells(CellRange range, InsertShiftType shiftType)
		{
			return this.EndLostRangeIsNonEmpty(range, shiftType);
		}

		bool EndLostRangeIsNonEmpty(CellRange range, InsertShiftType shiftType)
		{
			CellRange cellRange;
			if (shiftType == InsertShiftType.Down)
			{
				cellRange = new CellRange(SpreadsheetDefaultValues.RowCount - range.RowCount, range.FromIndex.ColumnIndex, SpreadsheetDefaultValues.RowCount - 1, range.ToIndex.ColumnIndex);
			}
			else
			{
				if (shiftType != InsertShiftType.Right)
				{
					throw new NotSupportedException("Shift type not supported.");
				}
				cellRange = new CellRange(range.FromIndex.RowIndex, SpreadsheetDefaultValues.ColumnCount - range.ColumnCount, range.ToIndex.RowIndex, SpreadsheetDefaultValues.ColumnCount - 1);
			}
			return this.IsCellRangeEmpty(cellRange);
		}

		void InsertCellsInternal(CellRange cellRange, InsertShiftType insertShiftType)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			if (insertShiftType == InsertShiftType.Right)
			{
				CellRange cellRange2 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex, Math.Min(usedCellRange.ToIndex.ColumnIndex + cellRange.ColumnCount, SpreadsheetDefaultValues.ColumnCount - 1));
				int num = SpreadsheetDefaultValues.RowCount * cellRange.ColumnCount;
				foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange2).Reverse<LongRange>())
				{
					if (longRange.End + (long)num <= SpreadsheetDefaultValues.CellCount)
					{
						this.TranslateWithoutPropertyChanged(num, longRange);
					}
				}
				this.Clear(cellRange);
				if (cellRange2.FromIndex.ColumnIndex <= 0)
				{
					goto IL_1BA;
				}
				LongRange longRange2 = new LongRange(WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange2.FromIndex.RowIndex, cellRange2.FromIndex.ColumnIndex - 1), WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange2.ToIndex.RowIndex, cellRange2.FromIndex.ColumnIndex - 1));
				using (IEnumerator<LongRange> enumerator2 = WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange).Reverse<LongRange>().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						LongRange longRange3 = enumerator2.Current;
						if (longRange3.End + (long)num <= SpreadsheetDefaultValues.CellCount)
						{
							base.Copy(longRange2.Start, longRange2.Length, longRange3.Start, true);
						}
					}
					goto IL_1BA;
				}
			}
			CellRange cellRange3 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, Math.Min(usedCellRange.ToIndex.RowIndex + cellRange.RowCount, SpreadsheetDefaultValues.RowCount - 1), cellRange.ToIndex.ColumnIndex);
			this.Translate(cellRange3, (long)cellRange.RowCount, true);
			IL_1BA:
			base.InvalidateAllPropertiesUsedCellRange();
		}

		internal void InsertEmptyCells(CellRange cellRange, InsertShiftType insertShiftType)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			CellRange cellRange2 = new CellRange(usedCellRange.FromIndex.RowIndex, usedCellRange.FromIndex.ColumnIndex, usedCellRange.ToIndex.RowIndex + 1, usedCellRange.ToIndex.ColumnIndex + 1);
			if (!cellRange2.IntersectsWith(cellRange))
			{
				return;
			}
			int toRowIndex;
			int toColumnIndex;
			if (insertShiftType == InsertShiftType.Right)
			{
				toRowIndex = System.Math.Min(cellRange.ToIndex.RowIndex, usedCellRange.ToIndex.RowIndex);
				toColumnIndex = System.Math.Min(usedCellRange.ToIndex.ColumnIndex + cellRange.ColumnCount, SpreadsheetDefaultValues.ColumnCount - 1);
			}
			else
			{
				toRowIndex = System.Math.Min(usedCellRange.ToIndex.RowIndex + cellRange.RowCount, SpreadsheetDefaultValues.RowCount - 1);
				toColumnIndex = System.Math.Min(cellRange.ToIndex.ColumnIndex, usedCellRange.ToIndex.ColumnIndex);
			}
			CellRange cellRange3 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, toRowIndex, toColumnIndex);
			this.InsertCellsInternal(cellRange, insertShiftType);
			this.RaisePropertyChangedForRegisteredProperties(cellRange3);
		}

		internal void InsertCellsAndSetValues(CellRange cellRange, InsertShiftType insertShiftType, CellsPropertyBag oldValues)
		{
			this.InsertCellsInternal(cellRange, insertShiftType);
			this.CopyFromWithoutPropertyChanged(cellRange, oldValues);
			CellRange usedCellRange = base.GetUsedCellRange();
			int toRowIndex;
			int toColumnIndex;
			if (insertShiftType == InsertShiftType.Right)
			{
				toRowIndex = System.Math.Min(cellRange.ToIndex.RowIndex, usedCellRange.ToIndex.RowIndex);
				toColumnIndex = System.Math.Min(usedCellRange.ToIndex.ColumnIndex + cellRange.ColumnCount, SpreadsheetDefaultValues.ColumnCount - 1);
			}
			else
			{
				toRowIndex = System.Math.Min(usedCellRange.ToIndex.RowIndex + cellRange.RowCount, SpreadsheetDefaultValues.RowCount - 1);
				toColumnIndex = System.Math.Min(cellRange.ToIndex.ColumnIndex, usedCellRange.ToIndex.ColumnIndex);
			}
			CellRange cellRange2 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, toRowIndex, toColumnIndex);
			this.RaisePropertyChangedForRegisteredProperties(cellRange2);
		}

		internal void RemoveCells(CellRange cellRange, RemoveShiftType shiftType)
		{
			CellRange usedCellRange = base.GetUsedCellRange();
			if (!usedCellRange.IntersectsWith(cellRange))
			{
				return;
			}
			Dictionary<IPropertyDefinition, CellRange> dictionary;
			if (shiftType == RemoveShiftType.Left)
			{
				dictionary = this.GetChangedProperties((CellRange propertyUsedCellRange) => new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, Math.Min(cellRange.ToIndex.RowIndex, propertyUsedCellRange.ToIndex.RowIndex), propertyUsedCellRange.ToIndex.ColumnIndex));
				this.RemoveCellsLeft(cellRange, usedCellRange);
			}
			else
			{
				dictionary = this.GetChangedProperties((CellRange propertyUsedCellRange) => new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, propertyUsedCellRange.ToIndex.RowIndex, Math.Min(cellRange.ToIndex.ColumnIndex, propertyUsedCellRange.ToIndex.ColumnIndex)));
				this.RemoveCellsUp(cellRange, usedCellRange);
			}
			this.RaisePropertyChangedForRegisteredProperties(dictionary);
			base.InvalidateAllPropertiesUsedCellRange();
		}

		void RemoveCellsLeft(CellRange cellRange, CellRange usedCellRange)
		{
			int fromColumnIndex = System.Math.Min(cellRange.ToIndex.ColumnIndex + 1, SpreadsheetDefaultValues.ColumnCount - 1);
			int toColumnIndex = Math.Max(usedCellRange.ToIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex + 1);
			CellRange cellRange2 = new CellRange(cellRange.FromIndex.RowIndex, fromColumnIndex, cellRange.ToIndex.RowIndex, toColumnIndex);
			if (cellRange.ToIndex.ColumnIndex != SpreadsheetDefaultValues.ColumnCount - 1)
			{
				int num = -SpreadsheetDefaultValues.RowCount * cellRange.ColumnCount;
				foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange2))
				{
					if (longRange.Start + (long)num >= 0L)
					{
						this.TranslateWithoutPropertyChanged(num, longRange);
					}
				}
			}
			CellRange cellRange3 = new CellRange(cellRange2.FromIndex.RowIndex, cellRange2.ToIndex.ColumnIndex - cellRange.ColumnCount + 1, cellRange2.ToIndex.RowIndex, Math.Min(usedCellRange.ToIndex.ColumnIndex, cellRange2.ToIndex.ColumnIndex + 1));
			this.Clear(cellRange3);
		}

		void RemoveCellsUp(CellRange cellRange, CellRange usedCellRange)
		{
			CellRange cellRange2 = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, Math.Max(cellRange.ToIndex.RowIndex, usedCellRange.ToIndex.RowIndex), cellRange.ToIndex.ColumnIndex);
			this.Translate(cellRange2, (long)(-(long)cellRange.RowCount), false);
			CellRange cellRange3 = new CellRange(Math.Max(usedCellRange.ToIndex.RowIndex - cellRange.RowCount + 1, cellRange.FromIndex.RowIndex), cellRange.FromIndex.ColumnIndex, Math.Max(usedCellRange.ToIndex.RowIndex, cellRange.ToIndex.RowIndex), cellRange.ToIndex.ColumnIndex);
			this.Clear(cellRange3);
		}

		internal void RemoveCell(int rowIndex, int columnIndex, int itemCount, RemoveShiftType shiftType)
		{
			if (shiftType == RemoveShiftType.Left)
			{
				for (int i = columnIndex; i < SpreadsheetDefaultValues.ColumnCount - itemCount; i++)
				{
					base.Copy(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, i + itemCount), WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, i), false);
				}
				for (int j = SpreadsheetDefaultValues.ColumnCount - itemCount; j < SpreadsheetDefaultValues.ColumnCount; j++)
				{
					base.Clear(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, j));
				}
			}
			else
			{
				CellRange cellRange = new CellRange(rowIndex, columnIndex, SpreadsheetDefaultValues.RowCount - 1, columnIndex);
				this.Translate(cellRange, (long)(-(long)itemCount), false);
				this.Clear(new CellRange(SpreadsheetDefaultValues.RowCount - itemCount, columnIndex, SpreadsheetDefaultValues.RowCount - 1, columnIndex));
			}
			base.InvalidateAllPropertiesUsedCellRange();
		}

		void TranslateWithoutPropertyChanged(int offset, LongRange range)
		{
			foreach (IPropertyDefinition property in base.GetRegisteredProperties())
			{
				ICompressedList propertyValueCollection = base.GetPropertyValueCollection(property);
				ICompressedList value = propertyValueCollection.GetValue(range.Start, range.End);
				ICompressedList value2 = value.Offset((long)offset);
				propertyValueCollection.SetValue(value2);
			}
		}

		void CopyFromWithoutPropertyChanged(CellRange cellRange, CellsPropertyBag oldValues)
		{
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				foreach (IPropertyDefinition property in base.GetRegisteredProperties())
				{
					ICompressedList propertyValueCollection = oldValues.GetPropertyValueCollection(property);
					ICompressedList propertyValueCollection2 = base.GetPropertyValueCollection(property);
					propertyValueCollection2.SetValue(propertyValueCollection.GetValue(longRange.Start, longRange.End));
				}
			}
		}

		Dictionary<IPropertyDefinition, CellRange> GetChangedProperties(Func<CellRange, CellRange> getCellRange)
		{
			Dictionary<IPropertyDefinition, CellRange> dictionary = new Dictionary<IPropertyDefinition, CellRange>();
			foreach (IPropertyDefinition propertyDefinition in base.GetRegisteredProperties())
			{
				CellRange usedCellRange = base.GetUsedCellRange(propertyDefinition);
				if (!usedCellRange.Equals(CellRange.Empty) || !this.IsPropertyDefault(propertyDefinition, 0L))
				{
					CellRange value = getCellRange(usedCellRange);
					dictionary.Add(propertyDefinition, value);
				}
			}
			return dictionary;
		}

		void Clear(CellRange cellRange)
		{
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				base.Clear(longRange.Start, longRange.End);
			}
		}

		void TranslateRow(int index, int offset, bool useSameValueAsPrevious)
		{
			CellRange cellRange = new CellRange(index, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
			this.Translate(cellRange, (long)offset, useSameValueAsPrevious);
		}

		void TranslateColumn(int index, int offset)
		{
			CellRange cellRange = new CellRange(0, index, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
			long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.FromIndex);
			long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.ToIndex);
			base.Translate(fromIndex, toIndex, (long)(offset * SpreadsheetDefaultValues.RowCount), false);
		}

		void Translate(CellRange cellRange, long offset, bool useSameValueAsPrevious)
		{
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				base.Translate(longRange.Start, longRange.End, offset, useSameValueAsPrevious);
			}
		}

		void RaisePropertyChangedForRegisteredProperties(CellRange cellRange)
		{
			foreach (IPropertyDefinition property in base.GetRegisteredProperties())
			{
				CellRange usedCellRange = base.GetUsedCellRange(property);
				if ((!usedCellRange.Equals(CellRange.Empty) || !this.IsPropertyDefault(property, 0L)) && cellRange.IntersectsWith(usedCellRange))
				{
					CellRange cellRange2 = new CellRange(cellRange.FromIndex, cellRange.ToIndex);
					this.OnPropertyChanged(new CellPropertyChangedEventArgs(property, cellRange2));
				}
			}
		}

		void RaisePropertyChangedForRegisteredProperties(Dictionary<IPropertyDefinition, CellRange> changedProperties)
		{
			if (changedProperties != null)
			{
				foreach (KeyValuePair<IPropertyDefinition, CellRange> keyValuePair in changedProperties)
				{
					this.OnPropertyChanged(new CellPropertyChangedEventArgs(keyValuePair.Key, keyValuePair.Value));
				}
			}
		}

		bool IsCellRangeEmpty(CellRange cellRange)
		{
			IEnumerable<LongRange> enumerable = WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange);
			foreach (LongRange longRange in enumerable)
			{
				long start = longRange.Start;
				long end = longRange.End;
				foreach (IPropertyDefinition property in base.GetRegisteredProperties())
				{
					ICompressedList propertyValueCollection = base.GetPropertyValueCollection(property);
					if (propertyValueCollection.GetLastNonEmptyRangeEnd(start, end) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		internal bool IsDefault(long index)
		{
			foreach (IPropertyDefinition property in CellPropertyDefinitions.AllPropertyDefinitions)
			{
				if (base.GetPropertyValueCollection(property).GetLastNonEmptyRangeEnd(index, index) != null)
				{
					return false;
				}
			}
			return true;
		}

		bool IsPropertyDefault(IPropertyDefinition property, long index)
		{
			return base.GetPropertyValueCollection(property).GetLastNonEmptyRangeEnd(index, index) == null;
		}

		internal void SuspendPropertyChanged()
		{
			this.suspendPropertyChangedCounter.BeginUpdate();
		}

		internal void ResumePropertyChanged()
		{
			this.ResumePropertyChanged(null);
		}

		internal void ResumePropertyChanged(CellRange updatedRange)
		{
			this.suspendPropertyChangedCounter.EndUpdate();
			if (!this.suspendPropertyChangedCounter.IsUpdateInProgress && this.changedProperties.Count > 0)
			{
				foreach (IPropertyDefinition property in this.changedProperties)
				{
					if (updatedRange == null)
					{
						updatedRange = base.GetUsedCellRange(property);
					}
					this.OnPropertyChanged(new CellPropertyChangedEventArgs(property, updatedRange));
				}
				this.changedProperties.Clear();
			}
		}

		public ICompressedList<T> GetPropertyValue<T>(IPropertyDefinition<T> property, CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.FromIndex);
			long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.ToIndex);
			ICompressedList<T> compressedList = new CompressedList<T>(fromIndex, toIndex, property.DefaultValue);
			ICompressedList<T> propertyValueCollection = base.GetPropertyValueCollection<T>(property);
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				ICompressedList<T> value = propertyValueCollection.GetValue(longRange.Start, longRange.End);
				compressedList.SetValue(value);
			}
			return compressedList;
		}

		public T GetPropertyValue<T>(IPropertyDefinition<T> property, CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellIndex);
			return base.GetPropertyValue<T>(property, index);
		}

		public void SetPropertyValue<T>(IPropertyDefinition<T> property, CellIndex cellIndex, T value)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "index");
			this.SetPropertyValue<T>(property, cellIndex.ToCellRange(), value);
		}

		public void SetPropertyValue<T>(IPropertyDefinition<T> property, CellRange cellRange, T value)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "range");
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				base.SetPropertyValue<T>(property, longRange.Start, longRange.End, value);
			}
		}

		public void SetPropertyValue<T>(IPropertyDefinition<T> property, CellRange cellRange, ICompressedList values)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			Guard.ThrowExceptionIfNull<ICompressedList>(values, "values");
			ICompressedList propertyValueCollection = base.GetPropertyValueCollection<T>(property);
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				ICompressedList value = values.GetValue(longRange.Start, longRange.End);
				propertyValueCollection.SetValue(value);
			}
			this.OnPropertyChanged(new CellPropertyChangedEventArgs(property, cellRange));
		}

		public void UpdatePropertyValue<T>(IPropertyDefinition<T> property, CellRange cellRange, Func<T, T> newValueTransform)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			Guard.ThrowExceptionIfNull<Func<T, T>>(newValueTransform, "newValueTransform");
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				base.UpdatePropertyValue<T>(property, longRange.Start, longRange.End, newValueTransform);
			}
		}

		public void ClearPropertyValue<T>(IPropertyDefinition<T> property, CellIndex cellIndex)
		{
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellIndex);
			base.ClearPropertyValue<T>(property, index);
		}

		public void ClearPropertyValue<T>(IPropertyDefinition<T> property, CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "range");
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				base.ClearPropertyValue<T>(property, longRange.Start, longRange.End);
			}
		}

		public void CopyPropertiesFrom(CellsPropertyBag fromProperties, CellRange cellRange)
		{
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				base.CopyPropertiesFrom(fromProperties, longRange.Start, longRange.End);
			}
		}

		protected override void OnPropertyChanged(IPropertyDefinition property, long fromIndex, long toIndex)
		{
			base.InvalidatePropertyUsedCellRange(property);
			if (this.suspendPropertyChangedCounter.IsUpdateInProgress)
			{
				if (!this.changedProperties.Contains(property))
				{
					this.changedProperties.Add(property);
					return;
				}
			}
			else
			{
				int fromRowIndex;
				int fromColumnIndex;
				WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(fromIndex, out fromRowIndex, out fromColumnIndex);
				int toRowIndex;
				int toColumnIndex;
				WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(toIndex, out toRowIndex, out toColumnIndex);
				CellRange cellRange = new CellRange(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
				this.OnPropertyChanged(new CellPropertyChangedEventArgs(property, cellRange));
			}
		}

		internal IEnumerable<CellRange> GetValueContainingCellRanges<T>(IPropertyDefinition<T> properyDefinition, T value)
		{
			ICompressedList<T> propertyValueCollection = base.GetPropertyValueCollection<T>(properyDefinition);
			IEnumerable<Range<long, T>> nonDefaultRanges = propertyValueCollection.GetNonDefaultRanges();
			IEnumerable<Range<long, T>> source = nonDefaultRanges.Where(delegate(Range<long, T> p)
			{
				T value2 = p.Value;
				return value2.Equals(value);
			});
			IOrderedEnumerable<Range<long, T>> orderedEnumerable = from p in source
				orderby p.Start
				select p;
			List<CellRange> list = new List<CellRange>();
			foreach (Range<long, T> range in orderedEnumerable)
			{
				list.Add(WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End));
			}
			while (this.MergeCellRanges(list))
			{
			}
			return list;
		}

		bool MergeCellRanges(List<CellRange> usedRanges)
		{
			for (int i = 0; i < usedRanges.Count; i++)
			{
				for (int j = i + 1; j < usedRanges.Count; j++)
				{
					CellRange cellRange = usedRanges[i];
					CellRange cellRange2 = usedRanges[j];
					if (this.CanMergeCellRanges(cellRange, cellRange2))
					{
						usedRanges.Remove(cellRange);
						usedRanges.Remove(cellRange2);
						usedRanges.Insert(0, this.MergeCellRanges(cellRange, cellRange2));
						return true;
					}
				}
			}
			return false;
		}

		bool CanMergeCellRanges(CellRange firstRange, CellRange secondRange)
		{
			return firstRange.RowCount == secondRange.RowCount && firstRange.FromIndex.RowIndex == secondRange.FromIndex.RowIndex && (firstRange.ToIndex.ColumnIndex == secondRange.FromIndex.ColumnIndex - 1 || firstRange.FromIndex.ColumnIndex == secondRange.ToIndex.ColumnIndex + 1);
		}

		CellRange MergeCellRanges(CellRange firstRange, CellRange secondRange)
		{
			return new CellRange(Math.Min(firstRange.FromIndex.RowIndex, secondRange.FromIndex.RowIndex), Math.Min(firstRange.FromIndex.ColumnIndex, secondRange.FromIndex.ColumnIndex), Math.Max(firstRange.ToIndex.RowIndex, secondRange.ToIndex.RowIndex), Math.Max(firstRange.ToIndex.ColumnIndex, secondRange.ToIndex.ColumnIndex));
		}

		protected override ICompressedList<T> GetLocalPropertyValueRespectingRowsColumns<T>(IPropertyDefinition<T> property, Worksheet worksheet, long fromIndex, long toIndex)
		{
			int num;
			int num2;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(fromIndex, out num, out num2);
			int num3;
			int num4;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(toIndex, out num3, out num4);
			ICompressedList<T> propertyValue = base.GetPropertyValue<T>(property, fromIndex, toIndex);
			ICompressedList<T> compressedList = new CompressedList<T>(propertyValue.FromIndex, propertyValue.ToIndex, property.DefaultValue);
			if (worksheet.Columns.ContainsPropertyDefinition(property))
			{
				ICompressedList<T> value = worksheet.Columns.PropertyBag.GetPropertyValueCollection<T>(property).GetValue((long)num2, (long)num4);
				foreach (Range<long, T> range in value.GetNonDefaultRanges())
				{
					int num5 = (int)range.Start;
					while ((long)num5 <= range.End)
					{
						long fromIndex2 = WorksheetPropertyBagBase.ConvertCellIndexToLong(num, num5);
						long toIndex2 = WorksheetPropertyBagBase.ConvertCellIndexToLong(num3, num5);
						compressedList.SetValue(fromIndex2, toIndex2, range.Value);
						num5++;
					}
				}
			}
			if (worksheet.Rows.ContainsPropertyDefinition(property))
			{
				ICompressedList<T> value2 = worksheet.Rows.PropertyBag.GetPropertyValueCollection<T>(property).GetValue((long)num, (long)num3);
				foreach (Range<long, T> range2 in value2.GetNonDefaultRanges())
				{
					for (int i = num2; i <= num4; i++)
					{
						long fromIndex3 = WorksheetPropertyBagBase.ConvertCellIndexToLong((int)range2.Start, i);
						long toIndex3 = WorksheetPropertyBagBase.ConvertCellIndexToLong((int)range2.End, i);
						compressedList.SetValue(fromIndex3, toIndex3, range2.Value);
					}
				}
			}
			foreach (Range<long, T> range3 in propertyValue.GetNonDefaultRanges())
			{
				compressedList.SetValue(range3.Start, range3.End, range3.Value);
			}
			return compressedList;
		}

		protected override bool TryGetRowOrColumnPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, long index, out T result)
		{
			int num;
			int num2;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out num, out num2);
			result = worksheet.Rows.PropertyBag.GetPropertyValueRespectingStyle<T>(property, worksheet, (long)num);
			if (TelerikHelper.EqualsOfT<T>(result, property.DefaultValue))
			{
				result = worksheet.Columns.PropertyBag.GetPropertyValueRespectingStyle<T>(property, worksheet, (long)num2);
				if (TelerikHelper.EqualsOfT<T>(result, property.DefaultValue))
				{
					return false;
				}
			}
			return true;
		}

		protected override bool TryGetRowOrColumnPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, long fromIndex, long toIndex, out ICompressedList<T> rowOrColumnPropertyValuesRespectingStyle)
		{
			rowOrColumnPropertyValuesRespectingStyle = new CompressedList<T>(fromIndex, toIndex, property.DefaultValue);
			int num;
			int num2;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(fromIndex, out num, out num2);
			int num3;
			int num4;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(toIndex, out num3, out num4);
			for (int i = num2; i <= num4; i++)
			{
				T propertyValueRespectingStyle = worksheet.Columns.PropertyBag.GetPropertyValueRespectingStyle<T>(property, worksheet, (long)i);
				lock (property)
				{
					if (!TelerikHelper.EqualsOfT<T>(propertyValueRespectingStyle, property.DefaultValue))
					{
						long fromIndex2 = WorksheetPropertyBagBase.ConvertCellIndexToLong(num, i);
						long toIndex2 = WorksheetPropertyBagBase.ConvertCellIndexToLong(num3, i);
						rowOrColumnPropertyValuesRespectingStyle.SetValue(fromIndex2, toIndex2, propertyValueRespectingStyle);
					}
				}
			}
			IEnumerable<Range<long, T>> nonDefaultRanges = worksheet.Rows.PropertyBag.GetPropertyValueCollection<T>(property).GetValue((long)num, (long)num3).GetNonDefaultRanges();
			IEnumerable<Range<long, string>> nonDefaultRanges2 = worksheet.Rows.PropertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty).GetValue((long)num, (long)num3).GetNonDefaultRanges();
			foreach (Range<long, string> range in nonDefaultRanges2)
			{
				CellStyle byName = worksheet.Workbook.Styles.GetByName(range.Value);
				if (byName.GetIsPropertyIncluded(property))
				{
					for (int j = num2; j <= num4; j++)
					{
						long fromIndex3 = WorksheetPropertyBagBase.ConvertCellIndexToLong((int)range.Start, j);
						long toIndex3 = WorksheetPropertyBagBase.ConvertCellIndexToLong((int)range.End, j);
						rowOrColumnPropertyValuesRespectingStyle.SetValue(fromIndex3, toIndex3, byName.GetPropertyValue<T>(property));
					}
				}
			}
			foreach (Range<long, T> range2 in nonDefaultRanges)
			{
				for (int k = num2; k <= num4; k++)
				{
					long fromIndex4 = WorksheetPropertyBagBase.ConvertCellIndexToLong((int)range2.Start, k);
					long toIndex4 = WorksheetPropertyBagBase.ConvertCellIndexToLong((int)range2.End, k);
					rowOrColumnPropertyValuesRespectingStyle.SetValue(fromIndex4, toIndex4, range2.Value);
				}
			}
			return true;
		}

		internal void BeginSort()
		{
			this.sortCounter.BeginUpdate();
		}

		internal void EndSort()
		{
			this.sortCounter.EndUpdate();
		}

		internal void Sort(Worksheet worksheet, CellRange range, int[] sortedIndexes)
		{
			List<Action> list = new List<Action>();
			using (IEnumerator<IPropertyDefinition> enumerator = CellPropertyDefinitions.AllPropertyDefinitions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IPropertyDefinition propertyDefinition = enumerator.Current;
					list.Add(delegate
					{
						for (int i = range.FromIndex.ColumnIndex; i <= range.ToIndex.ColumnIndex; i++)
						{
							long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(range.FromIndex.RowIndex, i);
							long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(range.ToIndex.RowIndex, i);
							if (propertyDefinition == CellPropertyDefinitions.ValueProperty)
							{
								this.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty).Sort(fromIndex, toIndex, sortedIndexes, delegate(long newIndex, ValueBox<ICellValue> value)
								{
									if (value == null)
									{
										return value;
									}
									FormulaCellValue formulaCellValue = value.Value as FormulaCellValue;
									if (formulaCellValue != null)
									{
										int targetRowIndex;
										int targetColumnIndex;
										WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(newIndex, out targetRowIndex, out targetColumnIndex);
										return new ValueBox<ICellValue>(formulaCellValue.CloneAndTranslate(worksheet, targetRowIndex, targetColumnIndex, false));
									}
									return value;
								});
							}
							else
							{
								this.GetPropertyValueCollection(propertyDefinition).Sort(fromIndex, toIndex, sortedIndexes);
							}
						}
					});
				}
			}
			TasksHelper.DoAsync(list);
			this.OnAfterSort(range);
			this.OnSorted(new SortedEventArgs(range, sortedIndexes));
		}

		void OnAfterSort(CellRange range)
		{
			if (this.IsSortInProgress)
			{
				if (!this.sortedRanges.Contains(range))
				{
					this.sortedRanges.Add(range);
					return;
				}
			}
			else
			{
				this.FireSortPropertyChanged(range);
			}
		}

		void OnSortCompleted()
		{
			foreach (CellRange range in this.sortedRanges)
			{
				this.FireSortPropertyChanged(range);
			}
			this.sortedRanges.Clear();
		}

		void FireSortPropertyChanged(CellRange range)
		{
			foreach (IPropertyDefinition property in CellPropertyDefinitions.AllPropertyDefinitions)
			{
				this.OnPropertyChanged(new CellPropertyChangedEventArgs(property, range)
				{
					ShouldRespectAffectLayoutProperty = false
				});
			}
		}

		internal void OnStyleChanged(CellStyle style, IEnumerable<IPropertyDefinition> changedProperties)
		{
			List<Tuple<long, long>> list = new List<Tuple<long, long>>();
			foreach (Range<long, string> range in base.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty))
			{
				if (range.Value == style.Name)
				{
					list.Add(new Tuple<long, long>(range.Start, range.End));
				}
			}
			foreach (IPropertyDefinition property in changedProperties)
			{
				foreach (Tuple<long, long> tuple in list)
				{
					this.OnPropertyChanged(property, tuple.Item1, tuple.Item2);
				}
			}
		}

		public event EventHandler<CellPropertyChangedEventArgs> PropertyChanged;

		protected virtual void OnPropertyChanged(CellPropertyChangedEventArgs args)
		{
			Guard.ThrowExceptionIfNull<CellPropertyChangedEventArgs>(args, "args");
			if (this.suspendPropertyChangedCounter.IsUpdateInProgress)
			{
				if (!this.changedProperties.Contains(args.Property))
				{
					this.changedProperties.Add(args.Property);
					return;
				}
			}
			else if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, args);
			}
		}

		internal event EventHandler<SortedEventArgs> Sorted;

		internal void OnSorted(SortedEventArgs args)
		{
			Guard.ThrowExceptionIfNull<SortedEventArgs>(args, "args");
			if (this.Sorted != null)
			{
				this.Sorted(this, args);
			}
		}

		readonly BeginEndCounter suspendPropertyChangedCounter;

		readonly HashSet<IPropertyDefinition> changedProperties;

		readonly BeginEndCounter sortCounter;

		readonly HashSet<CellRange> sortedRanges;
	}
}
