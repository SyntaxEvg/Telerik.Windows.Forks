using System;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class ForeColorFilter : FilterBase<ThemableColor>, IWorksheetFilter
	{
		public ThemableColor Color
		{
			get
			{
				return this.color;
			}
		}

		void IWorksheetFilter.SetWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.actualColor = SortAndFilterHelper.GetThemableColorWithLocalColors(this.color, this.worksheet.Workbook.Theme.ColorScheme);
		}

		protected override IPropertyDefinition<ThemableColor> PropertyDefinition
		{
			get
			{
				return CellPropertyDefinitions.ForeColorProperty;
			}
		}

		public ForeColorFilter(int relativeColumnIndex, ThemableColor color)
			: base(relativeColumnIndex)
		{
			Guard.ThrowExceptionIfNull<ThemableColor>(color, "color");
			this.color = color;
		}

		internal override IFilter Copy(int newRelativeColumnIndex)
		{
			return new ForeColorFilter(newRelativeColumnIndex, this.Color);
		}

		public override bool ShouldShowValue(object value)
		{
			ThemableColor themableColor = value as ThemableColor;
			Guard.ThrowExceptionIfNull<Worksheet>(this.worksheet, "worksheet");
			themableColor = SortAndFilterHelper.GetThemableColorWithLocalColors(themableColor, this.worksheet.Workbook.Theme.ColorScheme);
			return themableColor.Equals(this.actualColor);
		}

		public override bool Equals(object obj)
		{
			ForeColorFilter foreColorFilter = obj as ForeColorFilter;
			return foreColorFilter != null && this.Color.Equals(foreColorFilter.Color);
		}

		public override int GetHashCode()
		{
			return this.GetHashCode();
		}

		readonly ThemableColor color;

		ThemableColor actualColor;

		Worksheet worksheet;
	}
}
