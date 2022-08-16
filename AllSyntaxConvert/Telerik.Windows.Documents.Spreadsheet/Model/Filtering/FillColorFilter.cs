using System;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class FillColorFilter : FilterBase<IFill>, IWorksheetFilter
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

		public FillColorFilter(int relativeColumnIndex, IFill fill)
			: base(relativeColumnIndex)
		{
			Guard.ThrowExceptionIfNull<IFill>(fill, "fill");
			this.fill = fill;
		}

		internal override IFilter Copy(int newRelativeColumnIndex)
		{
			return new FillColorFilter(newRelativeColumnIndex, this.Fill);
		}

		void IWorksheetFilter.SetWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.actualFill = SortAndFilterHelper.GetIFillWithLocalColors(this.fill, this.worksheet.Workbook.Theme.ColorScheme);
		}

		public override bool ShouldShowValue(object value)
		{
			IFill fill = value as IFill;
			Guard.ThrowExceptionIfNull<Worksheet>(this.worksheet, "worksheet");
			fill = SortAndFilterHelper.GetIFillWithLocalColors(fill, this.worksheet.Workbook.Theme.ColorScheme);
			return fill.Equals(this.actualFill);
		}

		public override bool Equals(object obj)
		{
			FillColorFilter fillColorFilter = obj as FillColorFilter;
			return fillColorFilter != null && this.Fill.Equals(fillColorFilter.Fill);
		}

		public override int GetHashCode()
		{
			return this.Fill.GetHashCode();
		}

		readonly IFill fill;

		IFill actualFill;

		Worksheet worksheet;
	}
}
