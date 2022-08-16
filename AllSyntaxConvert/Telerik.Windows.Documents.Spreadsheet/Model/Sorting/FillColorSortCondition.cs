using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting.Conditions;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public sealed class FillColorSortCondition : OrderedSortConditionBase<IFill>, IWorksheetSortCondition
	{
		public IFill Fill
		{
			get
			{
				return this.fill;
			}
		}

		protected override IPropertyDefinition<IFill> PropertyDefinition
		{
			get
			{
				return CellPropertyDefinitions.FillProperty;
			}
		}

		public override IComparer<SortValue> Comparer
		{
			get
			{
				if (this.comparer == null)
				{
					this.comparer = new FillColorComparer(this.actualFill, this.sortOrder);
				}
				return this.comparer;
			}
		}

		public FillColorSortCondition(int relativeIndex, IFill fill, SortOrder sortOrder)
			: base(relativeIndex, sortOrder)
		{
			this.sortOrder = sortOrder;
			this.fill = fill;
		}

		void IWorksheetSortCondition.SetWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.actualFill = SortAndFilterHelper.GetIFillWithLocalColors(this.fill, this.worksheet.Workbook.Theme.ColorScheme);
		}

		public override object GetValue(Cells cells, int rowIndex, int columnIndex)
		{
			IFill fill = (IFill)base.GetValue(cells, rowIndex, columnIndex);
			return SortAndFilterHelper.GetIFillWithLocalColors(fill, this.worksheet.Workbook.Theme.ColorScheme);
		}

		internal override ISortCondition Copy(int newRelativeColumnIndex)
		{
			return new FillColorSortCondition(newRelativeColumnIndex, this.Fill, base.SortOrder);
		}

		public override bool Equals(object obj)
		{
			FillColorSortCondition fillColorSortCondition = obj as FillColorSortCondition;
			return fillColorSortCondition != null && this.Fill.Equals(fillColorSortCondition.Fill) && base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Fill.GetHashCode(), base.GetHashCode());
		}

		readonly IFill fill;

		readonly SortOrder sortOrder;

		IFill actualFill;

		IComparer<SortValue> comparer;

		Worksheet worksheet;
	}
}
