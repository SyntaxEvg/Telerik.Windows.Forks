using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class CopyPasteContext
	{
		public Worksheet FromWorksheet
		{
			get
			{
				return this.fromWorksheet;
			}
		}

		public List<CellRange> FromCellRange
		{
			get
			{
				return this.fromCellRange;
			}
		}

		public Worksheet ToWorksheet
		{
			get
			{
				return this.toWorksheet;
			}
		}

		public CellRange ToCellRange
		{
			get
			{
				return this.toCellRange;
			}
		}

		public IEnumerable<FloatingShapeBase> FromShapes
		{
			get
			{
				return this.fromShapes;
			}
		}

		public IEnumerable<FloatingShapeBase> ToShapes
		{
			get
			{
				return this.toShapes;
			}
		}

		public PasteOptions PasteOptions
		{
			get
			{
				return this.pasteOptions;
			}
		}

		public bool IsCopying
		{
			get
			{
				return this.isCopying;
			}
		}

		public bool ShouldPasteAsText
		{
			get
			{
				return this.shouldPasteAsText;
			}
		}

		public bool IsCellCopyPaste
		{
			get
			{
				return this.isCellCopyPaste;
			}
		}

		public List<CellRange> AffectedCellRanges
		{
			get
			{
				return this.affectedCellRanges;
			}
			set
			{
				Guard.ThrowExceptionIfNotNull<List<CellRange>>(this.affectedCellRanges, "affectedCellRanges");
				this.affectedCellRanges = value;
			}
		}

		public int[] HiddenRows
		{
			get
			{
				return this.hiddenRows;
			}
		}

		public int[] HiddenColumns
		{
			get
			{
				return this.hiddenColumns;
			}
		}

		public CopyPasteContext(Worksheet fromWorksheet, CellRange fromCellRange, Worksheet toWorksheet, CellRange toCellRange, IEnumerable<FloatingShapeBase> fromShapes, IEnumerable<FloatingShapeBase> toShapes, PasteOptions pasteOptions, bool isCellCopyPaste, bool isCopying, bool shouldPasteAsText)
		{
			if (isCellCopyPaste)
			{
				Guard.ThrowExceptionIfNull<Worksheet>(fromWorksheet, "fromWorksheet");
				Guard.ThrowExceptionIfNull<CellRange>(fromCellRange, "fromCellRange");
				Guard.ThrowExceptionIfNull<Worksheet>(toWorksheet, "toWorksheet");
				Guard.ThrowExceptionIfNull<CellRange>(toCellRange, "toCellRange");
			}
			else
			{
				Guard.ThrowExceptionIfNull<IEnumerable<FloatingShapeBase>>(fromShapes, "fromShapes");
				Guard.ThrowExceptionIfNull<Worksheet>(fromWorksheet, "fromWorksheet");
				Guard.ThrowExceptionIfNull<Worksheet>(toWorksheet, "toWorksheet");
			}
			this.fromWorksheet = fromWorksheet;
			this.fromCellRange = CopyPasteContext.GetFromCellRange(this.fromWorksheet, fromCellRange, out this.hiddenRows, out this.hiddenColumns);
			this.toWorksheet = toWorksheet;
			this.toCellRange = toCellRange;
			this.fromShapes = fromShapes;
			this.toShapes = toShapes;
			this.isCopying = isCopying;
			this.shouldPasteAsText = shouldPasteAsText;
			this.isCellCopyPaste = isCellCopyPaste;
			this.pasteOptions = CopyPasteContext.GetPasteOptions(pasteOptions, shouldPasteAsText);
		}

		public static List<CellRange> GetFromCellRange(Worksheet fromWorksheet, CellRange fromCellRange)
		{
			int[] array;
			int[] array2;
			return CopyPasteContext.GetFromCellRange(fromWorksheet, fromCellRange, out array, out array2);
		}

		static List<CellRange> GetFromCellRange(Worksheet fromWorksheet, CellRange fromCellRange, out int[] hiddenRows, out int[] hiddenColumns)
		{
			if (fromCellRange == null)
			{
				hiddenRows = null;
				hiddenColumns = null;
			}
			bool flag = !fromWorksheet.Filter.FilterIsApplied;
			List<CellRange> result;
			if (flag)
			{
				result = new List<CellRange> { fromCellRange };
				hiddenRows = new int[0];
				hiddenColumns = new int[0];
			}
			else
			{
				result = SpreadsheetHelper.SplitIntoRangesAccordingToHidden(fromCellRange, fromWorksheet, out hiddenRows, out hiddenColumns);
			}
			return result;
		}

		static PasteOptions GetPasteOptions(PasteOptions pasteOptions, bool shouldPasteAsText)
		{
			if (shouldPasteAsText)
			{
				return new PasteOptions(PasteType.Values, false);
			}
			if (pasteOptions != null)
			{
				return pasteOptions;
			}
			return PasteOptions.All;
		}

		public bool ShouldCopyPasteHyperlinks()
		{
			return this.IsCopying || this.PasteOptions.PasteType == PasteType.All;
		}

		internal bool ShouldCopyPasteColumnWidths()
		{
			return this.PasteOptions.PasteType == PasteType.ColumnWidths || this.IsCopying;
		}

		readonly Worksheet fromWorksheet;

		readonly List<CellRange> fromCellRange;

		readonly Worksheet toWorksheet;

		readonly CellRange toCellRange;

		readonly IEnumerable<FloatingShapeBase> fromShapes;

		readonly IEnumerable<FloatingShapeBase> toShapes;

		readonly PasteOptions pasteOptions;

		readonly bool isCopying;

		readonly bool shouldPasteAsText;

		readonly bool isCellCopyPaste;

		readonly int[] hiddenRows;

		readonly int[] hiddenColumns;

		List<CellRange> affectedCellRanges;
	}
}
