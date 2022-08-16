using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	abstract class NumberRelatedAutoFillGeneratorBase : IAutoFillGenerator, INamedObject
	{
		public abstract string Name { get; }

		protected abstract double ValuesRangeUpperBound { get; }

		protected abstract double ValuesRangeLowerBound { get; }

		protected abstract bool AllowReversingOfTheFitBelowTheLowerBoundValue { get; }

		protected virtual int MinAllowedInitialValuesCount
		{
			get
			{
				return 1;
			}
		}

		public void FillSeries(Worksheet worksheet, CellIndex[] indexes, int initialIndexesCount)
		{
			if (initialIndexesCount < this.MinAllowedInitialValuesCount && (indexes.Length < 2 || indexes[0].Offset(1, 0).Equals(indexes[1]) || indexes[0].Offset(0, 1).Equals(indexes[1]) || indexes[1].Offset(1, 0).Equals(indexes[0]) || indexes[1].Offset(0, 1).Equals(indexes[0])))
			{
				return;
			}
			int num = indexes.Length;
			ICellValue[] array = new ICellValue[initialIndexesCount];
			for (int i = 0; i < initialIndexesCount; i++)
			{
				ICellValue value = worksheet.Cells[indexes[i]].GetValue().Value;
				array[i] = value;
			}
			double?[] array2 = this.ExtractNumberValues(array, num);
			NumberRelatedAutoFillGeneratorBase.Region[] array3 = this.ExtractRegions(array, array2);
			bool flag = false;
			foreach (NumberRelatedAutoFillGeneratorBase.Region region in array3)
			{
				if (this.IsRegionIncreasing(region) || this.IsRegionDecreasing(region))
				{
					this.ApplyGenerator(ref array2, region, initialIndexesCount);
					flag = true;
				}
			}
			ICellValue[] array5 = new ICellValue[num];
			if (flag)
			{
				for (int k = 0; k < array2.Length; k++)
				{
					if (array2[k] != null && array2[k].Value >= this.ValuesRangeLowerBound && array2[k].Value <= this.ValuesRangeUpperBound)
					{
						array5[k] = this.ValueToResultCellValue(array2[k].Value, array[k % initialIndexesCount]);
					}
				}
			}
			for (int l = 0; l < indexes.Length; l++)
			{
				if (array5[l] != null)
				{
					worksheet.Cells[indexes[l]].SetValueAndExpandToFitNumberValuesWidth(array5[l]);
				}
			}
		}

		double?[] ExtractNumberValues(ICellValue[] initialValues, int resultSeriesLength)
		{
			double?[] array = new double?[resultSeriesLength];
			int num = 0;
			for (int i = 0; i < initialValues.Length; i++)
			{
				double value;
				if (this.TryParse(initialValues[i], out value))
				{
					array[num] = new double?(value);
				}
				else
				{
					array[num] = null;
				}
				num++;
			}
			return array;
		}

		NumberRelatedAutoFillGeneratorBase.Region[] ExtractRegions(ICellValue[] initialValues, double?[] numberValues)
		{
			List<NumberRelatedAutoFillGeneratorBase.Region> list = new List<NumberRelatedAutoFillGeneratorBase.Region>();
			int num = initialValues.Length;
			bool flag = true;
			int num2 = 0;
			ICellValue previousCellValue = null;
			double? previousNumberValue = numberValues[0];
			do
			{
				if (this.IsValueFitInDesiredBounds(numberValues[num2]))
				{
					if (!this.HaveTheSameOrgin(previousCellValue, initialValues[num2], previousNumberValue, numberValues[num2]))
					{
						flag = true;
					}
					if (flag)
					{
						NumberRelatedAutoFillGeneratorBase.Region item = new NumberRelatedAutoFillGeneratorBase.Region(num2);
						list.Add(item);
						flag = false;
					}
					list[list.Count - 1].NumberItems.Add(numberValues[num2]);
				}
				else
				{
					flag = true;
				}
				previousCellValue = initialValues[num2];
				previousNumberValue = numberValues[num2];
				num2++;
			}
			while (num2 < num);
			List<NumberRelatedAutoFillGeneratorBase.Region> list2 = new List<NumberRelatedAutoFillGeneratorBase.Region>();
			foreach (NumberRelatedAutoFillGeneratorBase.Region region in list)
			{
				bool flag2 = false;
				foreach (double? num3 in region.NumberItems)
				{
					if (num3 != null)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					list2.Add(region);
				}
			}
			return list2.ToArray();
		}

		protected virtual bool IsValueFitInDesiredBounds(double? value)
		{
			double? num = value;
			double valuesRangeLowerBound = this.ValuesRangeLowerBound;
			if (num.GetValueOrDefault() >= valuesRangeLowerBound && num != null)
			{
				double? num2 = value;
				double valuesRangeUpperBound = this.ValuesRangeUpperBound;
				return num2.GetValueOrDefault() <= valuesRangeUpperBound && num2 != null;
			}
			return false;
		}

		bool CheckSequence(NumberRelatedAutoFillGeneratorBase.Region region, Func<double, double, bool> checker)
		{
			bool flag = true;
			if (region.NumberItems.Count > 1)
			{
				int num = 0;
				while (num < region.NumberItems.Count && region.NumberItems[num] == null)
				{
					num++;
				}
				int num2 = num + 1;
				while (num2 < region.NumberItems.Count && region.NumberItems[num2] == null)
				{
					num2++;
				}
				int index = num;
				int num3 = num2;
				while (num3 < region.NumberItems.Count && !region.NumberItems[num3].Equals(-1))
				{
					if (region.NumberItems[num3] != null)
					{
						bool flag2 = checker(region.NumberItems[index].Value, region.NumberItems[num3].Value);
						index = num3;
						if (flag != flag2)
						{
							return false;
						}
						flag = flag2;
					}
					num3++;
				}
			}
			return region.NumberItems.Count == 1 || flag;
		}

		bool IsRegionIncreasing(NumberRelatedAutoFillGeneratorBase.Region region)
		{
			return this.CheckSequence(region, (double velue1, double value2) => velue1 < value2);
		}

		bool IsRegionDecreasing(NumberRelatedAutoFillGeneratorBase.Region region)
		{
			return this.CheckSequence(region, (double velue1, double value2) => velue1 > value2);
		}

		double?[] RemoveNullsIfNeeded(double?[] initialValues)
		{
			int num = 0;
			double?[] array = this.TrimNullValues(initialValues);
			if (array.Length < 2)
			{
				return initialValues;
			}
			int num2 = 0;
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					num2 = i;
					break;
				}
				num++;
			}
			int num3 = 0;
			for (int j = num2 + 1; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					if (num != num3)
					{
						return initialValues;
					}
					num3 = 0;
				}
				else
				{
					num3++;
				}
			}
			return (from x in array
				where x != null
				select x).ToArray<double?>();
		}

		int CountLeadingNullValues(double?[] initialValuesList)
		{
			for (int i = 0; i < initialValuesList.Length; i++)
			{
				if (initialValuesList[i] != null)
				{
					return i;
				}
			}
			return initialValuesList.Length;
		}

		int CountTrailingNullValues(double?[] initialValuesList)
		{
			for (int i = initialValuesList.Length - 1; i >= 0; i--)
			{
				if (initialValuesList[i] != null)
				{
					return initialValuesList.Length - i - 1;
				}
			}
			return initialValuesList.Length;
		}

		double?[] TrimNullValues(double?[] initialValuesList)
		{
			int num = this.CountLeadingNullValues(initialValuesList);
			int num2 = this.CountTrailingNullValues(initialValuesList);
			IEnumerable<double?> source = initialValuesList.Skip(num);
			source = source.Take(initialValuesList.Length - num - num2);
			return source.ToArray<double?>();
		}

		void ApplyGenerator(ref double?[] resultValues, NumberRelatedAutoFillGeneratorBase.Region region, int step)
		{
			int num = resultValues.Length;
			int num2 = (num - region.StartIndex) / (region.EndIndex - region.StartIndex);
			double?[] initialValues = region.NumberItems.ToArray();
			double?[] array = this.RemoveNullsIfNeeded(initialValues);
			double num3 = ((region.NumberItems.Count == 1) ? 1.0 : FillDataSeriesHelper.GetStepValue(array));
			double?[] array2 = this.ApplyLinearFill(initialValues, array, num, num3);
			int num4 = 0;
			for (int i = 0; i < num2 + 1; i++)
			{
				for (int j = 0; j < region.NumberItems.Count; j++)
				{
					double? num5 = array2[num4];
					double? num6 = num5;
					double valuesRangeUpperBound = this.ValuesRangeUpperBound;
					if (num6.GetValueOrDefault() > valuesRangeUpperBound && num6 != null)
					{
						double? num7 = num5;
						double valuesRangeUpperBound2 = this.ValuesRangeUpperBound;
						double? num8 = ((num7 != null) ? new double?(num7.GetValueOrDefault() % valuesRangeUpperBound2) : null);
						if (num8.GetValueOrDefault() == 0.0 && num8 != null)
						{
							num5 = new double?(this.ValuesRangeUpperBound);
						}
						else
						{
							double? num9 = num5;
							double? num10 = num5;
							double valuesRangeUpperBound3 = this.ValuesRangeUpperBound;
							double num11 = (double)((int)((num10 != null) ? new double?(num10.GetValueOrDefault() / valuesRangeUpperBound3) : null).Value) * this.ValuesRangeUpperBound;
							num5 = ((num9 != null) ? new double?(num9.GetValueOrDefault() - num11) : null);
						}
					}
					else
					{
						double? num12 = num5;
						double valuesRangeLowerBound = this.ValuesRangeLowerBound;
						if (num12.GetValueOrDefault() < valuesRangeLowerBound && num12 != null)
						{
							if (!this.AllowReversingOfTheFitBelowTheLowerBoundValue)
							{
								double? num13 = num5;
								double valuesRangeUpperBound4 = this.ValuesRangeUpperBound;
								double? num14 = num5;
								double valuesRangeUpperBound5 = this.ValuesRangeUpperBound;
								double num15 = valuesRangeUpperBound4 - (double)((int)((num14 != null) ? new double?(num14.GetValueOrDefault() / valuesRangeUpperBound5) : null).Value) * this.ValuesRangeUpperBound;
								num5 = ((num13 != null) ? new double?(num13.GetValueOrDefault() + num15) : null);
							}
							else
							{
								double?[] array3 = this.ToNullableDoubleArray(FillDataSeriesHelper.FillLinear(Math.Abs(num5.Value), array2.Length - num4, Math.Abs(num3), null));
								Array.ConstrainedCopy(array3, 0, array2, num4, array3.Length);
								num5 = array2[num4];
							}
						}
					}
					int num16 = region.StartIndex + i * step + j;
					if (num16 >= num)
					{
						return;
					}
					resultValues[num16] = num5;
					num4++;
					if (num16 >= num - 1)
					{
						return;
					}
				}
			}
		}

		double?[] ApplyLinearFill(double?[] initialValues, double?[] numberValues, int resultSeriesLength, double linearStepValue)
		{
			bool flag = false;
			foreach (double num in initialValues)
			{
				if (num == null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int num2 = this.CountLeadingNullValues(initialValues);
				int num3 = this.CountTrailingNullValues(initialValues);
				int num4 = initialValues.Length - num2 - num3;
				List<double?> list = new List<double?>();
				double[] array = FillDataSeriesHelper.FillLinearTrend(initialValues, resultSeriesLength + 1);
				double num5 = array[0];
				double num6 = num5 * 100000.0 % 10.0;
				if (initialValues.Length == numberValues.Length || (initialValues.Length != numberValues.Length && num6 != 0.0))
				{
					double?[] source = this.ToNullableDoubleArray(array);
					list.AddRange(initialValues);
					IEnumerable<double?> source2 = ((num4 == 1) ? source.Skip(num4) : source.Skip(num4 + num2));
					do
					{
						list.AddRange(Enumerable.Repeat<double?>(null, num2));
						list.AddRange(source2.Take(num4));
						list.AddRange(Enumerable.Repeat<double?>(null, num3));
						source2 = source2.Skip(num4);
					}
					while (list.Count < resultSeriesLength);
					list = list.Take(resultSeriesLength + 1).ToList<double?>();
				}
				else
				{
					double?[] source = this.ToNullableDoubleArray(FillDataSeriesHelper.FillLinear((from x in initialValues
						where x != null
						select x).First<double?>().Value, resultSeriesLength + 1, linearStepValue, null));
					list.AddRange(initialValues);
					double?[] array2 = source.Skip(numberValues.Length).ToArray<double?>();
					int num7 = 0;
					for (int j = list.Count; j < resultSeriesLength; j++)
					{
						double? item = null;
						if (initialValues[j % initialValues.Length] != null)
						{
							item = array2[num7];
							num7++;
						}
						list.Add(item);
					}
					list = list.Take(resultSeriesLength + 1).ToList<double?>();
				}
				return list.ToArray();
			}
			return this.ToNullableDoubleArray(FillDataSeriesHelper.FillLinear(initialValues[0].Value, resultSeriesLength + 1, linearStepValue, null));
		}

		double?[] ToNullableDoubleArray(double[] sourceArray)
		{
			double?[] array = new double?[sourceArray.Length];
			for (int i = 0; i < sourceArray.Length; i++)
			{
				array[i] = new double?(sourceArray[i]);
			}
			return array;
		}

		protected abstract bool TryParse(ICellValue value, out double number);

		protected abstract ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue);

		protected virtual bool HaveTheSameOrgin(ICellValue previousCellValue, ICellValue currentCellValue, double? previousNumberValue, double? currentNumberValues)
		{
			return previousNumberValue != null && currentNumberValues != null;
		}

		class Region
		{
			public int StartIndex
			{
				get
				{
					return this.startIndex;
				}
			}

			public int EndIndex
			{
				get
				{
					return this.startIndex + this.numberItems.Count;
				}
			}

			public List<double?> NumberItems
			{
				get
				{
					if (this.numberItems == null)
					{
						this.numberItems = new List<double?>();
					}
					return this.numberItems;
				}
			}

			public Region(int startIndex)
			{
				this.startIndex = startIndex;
			}

			readonly int startIndex;

			List<double?> numberItems;
		}
	}
}
