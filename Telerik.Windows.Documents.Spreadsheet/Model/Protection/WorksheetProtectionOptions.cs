using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Protection
{
	public class WorksheetProtectionOptions
	{
		public bool AllowFormatCells
		{
			get
			{
				return this.allowFormatCells;
			}
		}

		public bool AllowFormatColumns
		{
			get
			{
				return this.allowFormatColumns;
			}
		}

		public bool AllowFormatRows
		{
			get
			{
				return this.allowFormatRows;
			}
		}

		public bool AllowDeleteRows
		{
			get
			{
				return this.allowDeleteRows;
			}
		}

		public bool AllowInsertRows
		{
			get
			{
				return this.allowInsertRows;
			}
		}

		public bool AllowDeleteColumns
		{
			get
			{
				return this.allowDeleteColumns;
			}
		}

		public bool AllowInsertColumns
		{
			get
			{
				return this.allowInsertColumns;
			}
		}

		public bool AllowFiltering
		{
			get
			{
				return this.allowFiltering;
			}
		}

		public bool AllowSorting
		{
			get
			{
				return this.allowSorting;
			}
		}

		public WorksheetProtectionOptions(bool allowDeleteRows = false, bool allowInsertRows = false, bool allowDeleteColumns = false, bool allowInsertColumns = false, bool allowFormatCells = false, bool allowFormatColumns = false, bool allowFormatRows = false, bool allowFiltering = false, bool allowSorting = false)
		{
			this.allowFormatCells = allowFormatCells;
			this.allowFormatColumns = allowFormatColumns;
			this.allowFormatRows = allowFormatRows;
			this.allowDeleteRows = allowDeleteRows;
			this.allowInsertRows = allowInsertRows;
			this.allowDeleteColumns = allowDeleteColumns;
			this.allowInsertColumns = allowInsertColumns;
			this.allowFiltering = allowFiltering;
			this.allowSorting = allowSorting;
		}

		public override bool Equals(object obj)
		{
			WorksheetProtectionOptions worksheetProtectionOptions = obj as WorksheetProtectionOptions;
			return worksheetProtectionOptions != null && (this.AllowDeleteColumns == worksheetProtectionOptions.AllowDeleteColumns && this.AllowDeleteRows == worksheetProtectionOptions.AllowDeleteRows && this.AllowInsertColumns == worksheetProtectionOptions.AllowInsertColumns && this.AllowInsertRows == worksheetProtectionOptions.AllowInsertRows && this.AllowFormatCells == worksheetProtectionOptions.AllowFormatCells && this.AllowFormatColumns == worksheetProtectionOptions.AllowFormatColumns && this.AllowFormatRows == worksheetProtectionOptions.AllowFormatRows && this.AllowFiltering == worksheetProtectionOptions.AllowFiltering) && this.AllowSorting == worksheetProtectionOptions.AllowSorting;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.AllowDeleteColumns.GetHashCode(), this.AllowDeleteRows.GetHashCode(), this.AllowInsertColumns.GetHashCode(), this.AllowInsertRows.GetHashCode(), this.AllowFormatCells.GetHashCode(), this.AllowFormatColumns.GetHashCode(), this.AllowFormatRows.GetHashCode(), this.AllowFiltering.GetHashCode(), this.AllowSorting.GetHashCode());
		}

		public WorksheetProtectionOptions Clone()
		{
			return new WorksheetProtectionOptions(this.AllowDeleteRows, this.AllowInsertRows, this.AllowDeleteColumns, this.AllowInsertColumns, this.AllowFormatCells, this.AllowFormatColumns, this.AllowFormatRows, this.AllowFiltering, this.AllowSorting);
		}

		public static readonly WorksheetProtectionOptions Default = new WorksheetProtectionOptions(false, false, false, false, false, false, false, false, false);

		readonly bool allowFormatCells;

		readonly bool allowFormatColumns;

		readonly bool allowFormatRows;

		readonly bool allowDeleteRows;

		readonly bool allowInsertRows;

		readonly bool allowDeleteColumns;

		readonly bool allowInsertColumns;

		readonly bool allowFiltering;

		readonly bool allowSorting;
	}
}
