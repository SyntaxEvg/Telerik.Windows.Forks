using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries
{
	static class FillDataSeriesHelper
	{
		public static double[] FillWithOperator(double initialValue, int resultSeriesLength, double stepValue, Func<double, double, double?> applyStepValue, double? stopValue)
		{
			if (stopValue == null)
			{
				stopValue = new double?((stepValue >= 0.0) ? double.MaxValue : double.MinValue);
			}
			List<double> list = new List<double>();
			list.Add(initialValue);
			double? num = applyStepValue(list[list.Count - 1], stepValue);
			while (list.Count < resultSeriesLength && ((stepValue >= 0.0 && num.Value.LessOrEqualDouble(stopValue.Value)) || (stepValue < 0.0 && num.Value.GreaterOrEqualDouble(stopValue.Value))))
			{
				if (num == null)
				{
					return null;
				}
				list.Add(num.Value);
				num = applyStepValue(list.Last<double>(), stepValue);
			}
			return list.ToArray();
		}

		static bool ContainsNegativeNumbers(double?[] initialValues)
		{
			for (int i = 0; i < initialValues.Length; i++)
			{
				if (initialValues[i] != null && initialValues[i].Value < 0.0)
				{
					return true;
				}
			}
			return false;
		}

		public static double[] FillLinear(double initialValue, int resultSeriesLength, double stepValue, double? stopValue = null)
		{
			return FillDataSeriesHelper.FillWithOperator(initialValue, resultSeriesLength, stepValue, (double value, double step) => new double?(value + step), stopValue);
		}

		public static double[] FillLinearTrend(double?[] initialValues, int resultSeriesLength)
		{
			IEnumerable<double?> source = from x in initialValues
				where x != null
				select x;
			if (initialValues.Length == 1 || source.Count<double?>() == 1)
			{
				return FillDataSeriesHelper.FillLinear(source.First<double?>().Value, resultSeriesLength, 1.0, null);
			}
			return InterpolationExtensions.FillLinear(initialValues, resultSeriesLength, 1.0, true);
		}

		public static double[] FillExponential(double initialValue, int resultSeriesLength, double stepValue, double? stopValue)
		{
			if (stepValue < 0.0 && stopValue != null)
			{
				return null;
			}
			return FillDataSeriesHelper.FillWithOperator(initialValue, resultSeriesLength, stepValue, (double value, double step) => new double?(value * step), stopValue);
		}

		public static double[] FillExponentialTrend(double?[] initialValues, int resultSeriesLength)
		{
			if (FillDataSeriesHelper.ContainsNegativeNumbers(initialValues))
			{
				return null;
			}
			return InterpolationExtensions.FillExponential(initialValues, resultSeriesLength, 1.0, true);
		}

		static DateTime AddWeekDays(double days, DateTime dateTime)
		{
			dateTime = dateTime.AddDays(days);
			while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
			{
				dateTime = dateTime.AddDays(1.0);
			}
			return dateTime;
		}

		public static double[] FillDate(double initialValue, DateUnitType dateUnitType, int resultSeriesLength, double stepValue, double? stopValue)
		{
			Func<DateTime, double, double> transformDate = null;
			double lastResidue = 0.0;
			Func<double, double, double?> applyStepValue = delegate(double value, double step)
			{
				DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(value);
				if (dateTime == null)
				{
					return null;
				}
				double value2 = transformDate(dateTime.Value, step);
				return new double?(value2);
			};
			switch (dateUnitType)
			{
			case DateUnitType.Day:
				transformDate = delegate(DateTime dateTime, double step)
				{
					double num = (double)((int)Math.Floor(step));
					dateTime = dateTime.AddDays(num);
					double num2 = step - num;
					return FormatHelper.ConvertDateTimeToDouble(dateTime) + num2;
				};
				break;
			case DateUnitType.Weekday:
				transformDate = delegate(DateTime dateTime, double step)
				{
					int num = (int)Math.Floor(step);
					double days = step - (double)num;
					dateTime = FillDataSeriesHelper.AddWeekDays(days, dateTime);
					for (int i = 0; i < num; i++)
					{
						dateTime = FillDataSeriesHelper.AddWeekDays(1.0, dateTime);
					}
					return FormatHelper.ConvertDateTimeToDouble(dateTime);
				};
				break;
			case DateUnitType.Month:
				transformDate = delegate(DateTime dateTime, double step)
				{
					int num = (int)Math.Floor(step);
					double num2 = step - (double)num;
					lastResidue += num2;
					if (lastResidue >= 1.0)
					{
						lastResidue -= 1.0;
						num++;
					}
					dateTime = dateTime.AddMonths(num);
					return FormatHelper.ConvertDateTimeToDouble(dateTime);
				};
				break;
			case DateUnitType.Year:
				transformDate = delegate(DateTime dateTime, double step)
				{
					int num = (int)Math.Floor(step);
					double num2 = step - (double)num;
					lastResidue += num2;
					if (lastResidue >= 1.0)
					{
						lastResidue -= 1.0;
						num++;
					}
					dateTime = dateTime.AddYears(num);
					return FormatHelper.ConvertDateTimeToDouble(dateTime);
				};
				break;
			default:
				throw new InvalidOperationException();
			}
			return FillDataSeriesHelper.FillWithOperator(initialValue, resultSeriesLength, stepValue, applyStepValue, stopValue);
		}

		public static double GetStepValue(double?[] initialValues)
		{
			double[] array = FillDataSeriesHelper.FillLinearTrend(initialValues, initialValues.Length + 2);
			double num = array[array.Length - 1] - array[array.Length - 2];
			if (num != 0.0)
			{
				return num;
			}
			return 1.0;
		}

		public static CellRange[] ExtractMergedRanges(Worksheet worksheet, CellIndex[] indexes, int initialIndexesCount)
		{
			List<CellRange> list = new List<CellRange>();
			for (int i = 0; i < initialIndexesCount; i++)
			{
				CellIndex cellIndex = indexes[i];
				CellRange item;
				if (worksheet.Cells.TryGetContainingMergedRange(cellIndex, out item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		public static void UnmergeMergedRanges(Worksheet worksheet, CellRange[] mergedRanges)
		{
			foreach (CellRange cellRange in mergedRanges)
			{
				worksheet.Cells[cellRange].Unmerge();
			}
		}

		static bool Contains(this CellRange[] ranges, CellIndex cellIndex)
		{
			foreach (CellRange cellRange in ranges)
			{
				if (cellRange.Contains(cellIndex))
				{
					return true;
				}
			}
			return false;
		}

		public static void RepeatMergedRanges(Worksheet worksheet, CellRange[] mergedRanges, CellIndex[] indexes, int initialIndexesCount)
		{
			bool flag = indexes[indexes.Length - 1].ColumnIndex - indexes[0].ColumnIndex > 0;
			HashSet<CellIndex> mergedIndexes = new HashSet<CellIndex>();
			Action<CellIndex> action = delegate(CellIndex cellIndex)
			{
				if (!mergedIndexes.Contains(cellIndex))
				{
					mergedIndexes.Add(cellIndex);
				}
			};
			foreach (CellRange cellRange in mergedRanges)
			{
				for (int j = 0; j < initialIndexesCount; j++)
				{
					if (!mergedRanges.Contains(indexes[j]))
					{
						action(indexes[j]);
					}
					else if (flag)
					{
						if (cellRange.FromIndex.ColumnIndex == indexes[j].ColumnIndex)
						{
							action(indexes[j]);
						}
					}
					else if (cellRange.FromIndex.RowIndex == indexes[j].RowIndex)
					{
						action(indexes[j]);
					}
				}
			}
			int count = mergedIndexes.Count;
			for (int k = initialIndexesCount; k < indexes.Length; k++)
			{
				action(indexes[k]);
			}
			foreach (CellRange cellRange2 in FillDataSeriesHelper.RepeatMergedRanges(mergedRanges, mergedIndexes.ToArray<CellIndex>(), count, false))
			{
				if ((cellRange2.ColumnCount > 1 || cellRange2.RowCount > 1) && !worksheet.Cells.MergedCellRanges.IntersectsWithMergedRanges(cellRange2))
				{
					worksheet.Cells[cellRange2].Merge();
				}
			}
		}

		public static CellRange[] RepeatMergedRanges(CellRange[] mergedRanges, CellIndex[] indexes, int initialIndexesCount, bool isDirectionReversed = false)
		{
			List<CellRange> list = new List<CellRange>();
			List<CellIndex> list2 = new List<CellIndex>();
			if (isDirectionReversed)
			{
				CellIndex cellIndex3 = indexes.FirstOrDefault((CellIndex cellIndex) => cellIndex == mergedRanges.First<CellRange>().FromIndex);
				if (cellIndex3 != null)
				{
					list2.Add(cellIndex3);
				}
				else
				{
					list2.Add(indexes[0]);
				}
			}
			else
			{
				for (int i = 0; i < initialIndexesCount; i++)
				{
					list2.Add(indexes[i]);
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				CellIndex mergedRangeToIndex = FillDataSeriesHelper.GetMergedRangeToIndex(mergedRanges, list2[j]);
				if (mergedRangeToIndex == null)
				{
					list.Add(new CellRange(list2[j], list2[j]));
				}
				else
				{
					list.Add(new CellRange(list2[j], mergedRangeToIndex));
				}
			}
			if (list.Count == 0)
			{
				return new CellRange[0];
			}
			int rowsOffset = FillDataSeriesHelper.GetRowsOffset(indexes);
			int columnsOffset = FillDataSeriesHelper.GetColumnsOffset(indexes);
			CellIndex cellIndex2 = indexes[indexes.Length - 1];
			int num = 0;
			CellRange cellRange;
			do
			{
				CellRange lastRange = list.Last<CellRange>();
				CellIndex cellRangeFromIndex = FillDataSeriesHelper.GetCellRangeFromIndex(rowsOffset, columnsOffset, isDirectionReversed, lastRange);
				if (cellRangeFromIndex == null)
				{
					break;
				}
				CellIndex cellRangeToIndex = FillDataSeriesHelper.GetCellRangeToIndex(cellRangeFromIndex, list[num]);
				cellRange = new CellRange(cellRangeFromIndex, cellRangeToIndex);
				bool flag = FillDataSeriesHelper.ShouldAddToRangeCollection(isDirectionReversed, cellRange, cellIndex2, rowsOffset, columnsOffset);
				if (flag)
				{
					list.Add(cellRange);
				}
				num++;
				num %= initialIndexesCount;
			}
			while ((!isDirectionReversed) ? (cellRange.ToIndex.RowIndex < cellIndex2.RowIndex || cellRange.ToIndex.ColumnIndex < cellIndex2.ColumnIndex) : (cellRange.FromIndex.RowIndex > cellIndex2.RowIndex || cellRange.FromIndex.ColumnIndex > cellIndex2.ColumnIndex));
			return list.ToArray();
		}

		static CellIndex GetMergedRangeToIndex(CellRange[] mergedRanges, CellIndex result)
		{
			foreach (CellRange cellRange in mergedRanges)
			{
				if (cellRange.Contains(result))
				{
					return cellRange.ToIndex;
				}
			}
			return null;
		}

		static int GetColumnsOffset(CellIndex[] indexes)
		{
			int num = Math.Abs(indexes[indexes.Length - 1].ColumnIndex - indexes[0].ColumnIndex);
			if (num <= 0)
			{
				return 0;
			}
			return 1;
		}

		static int GetRowsOffset(CellIndex[] indexes)
		{
			int num = Math.Abs(indexes[indexes.Length - 1].RowIndex - indexes[0].RowIndex);
			if (num <= 0)
			{
				return 0;
			}
			return 1;
		}

		static CellIndex GetCellRangeToIndex(CellIndex fromIndex, CellRange cellRange)
		{
			return new CellIndex(fromIndex.RowIndex + cellRange.RowCount - 1, fromIndex.ColumnIndex + cellRange.ColumnCount - 1);
		}

		static CellIndex GetCellRangeFromIndex(int rowsOffset, int columnsOffset, bool isDirectionReversed, CellRange lastRange)
		{
			CellIndex result = null;
			if (rowsOffset > 0)
			{
				if (isDirectionReversed)
				{
					if (lastRange.FromIndex.RowIndex - lastRange.RowCount - 1 + rowsOffset >= 0)
					{
						result = new CellIndex(lastRange.FromIndex.RowIndex - lastRange.RowCount - 1 + rowsOffset, lastRange.FromIndex.ColumnIndex);
					}
				}
				else
				{
					result = new CellIndex(lastRange.FromIndex.RowIndex + lastRange.RowCount - 1 + rowsOffset, lastRange.FromIndex.ColumnIndex);
				}
			}
			else if (isDirectionReversed)
			{
				if (lastRange.FromIndex.ColumnIndex - lastRange.ColumnCount - 1 + columnsOffset >= 0)
				{
					result = new CellIndex(lastRange.FromIndex.RowIndex, lastRange.FromIndex.ColumnIndex - lastRange.ColumnCount - 1 + columnsOffset);
				}
			}
			else
			{
				result = new CellIndex(lastRange.FromIndex.RowIndex, lastRange.FromIndex.ColumnIndex + lastRange.ColumnCount - 1 + columnsOffset);
			}
			return result;
		}

		static bool ShouldAddToRangeCollection(bool isDirectionReversed, CellRange newRange, CellIndex lastIndex, int rowsOffset, int columnsOffset)
		{
			return (!isDirectionReversed) ? ((newRange.ToIndex.RowIndex <= lastIndex.RowIndex && rowsOffset > 0) || (newRange.ToIndex.ColumnIndex <= lastIndex.ColumnIndex && columnsOffset > 0)) : ((newRange.FromIndex.RowIndex >= lastIndex.RowIndex && rowsOffset > 0) || (newRange.FromIndex.ColumnIndex >= lastIndex.ColumnIndex && columnsOffset > 0));
		}

		public static void RepeatMergedRanges(Worksheet worksheet, CellRange[] mergedCellRanges, CellRange cellRange, int maxInitialValuesCount, CellOrientation cellOrientation, bool isDirectionReversed)
		{
			List<CellRange> list = new List<CellRange>();
			int num;
			if (cellOrientation == CellOrientation.Horizontal)
			{
				num = (int)Math.Ceiling((double)cellRange.ColumnCount / (double)maxInitialValuesCount);
			}
			else
			{
				num = (int)Math.Ceiling((double)cellRange.RowCount / (double)maxInitialValuesCount);
			}
			int num2 = 0;
			do
			{
				for (int i = 0; i < mergedCellRanges.Length; i++)
				{
					int num3 = num2 * maxInitialValuesCount;
					CellRange cellRange2;
					if (cellOrientation == CellOrientation.Horizontal)
					{
						if (!isDirectionReversed)
						{
							cellRange2 = mergedCellRanges[i].Offset(0, num3);
						}
						else
						{
							cellRange2 = mergedCellRanges[i].Offset(0, -num3);
						}
					}
					else if (!isDirectionReversed)
					{
						cellRange2 = mergedCellRanges[i].Offset(num3, 0);
					}
					else
					{
						cellRange2 = mergedCellRanges[i].Offset(-num3, 0);
					}
					if (cellRange.Contains(cellRange2))
					{
						list.Add(cellRange2);
					}
				}
				num2++;
			}
			while (num2 < num);
			foreach (CellRange cellRange3 in list)
			{
				worksheet.Cells[cellRange3].Merge();
			}
		}
	}
}
