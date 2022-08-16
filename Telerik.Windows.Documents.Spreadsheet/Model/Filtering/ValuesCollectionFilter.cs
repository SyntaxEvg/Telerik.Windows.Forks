using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class ValuesCollectionFilter : CellValuesFilterBase
	{
		public IEnumerable<string> StringValues
		{
			get
			{
				return this.stringValues;
			}
		}

		public IEnumerable<DateGroupItem> DateItems
		{
			get
			{
				return this.dateValues;
			}
		}

		public bool Blank
		{
			get
			{
				return this.blank;
			}
		}

		public ValuesCollectionFilter(int relativeColumnIndex, IEnumerable<string> stringValues)
			: this(relativeColumnIndex, stringValues, null, false)
		{
		}

		public ValuesCollectionFilter(int relativeColumnIndex, IEnumerable<DateGroupItem> dateValues)
			: this(relativeColumnIndex, null, dateValues, false)
		{
		}

		public ValuesCollectionFilter(int relativeColumnIndex, IEnumerable<string> stringValues, bool blank)
			: this(relativeColumnIndex, stringValues, null, blank)
		{
		}

		public ValuesCollectionFilter(int relativeColumnIndex, IEnumerable<DateGroupItem> dateValues, bool blank)
			: this(relativeColumnIndex, null, dateValues, blank)
		{
		}

		public ValuesCollectionFilter(int relativeColumnIndex, IEnumerable<string> stringValues, IEnumerable<DateGroupItem> dateValues)
			: this(relativeColumnIndex, stringValues, dateValues, false)
		{
		}

		public ValuesCollectionFilter(int relativeColumnIndex, IEnumerable<string> stringValues, IEnumerable<DateGroupItem> dateValues, bool blank)
			: base(relativeColumnIndex)
		{
			this.dateValues = ((dateValues == null) ? new DateGroupItem[0] : dateValues.ToArray<DateGroupItem>());
			this.stringValues = ((stringValues == null) ? new string[0] : stringValues.ToArray<string>());
			this.blank = blank;
		}

		internal override IFilter Copy(int newRelativeColumnIndex)
		{
			return new ValuesCollectionFilter(newRelativeColumnIndex, this.StringValues, this.DateItems, this.Blank);
		}

		public override object GetValue(Cells cells, int rowIndex, int columnIndex)
		{
			ICellValue cellValue = base.GetValue(cells, rowIndex, columnIndex) as ICellValue;
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			CellValueFormat propertyValueRespectingStyle = cells.PropertyBag.GetPropertyValueRespectingStyle<CellValueFormat>(CellPropertyDefinitions.FormatProperty, cells.Worksheet, index);
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (formulaCellValue != null)
			{
				cellValue = formulaCellValue.GetResultValueAsCellValue();
			}
			NumberCellValue numberCellValue = cellValue as NumberCellValue;
			if (propertyValueRespectingStyle.IsDateFormat() && numberCellValue != null && numberCellValue.Value >= 1.0)
			{
				DateTime? dateTime = numberCellValue.ToDateTime();
				if (dateTime != null)
				{
					return dateTime.Value;
				}
			}
			return cellValue.GetResultValueAsString(propertyValueRespectingStyle);
		}

		public override bool ShouldShowValue(object value)
		{
			string text = value as string;
			if (text != null)
			{
				return this.ShouldShowValue(text);
			}
			DateTime? dateTime = value as DateTime?;
			return dateTime != null && this.ShouldShowValue(dateTime.Value);
		}

		bool ShouldShowValue(string value)
		{
			return this.stringValues.Any((string x) => string.Equals(x, value, StringComparison.OrdinalIgnoreCase)) || (string.IsNullOrWhiteSpace(value) && this.blank);
		}

		bool ShouldShowValue(DateTime value)
		{
			return this.dateValues.Any((DateGroupItem x) => x.DateSatisfiesItem(value));
		}

		public override bool Equals(object obj)
		{
			ValuesCollectionFilter valuesCollectionFilter = obj as ValuesCollectionFilter;
			return valuesCollectionFilter != null && (this.Blank.Equals(valuesCollectionFilter.Blank) && this.StringValues.SequenceEqual(valuesCollectionFilter.StringValues)) && this.DateItems.SequenceEqual(valuesCollectionFilter.DateItems);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Blank.GetHashCode(), this.StringValues.GetHashCodeOrZero(), this.DateItems.GetHashCodeOrZero());
		}

		readonly string[] stringValues;

		readonly DateGroupItem[] dateValues;

		readonly bool blank;
	}
}
