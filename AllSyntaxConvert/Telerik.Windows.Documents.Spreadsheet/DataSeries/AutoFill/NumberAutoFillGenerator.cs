using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.DataSeries.AutoFill
{
	class NumberAutoFillGenerator : NumberRelatedAutoFillGeneratorBase
	{
		public override string Name
		{
			get
			{
				return NumberAutoFillGenerator.RuleName;
			}
		}

		protected override double ValuesRangeLowerBound
		{
			get
			{
				return NumberAutoFillGenerator.RangeLowerBound;
			}
		}

		protected override double ValuesRangeUpperBound
		{
			get
			{
				return NumberAutoFillGenerator.RangeUpperBound;
			}
		}

		protected override bool AllowReversingOfTheFitBelowTheLowerBoundValue
		{
			get
			{
				return NumberAutoFillGenerator.AllowReversingBelowLowerBound;
			}
		}

		protected override int MinAllowedInitialValuesCount
		{
			get
			{
				return NumberAutoFillGenerator.MinAllowedInitialValues;
			}
		}

		protected override bool IsValueFitInDesiredBounds(double? value)
		{
			return base.IsValueFitInDesiredBounds(value) || value == null;
		}

		protected override bool TryParse(ICellValue value, out double number)
		{
			number = 1.0;
			if (value.ValueType == CellValueType.Boolean || value.ValueType == CellValueType.Formula || value.ValueType == CellValueType.Empty)
			{
				return false;
			}
			string resultValueAsString = value.GetResultValueAsString(CellValueFormat.GeneralFormat);
			return double.TryParse(resultValueAsString, out number);
		}

		protected override ICellValue ValueToResultCellValue(double value, ICellValue firstInitialValue)
		{
			return value.ToCellValue();
		}

		protected override bool HaveTheSameOrgin(ICellValue previousCellValue, ICellValue currentCellValue, double? previousNumberValue, double? currentNumberValues)
		{
			return true;
		}

		static readonly string RuleName = "Number";

		static readonly double RangeLowerBound = double.MinValue;

		static readonly double RangeUpperBound = double.MaxValue;

		static readonly bool AllowReversingBelowLowerBound = false;

		static readonly int MinAllowedInitialValues = 2;
	}
}
