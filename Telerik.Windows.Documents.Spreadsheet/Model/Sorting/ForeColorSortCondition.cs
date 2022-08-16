using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting.Conditions;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public sealed class ForeColorSortCondition : OrderedSortConditionBase<ThemableColor>, IWorksheetSortCondition
	{
		public ThemableColor Color
		{
			get
			{
				return this.color;
			}
		}

		protected override IPropertyDefinition<ThemableColor> PropertyDefinition
		{
			get
			{
				return CellPropertyDefinitions.ForeColorProperty;
			}
		}

		public override IComparer<SortValue> Comparer
		{
			get
			{
				if (this.comparer == null)
				{
					this.comparer = new ForeColorComparer(this.actualColor, this.sortOrder);
				}
				return this.comparer;
			}
		}

		public ForeColorSortCondition(int relativeIndex, ThemableColor color, SortOrder sortOrder)
			: base(relativeIndex, sortOrder)
		{
			this.color = color;
			this.sortOrder = sortOrder;
		}

		void IWorksheetSortCondition.SetWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.actualColor = SortAndFilterHelper.GetThemableColorWithLocalColors(this.color, this.worksheet.Workbook.Theme.ColorScheme);
		}

		public override object GetValue(Cells cells, int rowIndex, int columnIndex)
		{
			ThemableColor themableColor = (ThemableColor)base.GetValue(cells, rowIndex, columnIndex);
			return SortAndFilterHelper.GetThemableColorWithLocalColors(themableColor, this.worksheet.Workbook.Theme.ColorScheme);
		}

		internal override ISortCondition Copy(int newRelativeColumnIndex)
		{
			return new ForeColorSortCondition(newRelativeColumnIndex, this.Color, base.SortOrder);
		}

		public override bool Equals(object obj)
		{
			ForeColorSortCondition foreColorSortCondition = obj as ForeColorSortCondition;
			return foreColorSortCondition != null && this.Color.Equals(foreColorSortCondition.Color) && base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Color.GetHashCode(), base.GetHashCode());
		}

		readonly ThemableColor color;

		readonly SortOrder sortOrder;

		IComparer<SortValue> comparer;

		ThemableColor actualColor;

		Worksheet worksheet;
	}
}
