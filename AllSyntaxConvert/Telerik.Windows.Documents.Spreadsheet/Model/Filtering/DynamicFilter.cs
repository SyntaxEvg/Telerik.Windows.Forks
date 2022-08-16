using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class DynamicFilter : CellValuesFilterBase, IWorksheetFilter, IRangeFilter
	{
		public DynamicFilterType DynamicFilterType
		{
			get
			{
				return this.filterType;
			}
		}

		public DynamicFilter(int relativeColumnIndex, DynamicFilterType dynamicFilterType)
			: base(relativeColumnIndex)
		{
			this.filterType = dynamicFilterType;
			this.InitializeShowForDatePredicates();
			this.InitializeShowForNumberPredicates();
		}

		internal override IFilter Copy(int newRelativeColumnIndex)
		{
			return new DynamicFilter(newRelativeColumnIndex, this.DynamicFilterType);
		}

		void IWorksheetFilter.SetWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
		}

		void IRangeFilter.SetColumnRange(CellRange columnRange)
		{
			this.SetColumnRange(columnRange);
		}

		void SetColumnRange(CellRange columnRange)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(this.worksheet, "worksheet");
			if (columnRange != null)
			{
				Guard.ThrowExceptionIfNotEqual<int>(1, columnRange.ColumnCount, "range");
			}
			if (this.filterType == DynamicFilterType.AboveAverage || this.filterType == DynamicFilterType.BelowAverage)
			{
				this.average = new double?(this.CalculateAverage(this.worksheet, columnRange));
			}
		}

		double CalculateAverage(Worksheet worksheet, CellRange range)
		{
			if (range == null)
			{
				return 0.0;
			}
			double num = 0.0;
			int num2 = 0;
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(i, range.FromIndex.ColumnIndex);
				ICellValue propertyValue = worksheet.Cells.PropertyBag.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, index);
				NumberCellValue numberCellValue = propertyValue as NumberCellValue;
				if (numberCellValue != null)
				{
					num += numberCellValue.Value;
					num2++;
				}
			}
			if (num2 != 0)
			{
				return num / (double)num2;
			}
			throw new InvalidOperationException();
		}

		public override object GetValue(Cells cells, int rowIndex, int columnIndex)
		{
			ICellValue cellValue = base.GetValue(cells, rowIndex, columnIndex) as ICellValue;
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (formulaCellValue != null)
			{
				cellValue = formulaCellValue.GetResultValueAsCellValue();
			}
			NumberCellValue numberCellValue = cellValue as NumberCellValue;
			if (numberCellValue == null)
			{
				return null;
			}
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			CellValueFormat propertyValue = cells.PropertyBag.GetPropertyValue<CellValueFormat>(CellPropertyDefinitions.FormatProperty, index);
			if (propertyValue.IsDateFormat())
			{
				DateTime? dateTime = numberCellValue.ToDateTime();
				if (dateTime != null && numberCellValue.Value >= 1.0)
				{
					return dateTime.Value;
				}
			}
			return numberCellValue.Value;
		}

		public override bool ShouldShowValue(object value)
		{
			if (value is DateTime)
			{
				return this.ShouldShowValueDate((DateTime)value);
			}
			return value is double && this.ShouldShowValueNumber((double)value);
		}

		bool ShouldShowValueNumber(double number)
		{
			if (!this.shouldShowForNumber.ContainsKey(this.filterType))
			{
				throw new FilteringException("Invalid dynamic filter type", new InvalidOperationException("Invalid dynamic filter type"), "Spreadsheet_Filtering_InvalidDynamicFilter");
			}
			return this.shouldShowForNumber[this.filterType](number);
		}

		bool ShouldShowValueDate(DateTime date)
		{
			if (!this.shouldShowForDate.ContainsKey(this.filterType))
			{
				throw new FilteringException("Invalid dynamic filter type", new InvalidOperationException("Invalid dynamic filter type"), "Spreadsheet_Filtering_InvalidDynamicFilter");
			}
			return this.shouldShowForDate[this.filterType](date);
		}

		void InitializeShowForNumberPredicates()
		{
			this.shouldShowForNumber = new Dictionary<DynamicFilterType, Func<double, bool>>();
			this.shouldShowForNumber.Add(DynamicFilterType.BelowAverage, delegate(double number)
			{
				if (this.average == null)
				{
					throw new FilteringException("This filter is not yet assigned to a column.", new InvalidOperationException("This filter is not yet assigned to a column."), "Spreadsheet_Filtering_RangeFilterNotAssigned");
				}
				double? num = this.average;
				return number < num.GetValueOrDefault() && num != null;
			});
			this.shouldShowForNumber.Add(DynamicFilterType.AboveAverage, delegate(double number)
			{
				if (this.average == null)
				{
					throw new FilteringException("This filter is not yet assigned to a column.", new InvalidOperationException("This filter is not yet assigned to a column."), "Spreadsheet_Filtering_RangeFilterNotAssigned");
				}
				double? num = this.average;
				return number > num.GetValueOrDefault() && num != null;
			});
			this.shouldShowForNumber.Add(DynamicFilterType.None, delegate(double number)
			{
				throw new FilteringException("Invalid dynamic filter type", new InvalidOperationException("Invalid dynamic filter type"), "Spreadsheet_Filtering_InvalidDynamicFilter");
			});
			foreach (object obj in Enum.GetValues(typeof(DynamicFilterType)))
			{
				DynamicFilterType key = (DynamicFilterType)obj;
				if (!this.shouldShowForNumber.ContainsKey(key))
				{
					this.shouldShowForNumber.Add(key, (double number) => false);
				}
			}
		}

		void InitializeShowForDatePredicates()
		{
			this.shouldShowForDate = new Dictionary<DynamicFilterType, Func<DateTime, bool>>();
			this.shouldShowForDate.Add(DynamicFilterType.BelowAverage, delegate(DateTime date)
			{
				double num = FormatHelper.ConvertDateTimeToDouble(date);
				double? num2 = this.average;
				return num < num2.GetValueOrDefault() && num2 != null;
			});
			this.shouldShowForDate.Add(DynamicFilterType.AboveAverage, delegate(DateTime date)
			{
				double num = FormatHelper.ConvertDateTimeToDouble(date);
				double? num2 = this.average;
				return num > num2.GetValueOrDefault() && num2 != null;
			});
			this.shouldShowForDate.Add(DynamicFilterType.Tomorrow, (DateTime date) => date.AddDays(1.0) == DateTime.Today);
			this.shouldShowForDate.Add(DynamicFilterType.Today, (DateTime date) => date == DateTime.Today);
			this.shouldShowForDate.Add(DynamicFilterType.Yesterday, (DateTime date) => date.AddDays(1.0) == DateTime.Today);
			this.shouldShowForDate.Add(DynamicFilterType.NextWeek, (DateTime date) => date.GetBeginningOfWeek().AddDays(-7.0) == DateTime.Today.GetBeginningOfWeek());
			this.shouldShowForDate.Add(DynamicFilterType.ThisWeek, (DateTime date) => date.GetBeginningOfWeek() == DateTime.Today.GetBeginningOfWeek());
			this.shouldShowForDate.Add(DynamicFilterType.LastWeek, (DateTime date) => date.GetBeginningOfWeek().AddDays(7.0) == DateTime.Today.GetBeginningOfWeek());
			this.shouldShowForDate.Add(DynamicFilterType.NextMonth, (DateTime date) => date.AddMonths(-1).Month == DateTime.Today.Month);
			this.shouldShowForDate.Add(DynamicFilterType.ThisMonth, (DateTime date) => date.Month == DateTime.Today.Month);
			this.shouldShowForDate.Add(DynamicFilterType.LastMonth, (DateTime date) => date.AddMonths(1).Month == DateTime.Today.Month);
			this.shouldShowForDate.Add(DynamicFilterType.NextQuarter, (DateTime date) => date.DateIsNextQuarter());
			this.shouldShowForDate.Add(DynamicFilterType.ThisQuarter, (DateTime date) => date.Year == DateTime.Today.Year && date.GetQuarter() == DateTime.Today.GetQuarter());
			this.shouldShowForDate.Add(DynamicFilterType.LastQuarter, (DateTime date) => date.DateIsLastQuarter());
			this.shouldShowForDate.Add(DynamicFilterType.NextYear, (DateTime date) => date.Year - 1 == DateTime.Today.Year);
			this.shouldShowForDate.Add(DynamicFilterType.ThisYear, (DateTime date) => date.Year == DateTime.Today.Year);
			this.shouldShowForDate.Add(DynamicFilterType.LastYear, (DateTime date) => date.Year + 1 == DateTime.Today.Year);
			this.shouldShowForDate.Add(DynamicFilterType.YearToDate, (DateTime date) => date.Year == DateTime.Today.Year && date <= DateTime.Today);
			this.shouldShowForDate.Add(DynamicFilterType.Quarter1, (DateTime date) => date.GetQuarter() == 1);
			this.shouldShowForDate.Add(DynamicFilterType.Quarter2, (DateTime date) => date.GetQuarter() == 2);
			this.shouldShowForDate.Add(DynamicFilterType.Quarter3, (DateTime date) => date.GetQuarter() == 3);
			this.shouldShowForDate.Add(DynamicFilterType.Quarter4, (DateTime date) => date.GetQuarter() == 4);
			this.shouldShowForDate.Add(DynamicFilterType.January, (DateTime date) => date.Month == 1);
			this.shouldShowForDate.Add(DynamicFilterType.February, (DateTime date) => date.Month == 2);
			this.shouldShowForDate.Add(DynamicFilterType.March, (DateTime date) => date.Month == 3);
			this.shouldShowForDate.Add(DynamicFilterType.April, (DateTime date) => date.Month == 4);
			this.shouldShowForDate.Add(DynamicFilterType.May, (DateTime date) => date.Month == 5);
			this.shouldShowForDate.Add(DynamicFilterType.June, (DateTime date) => date.Month == 6);
			this.shouldShowForDate.Add(DynamicFilterType.July, (DateTime date) => date.Month == 7);
			this.shouldShowForDate.Add(DynamicFilterType.August, (DateTime date) => date.Month == 8);
			this.shouldShowForDate.Add(DynamicFilterType.September, (DateTime date) => date.Month == 9);
			this.shouldShowForDate.Add(DynamicFilterType.October, (DateTime date) => date.Month == 10);
			this.shouldShowForDate.Add(DynamicFilterType.November, (DateTime date) => date.Month == 11);
			this.shouldShowForDate.Add(DynamicFilterType.December, (DateTime date) => date.Month == 12);
		}

		public override bool Equals(object obj)
		{
			DynamicFilter dynamicFilter = obj as DynamicFilter;
			return dynamicFilter != null && this.DynamicFilterType.Equals(dynamicFilter.DynamicFilterType);
		}

		public override int GetHashCode()
		{
			return this.DynamicFilterType.GetHashCode();
		}

		readonly DynamicFilterType filterType;

		double? average = null;

		Dictionary<DynamicFilterType, Func<DateTime, bool>> shouldShowForDate;

		Dictionary<DynamicFilterType, Func<double, bool>> shouldShowForNumber;

		Worksheet worksheet;
	}
}
