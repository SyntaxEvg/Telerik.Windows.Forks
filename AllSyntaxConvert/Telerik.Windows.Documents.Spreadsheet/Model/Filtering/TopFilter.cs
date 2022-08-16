using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class TopFilter : CellValuesFilterBase, IWorksheetFilter, IRangeFilter
	{
		public TopFilterType TopFilterType
		{
			get
			{
				return this.topFilterType;
			}
		}

		public double Value
		{
			get
			{
				return this.value;
			}
		}

		public TopFilter(int relativeColumnIndex)
			: this(relativeColumnIndex, TopFilterType.TopNumber, 10.0)
		{
		}

		public TopFilter(int relativeColumnIndex, TopFilterType topFilterType, double value)
			: base(relativeColumnIndex)
		{
			this.topFilterType = topFilterType;
			this.value = value;
		}

		internal override IFilter Copy(int newRelativeColumnIndex)
		{
			return new TopFilter(newRelativeColumnIndex, this.TopFilterType, this.Value);
		}

		public override bool ShouldShowValue(object value)
		{
			if (this.valuesToShow == null)
			{
				throw new FilteringException("This filter is not yet assigned to a column.", new InvalidOperationException("This filter is not yet assigned to a column."), "Spreadsheet_Filtering_RangeFilterNotAssigned");
			}
			FormulaCellValue formulaCellValue = value as FormulaCellValue;
			if (formulaCellValue != null)
			{
				value = formulaCellValue.GetResultValueAsCellValue();
			}
			NumberCellValue numberCellValue = value as NumberCellValue;
			return numberCellValue != null && this.valuesToShow.Contains(numberCellValue.Value);
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
			this.valuesToShow = this.GetValuesToBeShownArray(this.worksheet, columnRange, this.topFilterType, this.value);
		}

		HashSet<double> GetValuesToBeShownArray(Worksheet worksheet, CellRange range, TopFilterType topFilterType, double value)
		{
			if (range == null)
			{
				return new HashSet<double>();
			}
			List<double> list = new List<double>();
			for (int i = range.FromIndex.RowIndex; i <= range.ToIndex.RowIndex; i++)
			{
				long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(i, range.FromIndex.ColumnIndex);
				ICellValue cellValue = worksheet.Cells.PropertyBag.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, index);
				FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
				if (formulaCellValue != null)
				{
					cellValue = formulaCellValue.GetResultValueAsCellValue();
				}
				NumberCellValue numberCellValue = cellValue as NumberCellValue;
				if (numberCellValue != null)
				{
					list.Add(numberCellValue.Value);
				}
			}
			list.Sort();
			if (topFilterType == TopFilterType.TopNumber || topFilterType == TopFilterType.TopPercent)
			{
				list.Reverse();
			}
			int count;
			if (topFilterType == TopFilterType.TopPercent || topFilterType == TopFilterType.BottomPercent)
			{
				count = (int)((double)list.Count * value / 100.0);
			}
			else
			{
				count = (int)value;
			}
			return new HashSet<double>(list.Take(count));
		}

		public override bool Equals(object obj)
		{
			TopFilter topFilter = obj as TopFilter;
			return topFilter != null && this.TopFilterType.Equals(topFilter.TopFilterType) && this.Value.Equals(topFilter.Value);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.TopFilterType.GetHashCode(), this.Value.GetHashCode());
		}

		HashSet<double> valuesToShow;

		readonly TopFilterType topFilterType;

		readonly double value;

		Worksheet worksheet;
	}
}
