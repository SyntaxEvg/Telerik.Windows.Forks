using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class CellAreaReference
	{
		public CellReference FromCellReference
		{
			get
			{
				return this.fromCellReference;
			}
		}

		public CellReference ToCellReference
		{
			get
			{
				return this.toCellReference;
			}
		}

		public int RowsCount
		{
			get
			{
				return this.toCellReference.ActualRowIndex - this.fromCellReference.ActualRowIndex + 1;
			}
		}

		public int ColumnsCount
		{
			get
			{
				return this.toCellReference.ActualColumnIndex - this.fromCellReference.ActualColumnIndex + 1;
			}
		}

		public bool IsValid
		{
			get
			{
				return TelerikHelper.IsValidRowIndex(this.fromCellReference.ActualRowIndex) && TelerikHelper.IsValidRowIndex(this.toCellReference.ActualRowIndex) && TelerikHelper.IsValidColumnIndex(this.fromCellReference.ActualColumnIndex) && TelerikHelper.IsValidColumnIndex(this.toCellReference.ActualColumnIndex);
			}
		}

		internal CellAreaReference(CellReference cellReference)
			: this(cellReference, cellReference, false)
		{
		}

		internal CellAreaReference(CellReference fromCellReference, CellReference toCellReference, bool useBothReferencesInString = false)
		{
			Guard.ThrowExceptionIfNull<CellReference>(fromCellReference, "fromCellReference");
			Guard.ThrowExceptionIfNull<CellReference>(toCellReference, "toCellReference");
			this.fromCellReference = CellReference.GetTopLeftCellReference(fromCellReference, toCellReference);
			this.toCellReference = CellReference.GetBottomRightCellReference(fromCellReference, toCellReference);
			this.useBothReferencesInString = useBothReferencesInString;
		}

		internal static CellAreaReference CreateRange(CellAreaReference firstRange, CellAreaReference secondRange)
		{
			Guard.ThrowExceptionIfNull<CellAreaReference>(firstRange, "firstRange");
			Guard.ThrowExceptionIfNull<CellAreaReference>(secondRange, "secondRange");
			CellReference topLeftCellReference = CellReference.GetTopLeftCellReference(firstRange.FromCellReference, secondRange.FromCellReference);
			CellReference bottomRightCellReference = CellReference.GetBottomRightCellReference(firstRange.ToCellReference, secondRange.ToCellReference);
			return new CellAreaReference(topLeftCellReference, bottomRightCellReference, false);
		}

		internal static CellAreaReference CreateIntersection(CellAreaReference firstRange, CellAreaReference secondRange)
		{
			Guard.ThrowExceptionIfNull<CellAreaReference>(firstRange, "firstRange");
			Guard.ThrowExceptionIfNull<CellAreaReference>(secondRange, "secondRange");
			if (CellAreaReference.AreRangesIntersecting(firstRange, secondRange))
			{
				CellReference topLeftCellReference = CellReference.GetTopLeftCellReference(firstRange.ToCellReference, secondRange.ToCellReference);
				CellReference bottomRightCellReference = CellReference.GetBottomRightCellReference(firstRange.FromCellReference, secondRange.FromCellReference);
				return new CellAreaReference(topLeftCellReference, bottomRightCellReference, false);
			}
			return null;
		}

		internal static CellAreaReference CreateFromCellName(string cellName, CellIndex cellIndex)
		{
			return CellAreaReference.CreateFromCellName(cellName, cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		internal static CellAreaReference CreateFromCellName(string cellName, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNull<string>(cellName, "cellName");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			CellReference cellReference = CellReference.CreateFromCellName(cellName, rowIndex, columnIndex);
			if (cellReference != null)
			{
				return new CellAreaReference(cellReference);
			}
			return null;
		}

		internal static CellAreaReference CreateFromTwoCellNames(string leftCellName, string rightCellName, int rowIndex, int columnIndex, bool useTwoReferences)
		{
			Guard.ThrowExceptionIfNullOrEmpty(leftCellName, "leftCellName");
			Guard.ThrowExceptionIfNullOrEmpty(rightCellName, "rightCellName");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			CellReference cellReference = CellReference.CreateFromCellName(leftCellName, rowIndex, columnIndex);
			CellReference cellReference2 = CellReference.CreateFromCellName(rightCellName, rowIndex, columnIndex);
			if (leftCellName != null && cellReference2 != null)
			{
				return new CellAreaReference(cellReference, cellReference2, useTwoReferences);
			}
			return null;
		}

		static bool AreRangesIntersecting(CellAreaReference range1, CellAreaReference range2)
		{
			return CellAreaReference.AreCellReferencesInRange(range1, range2) || CellAreaReference.AreCellReferencesInRange(range2, range1);
		}

		static bool AreCellReferencesInRange(CellAreaReference range1, CellAreaReference range2)
		{
			int actualColumnIndex = range1.FromCellReference.ActualColumnIndex;
			int actualColumnIndex2 = range1.ToCellReference.ActualColumnIndex;
			int actualRowIndex = range1.FromCellReference.ActualRowIndex;
			int actualRowIndex2 = range1.ToCellReference.ActualRowIndex;
			return CellAreaReference.IsCellReferenceInRange(actualRowIndex, actualColumnIndex, range2) || CellAreaReference.IsCellReferenceInRange(actualRowIndex, actualColumnIndex2, range2) || CellAreaReference.IsCellReferenceInRange(actualRowIndex2, actualColumnIndex2, range2) || CellAreaReference.IsCellReferenceInRange(actualRowIndex2, actualColumnIndex, range2);
		}

		static bool IsCellReferenceInRange(int rowIndex, int columnIndex, CellAreaReference cellReferenceRange)
		{
			return cellReferenceRange.FromCellReference.ActualColumnIndex <= columnIndex && cellReferenceRange.ToCellReference.ActualColumnIndex >= columnIndex && cellReferenceRange.FromCellReference.ActualRowIndex <= rowIndex && cellReferenceRange.ToCellReference.ActualRowIndex >= rowIndex;
		}

		internal CellAreaReference CloneAndTranslate(int rowIndex, int columnIndex, bool onOverflowStartOver)
		{
			return new CellAreaReference(this.FromCellReference.CloneAndTranslate(rowIndex, columnIndex, onOverflowStartOver), this.ToCellReference.CloneAndTranslate(rowIndex, columnIndex, onOverflowStartOver), this.useBothReferencesInString);
		}

		internal CellAreaReference CloneAndTranslateUponInsertRemoveCells(ExpressionCloneAndTranslateContext context, int rangeFromIndex, int rangeToIndex, int referenceFromIndex, int referenceToIndex, bool translateSource, bool translateTarget)
		{
			int rowColumnCount = (context.IsHorizontal ? context.Range.ColumnCount : context.Range.RowCount);
			if (!context.IsRemove)
			{
				return this.CloneAndTranslateUponInsertCells(translateSource, translateTarget, context.IsHorizontal, rowColumnCount, rangeFromIndex, referenceFromIndex, context.OnOverflowStartOver);
			}
			if (translateTarget && rangeFromIndex <= referenceFromIndex && referenceToIndex <= rangeToIndex)
			{
				return null;
			}
			return this.CloneAndTranslateUponRemoveCells(translateSource, translateTarget, context.IsHorizontal, rowColumnCount, rangeFromIndex, rangeToIndex, referenceFromIndex, referenceToIndex, context.OnOverflowStartOver);
		}

		internal CellAreaReference CloneAndTranslateUponRemoveCells(bool translateSource, bool translateTarget, bool isHorizontal, int rowColumnCount, int rangeFromIndex, int rangeToIndex, int referenceFromIndex, int referenceToIndex, bool onOverflowStartOver)
		{
			int indexShift = 0;
			int num = 0;
			int indexShift2 = 0;
			int offsetShift = 0;
			if (translateSource && translateTarget)
			{
				indexShift = -rowColumnCount;
				indexShift2 = -rowColumnCount;
				if (rangeToIndex >= referenceFromIndex)
				{
					if (rangeFromIndex >= referenceFromIndex)
					{
						num = rowColumnCount;
					}
					else
					{
						num = referenceFromIndex - rangeToIndex;
						offsetShift = num;
					}
				}
			}
			else if (translateTarget)
			{
				if (rangeToIndex < referenceFromIndex)
				{
					num = -rowColumnCount;
					offsetShift = -rowColumnCount;
				}
				else if (rangeFromIndex >= referenceFromIndex)
				{
					offsetShift = rangeFromIndex - Math.Min(referenceToIndex, rangeToIndex) - 1;
				}
				else
				{
					num = rangeFromIndex - referenceFromIndex;
					offsetShift = -rowColumnCount;
				}
			}
			else if (translateSource)
			{
				indexShift = -rowColumnCount;
				num = rowColumnCount;
				indexShift2 = -rowColumnCount;
				offsetShift = rowColumnCount;
			}
			return new CellAreaReference(this.fromCellReference.CloneAndTranslateUponInsertRemoveCells(isHorizontal, indexShift, num, onOverflowStartOver), this.toCellReference.CloneAndTranslateUponInsertRemoveCells(isHorizontal, indexShift2, offsetShift, onOverflowStartOver), this.useBothReferencesInString);
		}

		internal CellAreaReference CloneAndTranslateUponInsertCells(bool translateSource, bool translateTarget, bool isHorizontal, int rowColumnCount, int rangeFromIndex, int referenceFromIndex, bool onOverflowStartOver)
		{
			int indexShift = 0;
			int offsetShift = 0;
			int indexShift2 = 0;
			int offsetShift2 = 0;
			if (translateSource && translateTarget)
			{
				indexShift = rowColumnCount;
				indexShift2 = rowColumnCount;
				if (rangeFromIndex > referenceFromIndex)
				{
					offsetShift = -rowColumnCount;
				}
			}
			else if (translateTarget)
			{
				offsetShift2 = rowColumnCount;
				if (rangeFromIndex <= referenceFromIndex)
				{
					offsetShift = rowColumnCount;
				}
			}
			else if (translateSource)
			{
				indexShift = rowColumnCount;
				offsetShift = -rowColumnCount;
				indexShift2 = rowColumnCount;
				offsetShift2 = -rowColumnCount;
			}
			return new CellAreaReference(this.fromCellReference.CloneAndTranslateUponInsertRemoveCells(isHorizontal, indexShift, offsetShift, onOverflowStartOver), this.toCellReference.CloneAndTranslateUponInsertRemoveCells(isHorizontal, indexShift2, offsetShift2, onOverflowStartOver), this.useBothReferencesInString);
		}

		public override bool Equals(object obj)
		{
			CellAreaReference cellAreaReference = obj as CellAreaReference;
			return cellAreaReference != null && this.FromCellReference.Equals(cellAreaReference.FromCellReference) && this.ToCellReference.Equals(cellAreaReference.ToCellReference);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.FromCellReference.GetHashCode(), this.ToCellReference.GetHashCode());
		}

		public override string ToString()
		{
			if (!this.IsValid)
			{
				return ErrorExpressions.ReferenceError.Value;
			}
			if (!this.useBothReferencesInString && this.FromCellReference.Equals(this.ToCellReference))
			{
				return NameConverter.ConvertCellReferenceToName(this.FromCellReference);
			}
			return NameConverter.ConvertCellReferenceRangeToName(this.FromCellReference, this.ToCellReference);
		}

		readonly CellReference fromCellReference;

		readonly CellReference toCellReference;

		readonly bool useBothReferencesInString;
	}
}
