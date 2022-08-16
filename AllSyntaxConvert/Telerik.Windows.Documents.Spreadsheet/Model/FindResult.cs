using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class FindResult
	{
		public WorksheetCellIndex FoundCell
		{
			get
			{
				return this.foundCell;
			}
		}

		public string ResultValue
		{
			get
			{
				if (this.resultValue == null)
				{
					this.resultValue = this.GetResultValue();
				}
				return this.resultValue;
			}
		}

		public string RawValue
		{
			get
			{
				if (this.rawValue == null)
				{
					this.rawValue = this.GetRawValue();
				}
				return this.rawValue;
			}
		}

		internal FindResult(WorksheetCellIndex foundCell)
		{
			this.foundCell = foundCell;
		}

		string GetResultValue()
		{
			CellSelection cellSelection = this.FoundCell.Worksheet.Cells[this.FoundCell.CellIndex];
			CellValueFormat value = cellSelection.GetFormat().Value;
			return cellSelection.GetValue().Value.GetResultValueAsString(value);
		}

		string GetRawValue()
		{
			CellSelection cellSelection = this.FoundCell.Worksheet.Cells[this.FoundCell.CellIndex];
			return cellSelection.GetValue().Value.RawValue;
		}

		public override bool Equals(object obj)
		{
			FindResult findResult = obj as FindResult;
			return findResult != null && TelerikHelper.EqualsOfT<WorksheetCellIndex>(this.FoundCell, findResult.FoundCell);
		}

		public override int GetHashCode()
		{
			return this.FoundCell.GetHashCodeOrZero();
		}

		readonly WorksheetCellIndex foundCell;

		string resultValue;

		string rawValue;
	}
}
