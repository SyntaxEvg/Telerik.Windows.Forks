using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class FindAndReplaceHelper
	{
		public static bool MoreThan(this CellIndex left, CellIndex right, FindBy searchBy)
		{
			if (searchBy == FindBy.Rows)
			{
				return left > right;
			}
			return !(left == null) && (left.ColumnIndex > right.ColumnIndex || (left.ColumnIndex == right.ColumnIndex && left.RowIndex > right.RowIndex));
		}

		public static bool MoreThan(this WorksheetCellIndex left, WorksheetCellIndex right, FindBy searchBy)
		{
			Guard.ThrowExceptionIfNotEqual<Workbook>(left.Worksheet.Workbook, right.Worksheet.Workbook, "worksheets should be in the same workbook");
			int worksheetIndex = FindAndReplaceHelper.GetWorksheetIndex(left.Worksheet);
			int worksheetIndex2 = FindAndReplaceHelper.GetWorksheetIndex(right.Worksheet);
			if (worksheetIndex == worksheetIndex2)
			{
				return left.CellIndex.MoreThan(right.CellIndex, searchBy);
			}
			return worksheetIndex > worksheetIndex2;
		}

		public static bool LessOrEqual(this WorksheetCellIndex left, WorksheetCellIndex right, FindBy searchBy)
		{
			return !left.MoreThan(right, searchBy);
		}

		public static IEnumerable<FindResult> OrderResults(FindOptions findOptions, IEnumerable<FindResult> findResults)
		{
			IEnumerable<FindResult> enumerable = findResults.TakeWhile((FindResult result) => result.FoundCell.LessOrEqual(findOptions.StartCell, findOptions.FindBy));
			IEnumerable<FindResult> first = findResults.Skip(enumerable.Count<FindResult>());
			findResults = first.Concat(enumerable);
			return findResults;
		}

		public static bool FindInRawValue(Range<CellIndex, ICellValue> range, FindOptions findOptions)
		{
			string rawValue = range.Value.RawValue;
			return FindAndReplaceHelper.FindInString(rawValue, findOptions);
		}

		public static bool FindInResultValue(Cells cells, Range<CellIndex, ICellValue> range, FindOptions findOptions)
		{
			CellValueFormat value = cells[range.Start].GetFormat().Value;
			string resultValueAsString = range.Value.GetResultValueAsString(value);
			return FindAndReplaceHelper.FindInString(resultValueAsString, findOptions);
		}

		public static bool FindInString(string rangeValue, FindOptions findOptions)
		{
			return !string.IsNullOrEmpty(rangeValue) && Regex.Match(rangeValue, findOptions.FindWhatRegex, findOptions.FindWhatRegexOptions).Success;
		}

		public static int GetWorksheetIndex(Worksheet worksheet)
		{
			return worksheet.Workbook.Worksheets.IndexOf(worksheet);
		}
	}
}
