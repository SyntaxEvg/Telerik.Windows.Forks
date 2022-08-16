using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class CellIndexBase : IComparable<CellIndexBase>
	{
		public abstract int RowIndex { get; }

		public abstract int ColumnIndex { get; }

		public override string ToString()
		{
			return string.Format("[RowIndex={0}, ColumnIndex={1}]", this.RowIndex, this.ColumnIndex);
		}

		public override bool Equals(object obj)
		{
			CellIndexBase cellIndexBase = obj as CellIndexBase;
			return !(cellIndexBase == null) && this == cellIndexBase;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.RowIndex.GetHashCode(), this.ColumnIndex.GetHashCode());
		}

		public static bool operator ==(CellIndexBase left, CellIndexBase right)
		{
			if (object.ReferenceEquals(left, right))
			{
				return true;
			}
			if (left == null)
			{
				return right == null;
			}
			return right != null && left.RowIndex == right.RowIndex && left.ColumnIndex == right.ColumnIndex;
		}

		public static bool operator !=(CellIndexBase left, CellIndexBase right)
		{
			return !(left == right);
		}

		public static bool operator <(CellIndexBase left, CellIndexBase right)
		{
			return !(left == null) && (left.RowIndex < right.RowIndex || (left.RowIndex == right.RowIndex && left.ColumnIndex < right.ColumnIndex));
		}

		public static bool operator >(CellIndexBase left, CellIndexBase right)
		{
			return !(left == null) && (left.RowIndex > right.RowIndex || (left.RowIndex == right.RowIndex && left.ColumnIndex > right.ColumnIndex));
		}

		public static bool operator <=(CellIndexBase left, CellIndexBase right)
		{
			return !(left > right);
		}

		public static bool operator >=(CellIndexBase left, CellIndexBase right)
		{
			return !(left < right);
		}

		public int CompareTo(CellIndexBase other)
		{
			if (this == other)
			{
				return 0;
			}
			if (this < other)
			{
				return -1;
			}
			return 1;
		}
	}
}
