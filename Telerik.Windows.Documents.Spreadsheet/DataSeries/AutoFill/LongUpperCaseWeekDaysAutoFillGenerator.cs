using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class LongUpperCaseWeekDaysAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return LongUpperCaseWeekDaysAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)LongUpperCaseWeekDaysAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)LongUpperCaseWeekDaysAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return LongUpperCaseWeekDaysAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public LongUpperCaseWeekDaysAutoFillGenerator()
		{
			this.weekDayToInt = new Dictionary<string, int>();
			this.weekDayToInt.Add("Monday", 1);
			this.weekDayToInt.Add("Tuesday", 2);
			this.weekDayToInt.Add("Wednesday", 3);
			this.weekDayToInt.Add("Thursday", 4);
			this.weekDayToInt.Add("Friday", 5);
			this.weekDayToInt.Add("Saturday", 6);
			this.weekDayToInt.Add("Sunday", 7);
			this.weekDayToIntKeys = this.weekDayToInt.Keys.ToArray<string>();
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

		static readonly string RuleName = "LongUpperCaseWeekDays";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 7;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> weekDayToInt;

		readonly string[] weekDayToIntKeys;
	}
}
