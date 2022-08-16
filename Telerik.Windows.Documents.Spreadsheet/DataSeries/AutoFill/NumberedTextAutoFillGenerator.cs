using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class NumberedTextAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return NumberedTextAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)NumberedTextAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)NumberedTextAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return NumberedTextAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		protected override bool TryParse(ICellValue value, out double number)
		{
			number = -1.0;
			if (value.ValueType != CellValueType.Text)
			{
				return false;
			}
			string resultValueAsString = value.GetResultValueAsString(CellValueFormat.GeneralFormat);
			if (resultValueAsString == null)
			{
				number = -1.0;
				return false;
			}
			int i = resultValueAsString.Length;
			StringBuilder stringBuilder = new StringBuilder();
			while (i > 0)
			{
				i--;
				if (!char.IsDigit(resultValueAsString[i]))
				{
					i++;
					break;
				}
				char c = resultValueAsString[i];
				stringBuilder.Insert(0, new char[] { c });
			}
			number = -1.0;
			int num;
			if (int.TryParse(stringBuilder.ToString(), out num))
			{
				number = (double)num;
				return true;
			}
			return false;
		}

		protected override ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue)
		{
			if (firstInitialValue.ValueType != CellValueType.Text)
			{
				return null;
			}
			string resultValueAsString = firstInitialValue.GetResultValueAsString(CellValueFormat.GeneralFormat);
			double num;
			if (this.TryParse(firstInitialValue, out num))
			{
				int length = resultValueAsString.LastIndexOf(((int)num).ToString());
				string str = resultValueAsString.Substring(0, length);
				return new TextCellValue(str + value.ToString());
			}
			return null;
		}

		protected override bool HaveTheSameOrgin(ICellValue previousCellValue, ICellValue currentCellValue, double? previousNumberValue, double? currentNumberValues)
		{
			if (previousNumberValue == null || currentNumberValues == null)
			{
				return false;
			}
			if (previousCellValue != null && currentCellValue != null)
			{
				double? num = previousNumberValue;
				double valuesRangeUpperBound = this.ValuesRangeUpperBound;
				if (num.GetValueOrDefault() <= valuesRangeUpperBound || num == null)
				{
					double? num2 = previousNumberValue;
					double valuesRangeLowerBound = this.ValuesRangeLowerBound;
					if (num2.GetValueOrDefault() >= valuesRangeLowerBound || num2 == null)
					{
						double? num3 = currentNumberValues;
						double valuesRangeUpperBound2 = this.ValuesRangeUpperBound;
						if (num3.GetValueOrDefault() <= valuesRangeUpperBound2 || num3 == null)
						{
							double? num4 = currentNumberValues;
							double valuesRangeLowerBound2 = this.ValuesRangeLowerBound;
							if (num4.GetValueOrDefault() >= valuesRangeLowerBound2 || num4 == null)
							{
								string text = ((previousCellValue.ValueType == CellValueType.Text) ? previousCellValue.GetResultValueAsString(CellValueFormat.GeneralFormat) : null);
								string text2 = ((currentCellValue.ValueType == CellValueType.Text) ? currentCellValue.GetResultValueAsString(CellValueFormat.GeneralFormat) : null);
								string b = text.Remove(text.LastIndexOf(previousNumberValue.ToString()));
								string a = text2.Remove(text2.LastIndexOf(currentNumberValues.ToString()));
								return a == b;
							}
						}
					}
				}
			}
			return true;
		}

		static readonly string RuleName = "NumberedText";

		static readonly int RangeLowerBound = 0;

		static readonly int RangeUpperBound = int.MaxValue;

		static readonly bool AllowReversingBelowLowerBound = true;
	}
}
