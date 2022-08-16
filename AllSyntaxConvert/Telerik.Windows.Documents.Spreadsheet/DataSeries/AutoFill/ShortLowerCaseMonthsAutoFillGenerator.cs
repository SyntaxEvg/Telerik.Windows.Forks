using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class ShortLowerCaseMonthsAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return ShortLowerCaseMonthsAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)ShortLowerCaseMonthsAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)ShortLowerCaseMonthsAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return ShortLowerCaseMonthsAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public ShortLowerCaseMonthsAutoFillGenerator()
		{
			this.monthsToInt = new Dictionary<string, int>();
			this.monthsToInt.Add("jan", 1);
			this.monthsToInt.Add("feb", 2);
			this.monthsToInt.Add("mar", 3);
			this.monthsToInt.Add("apr", 4);
			this.monthsToInt.Add("may", 5);
			this.monthsToInt.Add("jun", 6);
			this.monthsToInt.Add("jul", 7);
			this.monthsToInt.Add("aug", 8);
			this.monthsToInt.Add("sep", 9);
			this.monthsToInt.Add("oct", 10);
			this.monthsToInt.Add("nov", 11);
			this.monthsToInt.Add("dec", 12);
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

		static readonly string RuleName = "ShortLowerCaseMonths";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 12;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> monthsToInt;

		readonly string[] monthsToIntKeys;
	}
}
