using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class ShortLowerCaseWeekDaysAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return ShortLowerCaseWeekDaysAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)ShortLowerCaseWeekDaysAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)ShortLowerCaseWeekDaysAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return ShortLowerCaseWeekDaysAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public ShortLowerCaseWeekDaysAutoFillGenerator()
		{
			this.weekDayToInt = new Dictionary<string, int>();
			this.weekDayToInt.Add("mon", 1);
			this.weekDayToInt.Add("tue", 2);
			this.weekDayToInt.Add("wed", 3);
			this.weekDayToInt.Add("thu", 4);
			this.weekDayToInt.Add("fri", 5);
			this.weekDayToInt.Add("sat", 6);
			this.weekDayToInt.Add("sun", 7);
			this.weekDayToIntKeys = this.weekDayToInt.Keys.ToArray<string>();
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
			if (this.weekDayToInt.ContainsKey(resultValueAsString))
			{
				number = (double)this.weekDayToInt[resultValueAsString];
				return true;
			}
			return false;
		}

		protected override ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue)
		{
			return new TextCellValue(this.weekDayToIntKeys[(int)value - 1]);
		}

		static readonly string RuleName = "ShortLowerCaseWeekDays";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 7;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> weekDayToInt;

		readonly string[] weekDayToIntKeys;
	}
}
