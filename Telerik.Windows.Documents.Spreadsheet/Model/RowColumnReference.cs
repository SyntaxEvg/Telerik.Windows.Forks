using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class RowColumnReference
	{
		public int ActualIndex
		{
			get
			{
				if (this.IsAbsolute)
				{
					return this.index;
				}
				int num = this.index + this.offset;
				if (this.onOverflowStartOver)
				{
					if (num < 0)
					{
						return this.rowsColumnsCount + num;
					}
					if (num >= this.rowsColumnsCount)
					{
						return num - this.rowsColumnsCount;
					}
				}
				return num;
			}
		}

		public bool IsAbsolute
		{
			get
			{
				return this.isAbsolute;
			}
		}

		public RowColumnReference()
			: this(false, 0)
		{
		}

		internal RowColumnReference(bool onOverflowStartOver, int rowsColumnsCount)
		{
			this.onOverflowStartOver = onOverflowStartOver;
			this.rowsColumnsCount = rowsColumnsCount;
		}

		internal static RowColumnReference CreateRelativeRowColumnReference(int startIndex, int offset)
		{
			return RowColumnReference.CreateRelativeRowColumnReference(startIndex, offset, false, 0);
		}

		internal static RowColumnReference CreateRelativeRowColumnReference(int startIndex, int offset, bool onOverflowStartOver, int rowsColumnsCount)
		{
			return new RowColumnReference(onOverflowStartOver, rowsColumnsCount)
			{
				isAbsolute = false,
				index = startIndex,
				offset = offset
			};
		}

		internal static RowColumnReference CreateAbsoluteRowColumnReference(int targetIndex)
		{
			return new RowColumnReference(false, 0)
			{
				isAbsolute = true,
				index = targetIndex
			};
		}

		internal static RowColumnReference CreateRowColumnReference(bool isAbsolute, int index, int cellIndex, bool onOverflowStartOver, int rowsColumnsCount)
		{
			if (isAbsolute)
			{
				return RowColumnReference.CreateAbsoluteRowColumnReference(index);
			}
			return RowColumnReference.CreateRelativeRowColumnReference(cellIndex, index - cellIndex, onOverflowStartOver, rowsColumnsCount);
		}

		internal RowColumnReference CloneAndTranslate(int referenceIndex, bool onOverflowStartOver, int rowsColumnsCount)
		{
			return new RowColumnReference(onOverflowStartOver, rowsColumnsCount)
			{
				isAbsolute = this.IsAbsolute,
				index = (this.IsAbsolute ? this.index : referenceIndex),
				offset = this.offset
			};
		}

		internal RowColumnReference CloneAndTranslateUponInsertRemoveCells(int indexShift, int offsetShift, bool onOverflowStartOver, int rowsColumnsCount)
		{
			return new RowColumnReference(onOverflowStartOver, rowsColumnsCount)
			{
				isAbsolute = this.isAbsolute,
				index = this.index + indexShift,
				offset = this.offset + offsetShift
			};
		}

		public override bool Equals(object obj)
		{
			RowColumnReference rowColumnReference = obj as RowColumnReference;
			return rowColumnReference != null && (this.IsAbsolute == rowColumnReference.IsAbsolute && this.index == rowColumnReference.index) && this.offset == rowColumnReference.offset;
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.IsAbsolute.GetHashCode(), this.index.GetHashCode(), this.offset.GetHashCode());
		}

		readonly bool onOverflowStartOver;

		readonly int rowsColumnsCount;

		bool isAbsolute;

		int index;

		int offset;
	}
}
