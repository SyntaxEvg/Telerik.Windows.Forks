using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class OrdinalNumbersFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return OrdinalNumbersFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return (double)OrdinalNumbersFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return (double)OrdinalNumbersFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return OrdinalNumbersFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		protected override bool TryParse(ICellValue value, out double number)
		{
			number = 1.0;
			if (value.ValueType != CellValueType.Text)
			{
				return false;
			}
			string text = value.GetResultValueAsString(CellValueFormat.GeneralFormat);
			number = 1.0;
			if (text.Length < 3 || !OrdinalNumbersFillGenerator.ordinalSuffixes.Any((string sf) => text.EndsWith(sf)))
			{
				return false;
			}
			string s = text.Substring(0, text.Length - 2);
			return double.TryParse(s, out number);
		}

		protected override ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue)
		{
			string value2;
			switch ((int)value % 10)
			{
			case 1:
				value2 = value + "st";
				break;
			case 2:
				value2 = value + "nd";
				break;
			case 3:
				value2 = value + "rd";
				break;
			default:
				value2 = value + "th";
				break;
			}
			switch ((int)value)
			{
			case 11:
			case 12:
			case 13:
				value2 = value + "th";
				break;
			}
			return new TextCellValue(value2);
		}

		const string stSuffix = "st";

		const string ndSuffix = "nd";

		const string rdSuffix = "rd";

		const string thSuffix = "th";

		static readonly string RuleName = "OrdinalNumbers";

		static readonly int RangeLowerBound = 0;

		static readonly int RangeUpperBound = int.MaxValue;

		static readonly bool AllowReversingBelowLowerBound = true;

		static readonly List<string> ordinalSuffixes = new List<string> { "st", "nd", "rd", "th" };
	}
}
