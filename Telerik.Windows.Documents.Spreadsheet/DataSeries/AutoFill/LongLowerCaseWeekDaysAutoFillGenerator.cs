using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class LongLowerCaseWeekDaysAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return LongLowerCaseWeekDaysAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)LongLowerCaseWeekDaysAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)LongLowerCaseWeekDaysAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return LongLowerCaseWeekDaysAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		public LongLowerCaseWeekDaysAutoFillGenerator()
		{
			this.weekDayToInt = new Dictionary<string, int>();
			this.weekDayToInt.Add("monday", 1);
			this.weekDayToInt.Add("tuesday", 2);
			this.weekDayToInt.Add("wednesday", 3);
			this.weekDayToInt.Add("thursday", 4);
			this.weekDayToInt.Add("friday", 5);
			this.weekDayToInt.Add("saturday", 6);
			this.weekDayToInt.Add("sunday", 7);
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

		static readonly string RuleName = "LongLowerCaseWeekDays";

		static readonly int RangeLowerBound = 1;

		static readonly int RangeUpperBound = 7;

		static readonly bool AllowReversingBelowLowerBound = false;

		readonly Dictionary<string, int> weekDayToInt;

		readonly string[] weekDayToIntKeys;
	}
}
