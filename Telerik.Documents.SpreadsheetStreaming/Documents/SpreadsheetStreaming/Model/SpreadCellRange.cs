using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model
{
	class SpreadCellRange
	{
		public SpreadCellRange(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, fromRowIndex, "fromRowIndex");
			Guard.ThrowExceptionIfLessThan<int>(0, fromColumnIndex, "fromColumnIndex");
			Guard.ThrowExceptionIfLessThan<int>(fromRowIndex, toRowIndex, "toRowIndex");
			Guard.ThrowExceptionIfLessThan<int>(fromColumnIndex, toColumnIndex, "toColumnIndex");
			this.FromRowIndex = fromRowIndex;
			this.FromColumnIndex = fromColumnIndex;
			this.ToRowIndex = toRowIndex;
			this.ToColumnIndex = toColumnIndex;
		}

		public int FromRowIndex { get; set; }

		public int FromColumnIndex { get; set; }

		public int ToRowIndex { get; set; }

		public int ToColumnIndex { get; set; }

		public override bool Equals(object obj)
		{
			SpreadCellRange spreadCellRange = obj as SpreadCellRange;
			return spreadCellRange != null && (this.FromRowIndex == spreadCellRange.FromRowIndex && this.FromColumnIndex == spreadCellRange.FromColumnIndex && this.ToRowIndex == spreadCellRange.ToRowIndex) && this.ToColumnIndex == spreadCellRange.ToColumnIndex;
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.FromRowIndex.GetHashCodeOrZero(), this.FromColumnIndex.GetHashCodeOrZero(), this.ToRowIndex.GetHashCodeOrZero(), this.ToColumnIndex.GetHashCodeOrZero());
		}

		internal bool ContainsCell(int rowIndex, int columnIndex)
		{
			return this.FromRowIndex <= rowIndex && rowIndex <= this.ToRowIndex && this.FromColumnIndex <= columnIndex && columnIndex <= this.ToColumnIndex;
		}
	}
}
