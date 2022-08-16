using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting.Conditions;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	static class SortManager
	{
		public static void Sort(Cells cells, CellRange range, ISortCondition[] sortConditions)
		{
			using (new UpdateScope(new Action(cells.PropertyBag.BeginSort), new Action(cells.PropertyBag.EndSort)))
			{
				range = range.Intersect(cells.PropertyBag.GetUsedCellRange());
				foreach (ISortCondition sortCondition in sortConditions)
				{
					IWorksheetSortCondition worksheetSortCondition = sortCondition as IWorksheetSortCondition;
					if (worksheetSortCondition != null)
					{
						worksheetSortCondition.SetWorksheet(cells.Worksheet);
					}
					ICompressedList<bool> propertyValueCollection = cells.Worksheet.Rows.PropertyBag.GetPropertyValueCollection<bool>(RowColumnPropertyBagBase.HiddenProperty);
					int hiddenRowsCount = SortManager.GetHiddenRowsCount(range, propertyValueCollection);
					SortValue[] array = SortManager.ExtractValuesToSort(range, hiddenRowsCount, sortCondition, propertyValueCollection, cells);
					Array.Sort<SortValue>(array, sortCondition.Comparer);
					int[] sortedIndexes = SortManager.ExtractSortedIndexes(range, propertyValueCollection, array);
					SetSortCommandContext context = new SetSortCommandContext(cells.Worksheet, range, sortCondition, sortedIndexes);
					cells.Worksheet.ExecuteCommand<SetSortCommandContext>(WorkbookCommands.SetSort, context);
				}
			}
		}

		static SortValue[] ExtractValuesToSort(CellRange range, int hiddenRowsCount, ISortCondition sortCondition, ICompressedList<bool> hiddenRows, Cells cells)
		{
			SortValue[] array = new SortValue[range.RowCount - hiddenRowsCount];
			int columnIndex = range.FromIndex.ColumnIndex + sortCondition.RelativeIndex;
			int num = 0;
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				if (!hiddenRows.GetValue((long)i))
				{
					array[num] = new SortValue(i - range.FromIndex.RowIndex, sortCondition.GetValue(cells, i, columnIndex));
					num++;
				}
			}
			return array;
		}

		static int[] ExtractSortedIndexes(CellRange range, ICompressedList<bool> hiddenRows, SortValue[] array)
		{
			int[] array2 = new int[range.RowCount];
			int num = 0;
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				int num2 = i - range.FromIndex.RowIndex;
				if (hiddenRows.GetValue((long)i))
				{
					array2[num2] = num2;
				}
				else
				{
					array2[num2] = array[num].Index;
					num++;
				}
			}
			return array2;
		}

		static int GetHiddenRowsCount(CellRange range, ICompressedList<bool> hiddenRows)
		{
			long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(range.FromIndex.RowIndex, 0);
			long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(range.ToIndex.RowIndex, 0);
			ICompressedList<bool> value = hiddenRows.GetValue(fromIndex, toIndex);
			IEnumerable<Range<long, bool>> nonDefaultRanges = value.GetNonDefaultRanges();
			int num = 0;
			foreach (Range<long, bool> range2 in nonDefaultRanges)
			{
				num += (int)(range2.End - range2.Start) + 1;
			}
			return num;
		}

		public static bool AreSortConditionsDuplicating(ISortCondition[] sortConditions)
		{
			for (int i = 0; i < sortConditions.Length - 1; i++)
			{
				for (int j = i + 1; j < sortConditions.Length; j++)
				{
					if (sortConditions[i].Equals(sortConditions[j]))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
