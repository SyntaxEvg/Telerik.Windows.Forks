using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ExpressionCloneAndTranslateContext
	{
		public Worksheet Worksheet { get; set; }

		public int RowIndex { get; set; }

		public int ColumnIndex { get; set; }

		public InputStringCollection OldFormatCollection { get; set; }

		public InputStringCollection NewFormatCollection { get; set; }

		public bool IsInsertingRemovingCells { get; set; }

		public bool IsHorizontal { get; set; }

		public bool IsRemove { get; set; }

		public CellRange Range { get; set; }

		public bool OnOverflowStartOver { get; set; }

		public ExpressionCloneAndTranslateContext(Worksheet worksheet, int rowIndex, int columnIndex, bool onOverflowStartOver)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.Worksheet = worksheet;
			this.RowIndex = rowIndex;
			this.ColumnIndex = columnIndex;
			this.OnOverflowStartOver = onOverflowStartOver;
		}

		public ExpressionCloneAndTranslateContext(Worksheet worksheet, int rowIndex, int columnIndex, InputStringCollection oldFormatCollection, InputStringCollection newFormatCollection, bool onOverflowStartOver)
			: this(worksheet, rowIndex, columnIndex, onOverflowStartOver)
		{
			Guard.ThrowExceptionIfNull<InputStringCollection>(oldFormatCollection, "oldFormatCollection");
			Guard.ThrowExceptionIfNull<InputStringCollection>(newFormatCollection, "newFormatCollection");
			this.OldFormatCollection = oldFormatCollection;
			this.NewFormatCollection = newFormatCollection;
		}

		public ExpressionCloneAndTranslateContext(Worksheet worksheet, int rowIndex, int columnIndex, InputStringCollection oldFormatCollection, InputStringCollection newFormatCollection, bool isHorizontal, CellRange range, bool isRemove, bool onOverflowStartOver)
			: this(worksheet, rowIndex, columnIndex, oldFormatCollection, newFormatCollection, onOverflowStartOver)
		{
			this.IsInsertingRemovingCells = true;
			this.IsHorizontal = isHorizontal;
			this.Range = range;
			this.IsRemove = isRemove;
		}
	}
}
