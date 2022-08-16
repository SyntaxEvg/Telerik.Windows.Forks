using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class LongUpperCaseMonthsAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return LongUpperCaseMonthsAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)LongUpperCaseMonthsAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)LongUpperCaseMonthsAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return LongUpperCaseMonthsAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public LongUpperCaseMonthsAutoFillGenerator()
		{
			this.monthsToInt = new Dictionary<string, int>();
			this.monthsToInt.Add("January", 1);
			this.monthsToInt.Add("February", 2);
			this.monthsToInt.Add("March", 3);
			this.monthsToInt.Add("April", 4);
			this.monthsToInt.Add("May", 5);
			this.monthsToInt.Add("June", 6);
			this.monthsToInt.Add("July", 7);
			this.monthsToInt.Add("August", 8);
			this.monthsToInt.Add("September", 9);
			this.monthsToInt.Add("October", 10);
			this.monthsToInt.Add("November", 11);
			this.monthsToInt.Add("December", 12);
			this.monthsToIntKeys = this.monthsToInt.Keys.ToArray<string>();
		}

		protected override bool TryParse(ICellValue value, out double number)
		{
			number = 1.0;
			if (value.ValueType != CellValueType.Text)
			{
				return false;
			}
			string resultValueAsString = value.GetResultValueAsString(CellValueFormat.GeneralFormat);
			number = 1.0;
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

		static readonly string RuleName = "LongUpperCaseMonths";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 12;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> monthsToInt;

		readonly string[] monthsToIntKeys;
	}
}
