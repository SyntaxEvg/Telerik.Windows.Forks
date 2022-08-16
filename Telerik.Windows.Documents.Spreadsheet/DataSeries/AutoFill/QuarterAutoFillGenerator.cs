using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class QuarterAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return QuarterAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)QuarterAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)QuarterAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return QuarterAutoFillGenerator.AllowReversingBelowLowerBound;
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
			number = -1.0;
			if (resultValueAsString[0] == 'q' || resultValueAsString[0] == 'Q')
			{
				string s = resultValueAsString.Substring(1);
				if (double.TryParse(s, out number) && number >= (double)QuarterAutoFillGenerator.RangeLowerBound && number <= (double)QuarterAutoFillGenerator.RangeUpperBound)
				{
					return true;
				}
			}
			return false;
		}

		protected override ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue)
		{
			string resultValueAsString = firstInitialValue.GetResultValueAsString(CellValueFormat.GeneralFormat);
			char c = resultValueAsString[0];
			return new TextCellValue(c + value.ToString());
		}

		static readonly string RuleName = "Quarter";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 4;

		static readonly bool AllowReversingBelowLowerBound = false;
	}
}
