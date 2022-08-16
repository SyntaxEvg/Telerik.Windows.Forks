using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	class FilterManager
	{
		public FilterManager(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.relativeIndexToHiddenRowsState = new Dictionary<int, ICompressedList<bool>>();
			this.hiddenRows = new Dictionary<int, int>();
		}

		public void FilterSheet(CellRange range, IFilter filter)
		{
			Guard.ThrowExceptionIfNull<IFilter>(filter, "filter");
			int relativeColumnIndex = filter.RelativeColumnIndex;
			if (this.relativeIndexToHiddenRowsState.ContainsKey(relativeColumnIndex))
			{
				throw new FilteringException("This column is already filtered. Remove the filter before applying a new one.", new InvalidOperationException("This column is already filtered. Remove the filter before applying a new one."), "Spreadsheet_Filtering_ColumnFiltered");
			}
			CompressedList<bool> emptyCompressedList = this.GetEmptyCompressedList();
			this.relativeIndexToHiddenRowsState.Add(relativeColumnIndex, emptyCompressedList);
			if (range == null)
			{
				return;
			}
			int columnIndex = range.FromIndex.ColumnIndex + relativeColumnIndex;
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				object value = filter.GetValue(this.worksheet.Cells, i, columnIndex);
				if (!filter.ShouldShowValue(value))
				{
					if (!this.hiddenRows.ContainsKey(i))
					{
						this.SetHiddenOutsideCommand(i);
						this.hiddenRows.Add(i, 1);
					}
					else
					{
						Dictionary<int, int> dictionary;
						int key;
						(dictionary = this.hiddenRows)[key = i] = dictionary[key] + 1;
					}
					emptyCompressedList.SetValue((long)i, true);
				}
			}
		}

		public void ClearFilter(IFilter filter)
		{
			Guard.ThrowExceptionIfNull<IFilter>(filter, "filter");
			ICompressedList<bool> compressedList = this.relativeIndexToHiddenRowsState[filter.RelativeColumnIndex];
			IEnumerable<Range<long, bool>> nonDefaultRanges = compressedList.GetNonDefaultRanges();
			this.UnhideHiddenRanges(nonDefaultRanges);
			this.relativeIndexToHiddenRowsState.Remove(filter.RelativeColumnIndex);
		}

		void UnhideHiddenRanges(IEnumerable<Range<long, bool>> allHiddenRanges)
		{
			foreach (Range<long, bool> range in allHiddenRanges)
			{
				int num = (int)range.Start;
				int num2 = (int)range.End;
				for (int i = num; i <= num2; i++)
				{
					Dictionary<int, int> dictionary;
					int key;
					(dictionary = this.hiddenRows)[key = i] = dictionary[key] - 1;
					if (this.hiddenRows[i] == 0)
					{
						this.hiddenRows.Remove(i);
						this.ClearHiddenOutsideCommand(i);
					}
				}
			}
		}

		internal ICompressedList<bool> GetHiddenRowsState(int relativeColumnIndex)
		{
			ICompressedList<bool> result;
			if (!this.relativeIndexToHiddenRowsState.ContainsKey(relativeColumnIndex))
			{
				result = this.GetEmptyCompressedList();
			}
			else
			{
				result = this.relativeIndexToHiddenRowsState[relativeColumnIndex];
			}
			return result;
		}

		public void RestoreHiddenRowsState(int relativeColumnIndex, ICompressedList<bool> filterState)
		{
			this.relativeIndexToHiddenRowsState[relativeColumnIndex] = filterState;
			IEnumerable<Range<long, bool>> nonDefaultRanges = filterState.GetNonDefaultRanges();
			foreach (Range<long, bool> range in nonDefaultRanges)
			{
				int num = (int)range.Start;
				int num2 = (int)range.End;
				for (int i = num; i <= num2; i++)
				{
					if (this.hiddenRows.ContainsKey(i))
					{
						Dictionary<int, int> dictionary;
						int key;
						(dictionary = this.hiddenRows)[key = i] = dictionary[key] + 1;
					}
					else
					{
						this.hiddenRows.Add(i, 1);
						this.SetHiddenOutsideCommand(i);
					}
				}
			}
		}

		public ICompressedList<bool> GetRowsHiddenUniquelyByFilter(int relativeColumnIndex)
		{
			ICompressedList<bool> compressedList = new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), false);
			if (!this.relativeIndexToHiddenRowsState.ContainsKey(relativeColumnIndex))
			{
				return compressedList;
			}
			ICompressedList<bool> compressedList2 = this.relativeIndexToHiddenRowsState[relativeColumnIndex];
			IEnumerable<Range<long, bool>> nonDefaultRanges = compressedList2.GetNonDefaultRanges();
			foreach (Range<long, bool> range in nonDefaultRanges)
			{
				int num = (int)range.Start;
				int num2 = (int)range.End;
				for (int i = num; i <= num2; i++)
				{
					if (this.hiddenRows[i] == 1)
					{
						compressedList.SetValue((long)i, true);
					}
				}
			}
			return compressedList;
		}

		public void MoveFilter(int newIndex, int oldIndex)
		{
			ICompressedList<bool> value = this.relativeIndexToHiddenRowsState[oldIndex];
			this.relativeIndexToHiddenRowsState[newIndex] = value;
			this.relativeIndexToHiddenRowsState.Remove(oldIndex);
		}

		public void RearrangeRows(int absoluteRearrangeIndex, int delta, ShiftType shiftType)
		{
			if (shiftType != ShiftType.Down && shiftType != ShiftType.Up)
			{
				throw new ArgumentException("The shift type must be ShiftType.Down or ShiftType.Up", "shiftType");
			}
			this.RearrangeIndexToHiddenRowStateDictionary(absoluteRearrangeIndex, delta, shiftType);
			this.RearrangeHiddenRowsDictionary(absoluteRearrangeIndex, delta, shiftType);
		}

		void RearrangeIndexToHiddenRowStateDictionary(int absoluteRearrangeIndex, int delta, ShiftType shiftType)
		{
			Dictionary<int, ICompressedList<bool>> dictionary = new Dictionary<int, ICompressedList<bool>>();
			foreach (KeyValuePair<int, ICompressedList<bool>> keyValuePair in this.relativeIndexToHiddenRowsState)
			{
				ICompressedList<bool> value = keyValuePair.Value;
				IEnumerable<Range<long, bool>> nonDefaultRanges = value.GetNonDefaultRanges();
				CompressedList<bool> emptyCompressedList = this.GetEmptyCompressedList();
				foreach (Range<long, bool> range in nonDefaultRanges)
				{
					int num = (int)range.Start;
					int num2 = (int)range.End;
					for (int i = num; i <= num2; i++)
					{
						if (i < absoluteRearrangeIndex)
						{
							emptyCompressedList.SetValue((long)i, true);
						}
						else if (i >= absoluteRearrangeIndex)
						{
							if (shiftType == ShiftType.Down)
							{
								emptyCompressedList.SetValue((long)(i + delta), true);
							}
							else if (i > absoluteRearrangeIndex + delta)
							{
								emptyCompressedList.SetValue((long)(i - delta), true);
							}
						}
					}
				}
				dictionary.Add(keyValuePair.Key, emptyCompressedList);
			}
			this.relativeIndexToHiddenRowsState = dictionary;
		}

		void RearrangeHiddenRowsDictionary(int absoluteRearrangeIndex, int delta, ShiftType shiftType)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (KeyValuePair<int, int> keyValuePair in this.hiddenRows)
			{
				int key = keyValuePair.Key;
				if (key < absoluteRearrangeIndex)
				{
					dictionary.Add(key, keyValuePair.Value);
				}
				else if (key < absoluteRearrangeIndex)
				{
					dictionary.Add(key, keyValuePair.Value);
				}
				else if (key >= absoluteRearrangeIndex)
				{
					if (shiftType == ShiftType.Down)
					{
						dictionary.Add(key + delta, keyValuePair.Value);
					}
					else if (key > absoluteRearrangeIndex + delta)
					{
						dictionary.Add(key - delta, keyValuePair.Value);
					}
				}
			}
			this.hiddenRows = dictionary;
		}

		CompressedList<bool> GetEmptyCompressedList()
		{
			return new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1), false);
		}

		void SetHiddenOutsideCommand(int row)
		{
			this.SetHiddenOutsideCommand(row, row);
		}

		void SetHiddenOutsideCommand(int startRow, int endRow)
		{
			this.worksheet.Rows.PropertyBag.SetPropertyValue<bool>(RowColumnPropertyBagBase.HiddenProperty, startRow, endRow, true);
		}

		void ClearHiddenOutsideCommand(int row)
		{
			this.ClearHiddenOutsideCommand(row, row);
		}

		void ClearHiddenOutsideCommand(int startRow, int endRow)
		{
			this.worksheet.Rows.PropertyBag.ClearPropertyValue<bool>(RowColumnPropertyBagBase.HiddenProperty, startRow, endRow);
		}

		internal void CopyFrom(FilterManager other)
		{
			this.relativeIndexToHiddenRowsState = new Dictionary<int, ICompressedList<bool>>(other.relativeIndexToHiddenRowsState);
			this.hiddenRows = new Dictionary<int, int>(other.hiddenRows);
			this.relativeIndexToHiddenRowsState = new Dictionary<int, ICompressedList<bool>>();
			foreach (KeyValuePair<int, ICompressedList<bool>> keyValuePair in other.relativeIndexToHiddenRowsState)
			{
				ICompressedList<bool> value = keyValuePair.Value;
				CompressedList<bool> compressedList = new CompressedList<bool>(value);
				this.relativeIndexToHiddenRowsState[keyValuePair.Key] = compressedList;
				foreach (Range<long, bool> range in value.GetNonDefaultRanges())
				{
					compressedList.SetValue(range.Start, range.End, range.Value);
				}
			}
		}

		readonly Worksheet worksheet;

		Dictionary<int, ICompressedList<bool>> relativeIndexToHiddenRowsState;

		Dictionary<int, int> hiddenRows;
	}
}
