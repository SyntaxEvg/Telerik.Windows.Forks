using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class ShortUpperCaseMonthsAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return ShortUpperCaseMonthsAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)ShortUpperCaseMonthsAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)ShortUpperCaseMonthsAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return ShortUpperCaseMonthsAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public ShortUpperCaseMonthsAutoFillGenerator()
		{
			this.monthsToInt = new Dictionary<string, int>();
			this.monthsToInt.Add("Jan", 1);
			this.monthsToInt.Add("Feb", 2);
			this.monthsToInt.Add("Mar", 3);
			this.monthsToInt.Add("Apr", 4);
			this.monthsToInt.Add("May", 5);
			this.monthsToInt.Add("Jun", 6);
			this.monthsToInt.Add("Jul", 7);
			this.monthsToInt.Add("Aug", 8);
			this.monthsToInt.Add("Sep", 9);
			this.monthsToInt.Add("Oct", 10);
			this.monthsToInt.Add("Nov", 11);
			this.monthsToInt.Add("Dec", 12);
			this.monthsToIntKeys = this.monthsToInt.Keys.ToArray<string>();
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
			if (this.monthsToInt.ContainsKey(resultValueAsString))
			{
				number = (double)this.monthsToInt[resultValueAsString];
				return true;
			}
			return false;
		}

		protected override ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue)
		{
			return new TextCellValue(this.monthsToIntKeys[(int)value - 1]);
		}

		static readonly string RuleName = "ShortUpperCaseMonths";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 12;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> monthsToInt;

		readonly string[] monthsToIntKeys;
	}
}
