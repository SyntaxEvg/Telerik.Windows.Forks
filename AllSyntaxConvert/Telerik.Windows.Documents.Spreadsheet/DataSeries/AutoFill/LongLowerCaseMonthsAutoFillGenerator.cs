using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class LongLowerCaseMonthsAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return LongLowerCaseMonthsAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)LongLowerCaseMonthsAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)LongLowerCaseMonthsAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return LongLowerCaseMonthsAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public LongLowerCaseMonthsAutoFillGenerator()
		{
			this.monthsToInt = new Dictionary<string, int>();
			this.monthsToInt.Add("january", 1);
			this.monthsToInt.Add("february", 2);
			this.monthsToInt.Add("march", 3);
			this.monthsToInt.Add("april", 4);
			this.monthsToInt.Add("may", 5);
			this.monthsToInt.Add("june", 6);
			this.monthsToInt.Add("july", 7);
			this.monthsToInt.Add("august", 8);
			this.monthsToInt.Add("september", 9);
			this.monthsToInt.Add("october", 10);
			this.monthsToInt.Add("november", 11);
			this.monthsToInt.Add("december", 12);
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

		static readonly string RuleName = "LongLowerCaseMonths";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 12;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> monthsToInt;

		readonly string[] monthsToIntKeys;
	}
}
