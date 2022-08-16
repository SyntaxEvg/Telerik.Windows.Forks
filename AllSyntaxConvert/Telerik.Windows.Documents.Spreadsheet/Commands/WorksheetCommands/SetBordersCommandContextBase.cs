using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	abstract class SetBordersCommandContextBase : WorksheetCommandContextBase
	{
		public CellBorders NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		internal CellRange LeftBorderCellRange
		{
			get
			{
				return this.leftBorderCellRange;
			}
		}

		internal CellRange RightBorderCellRange
		{
			get
			{
				return this.rightBorderCellRange;
			}
		}

		internal CellRange TopBorderCellRange
		{
			get
			{
				return this.topBorderCellRange;
			}
		}

		internal CellRange BottomBorderCellRange
		{
			get
			{
				return this.bottomBorderCellRange;
			}
		}

		internal ICompressedList<CellBorder> OldLeftBorderValues { get; set; }

		internal ICompressedList<CellBorder> OldRightBorderValues { get; set; }

		internal ICompressedList<CellBorder> OldTopBorderValues { get; set; }

		internal ICompressedList<CellBorder> OldBottomBorderValues { get; set; }

		internal ICompressedList<CellBorder> OldDiagonalUpBorderValues { get; set; }

		internal ICompressedList<CellBorder> OldDiagonalDownBorderValues { get; set; }

		internal ICompressedList<CellBorder> NewLeftBorderValues
		{
			get
			{
				if (this.newLeftBorderValues == null)
				{
					this.newLeftBorderValues = this.CalculateNewLeftBorderValues();
				}
				return this.newLeftBorderValues;
			}
		}

		internal ICompressedList<CellBorder> NewRightBorderValues
		{
			get
			{
				if (this.newRightBorderValues == null)
				{
					this.newRightBorderValues = this.CalculateNewRightBorderValues();
				}
				return this.newRightBorderValues;
			}
		}

		internal ICompressedList<CellBorder> NewTopBorderValues
		{
			get
			{
				if (this.newTopBorderValues == null)
				{
					this.newTopBorderValues = this.CalculateNewTopBorderValues();
				}
				return this.newTopBorderValues;
			}
		}

		internal ICompressedList<CellBorder> NewBottomBorderValues
		{
			get
			{
				if (this.newBottomBorderValues == null)
				{
					this.newBottomBorderValues = this.CalculateNewBottomBorderValues();
				}
				return this.newBottomBorderValues;
			}
		}

		internal ICompressedList<CellBorder> NewDiagonalUpBorderValues
		{
			get
			{
				if (this.newDiagonalUpBorderValues == null)
				{
					this.newDiagonalUpBorderValues = this.CalculateNewDiagonalUpBorderValues();
				}
				return this.newDiagonalUpBorderValues;
			}
		}

		internal ICompressedList<CellBorder> NewDiagonalDownBorderValues
		{
			get
			{
				if (this.newDiagonalDownBorderValues == null)
				{
					this.newDiagonalDownBorderValues = this.CalculateNewDiagonalDownBorderValues();
				}
				return this.newDiagonalDownBorderValues;
			}
		}

		public SetBordersCommandContextBase(Worksheet worksheet, CellRange cellRange, CellBorders newValue)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			Guard.ThrowExceptionIfNull<CellBorders>(newValue, "newValue");
			this.newValue = newValue;
			this.cellRange = cellRange;
			if (newValue.Right == null)
			{
				this.leftBorderCellRange = this.cellRange;
			}
			else
			{
				this.leftBorderCellRange = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex, Math.Min(SpreadsheetDefaultValues.ColumnCount - 1, cellRange.ToIndex.ColumnIndex + 1));
			}
			if (newValue.Left == null)
			{
				this.rightBorderCellRange = this.cellRange;
			}
			else
			{
				this.rightBorderCellRange = new CellRange(cellRange.FromIndex.RowIndex, Math.Max(0, cellRange.FromIndex.ColumnIndex - 1), cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex);
			}
			if (newValue.Top == null)
			{
				this.bottomBorderCellRange = this.cellRange;
			}
			else
			{
				this.bottomBorderCellRange = new CellRange(Math.Max(0, cellRange.FromIndex.RowIndex - 1), cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex);
			}
			if (newValue.Bottom == null)
			{
				this.topBorderCellRange = this.cellRange;
				return;
			}
			this.topBorderCellRange = new CellRange(cellRange.FromIndex.RowIndex, cellRange.FromIndex.ColumnIndex, Math.Min(SpreadsheetDefaultValues.RowCount - 1, cellRange.ToIndex.RowIndex + 1), cellRange.ToIndex.ColumnIndex);
		}

		ICompressedList<CellBorder> CalculateNewLeftBorderValues()
		{
			ICompressedList<CellBorder> newValues = null;
			Action action = delegate()
			{
				if (newValues == null)
				{
					newValues = this.GetBorderProperty(CellPropertyDefinitions.LeftBorderProperty, this.LeftBorderCellRange);
				}
			};
			if (this.NewValue.Left != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex, this.cellRange.FromIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.Left);
			}
			if (this.CellRange.ColumnCount > 1 && this.NewValue.InsideVertical != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex + 1, this.CellRange.ToIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.InsideVertical);
			}
			if (this.NewValue.Right != null && this.CellRange.ToIndex.ColumnIndex < SpreadsheetDefaultValues.ColumnCount - 1)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.ToIndex.ColumnIndex + 1, this.CellRange.ToIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex + 1);
				this.ClearValuesInRange(range, newValues);
			}
			return newValues;
		}

		ICompressedList<CellBorder> CalculateNewRightBorderValues()
		{
			ICompressedList<CellBorder> newValues = null;
			Action action = delegate()
			{
				if (newValues == null)
				{
					newValues = this.GetBorderProperty(CellPropertyDefinitions.RightBorderProperty, this.RightBorderCellRange);
				}
			};
			if (this.NewValue.Right != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.ToIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.Right);
			}
			if (this.CellRange.ColumnCount > 1 && this.NewValue.InsideVertical != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex - 1);
				this.SetValuesInRange(range, newValues, this.NewValue.InsideVertical);
			}
			if (this.NewValue.Left != null && this.CellRange.FromIndex.ColumnIndex > 0)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex - 1, this.CellRange.ToIndex.RowIndex, this.cellRange.FromIndex.ColumnIndex - 1);
				this.ClearValuesInRange(range, newValues);
			}
			return newValues;
		}

		ICompressedList<CellBorder> CalculateNewTopBorderValues()
		{
			ICompressedList<CellBorder> newValues = null;
			Action action = delegate()
			{
				if (newValues == null)
				{
					newValues = this.GetBorderProperty(CellPropertyDefinitions.TopBorderProperty, this.TopBorderCellRange);
				}
			};
			if (this.NewValue.Top != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex, this.CellRange.FromIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.Top);
			}
			if (this.CellRange.RowCount > 1 && this.NewValue.InsideHorizontal != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex + 1, this.CellRange.FromIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.InsideHorizontal);
			}
			if (this.NewValue.Bottom != null && this.CellRange.ToIndex.RowIndex < SpreadsheetDefaultValues.RowCount - 1)
			{
				action();
				CellRange range = new CellRange(this.CellRange.ToIndex.RowIndex + 1, this.CellRange.FromIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex + 1, this.cellRange.ToIndex.ColumnIndex);
				this.ClearValuesInRange(range, newValues);
			}
			return newValues;
		}

		ICompressedList<CellBorder> CalculateNewBottomBorderValues()
		{
			ICompressedList<CellBorder> newValues = null;
			Action action = delegate()
			{
				if (newValues == null)
				{
					newValues = this.GetBorderProperty(CellPropertyDefinitions.BottomBorderProperty, this.BottomBorderCellRange);
				}
			};
			if (this.NewValue.Bottom != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.ToIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.Bottom);
			}
			if (this.CellRange.RowCount > 1 && this.NewValue.InsideHorizontal != null)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex, this.CellRange.FromIndex.ColumnIndex, this.CellRange.ToIndex.RowIndex - 1, this.CellRange.ToIndex.ColumnIndex);
				this.SetValuesInRange(range, newValues, this.NewValue.InsideHorizontal);
			}
			if (this.NewValue.Top != null && this.CellRange.FromIndex.RowIndex > 0)
			{
				action();
				CellRange range = new CellRange(this.CellRange.FromIndex.RowIndex - 1, this.CellRange.FromIndex.ColumnIndex, this.CellRange.FromIndex.RowIndex - 1, this.cellRange.ToIndex.ColumnIndex);
				this.ClearValuesInRange(range, newValues);
			}
			return newValues;
		}

		ICompressedList<CellBorder> CalculateNewDiagonalUpBorderValues()
		{
			if (this.NewValue.DiagonalUp == null)
			{
				return null;
			}
			ICompressedList<CellBorder> borderProperty = this.GetBorderProperty(CellPropertyDefinitions.DiagonalUpBorderProperty, this.CellRange);
			this.SetValuesInRange(this.CellRange, borderProperty, this.NewValue.DiagonalUp);
			return borderProperty;
		}

		ICompressedList<CellBorder> CalculateNewDiagonalDownBorderValues()
		{
			if (this.NewValue.DiagonalDown == null)
			{
				return null;
			}
			ICompressedList<CellBorder> borderProperty = this.GetBorderProperty(CellPropertyDefinitions.DiagonalDownBorderProperty, this.CellRange);
			this.SetValuesInRange(this.CellRange, borderProperty, this.NewValue.DiagonalDown);
			return borderProperty;
		}

		public abstract ICompressedList<CellBorder> GetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range);

		public abstract void SetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range, ICompressedList<CellBorder> values);

		protected abstract void SetValuesInRange(CellRange range, ICompressedList<CellBorder> collection, CellBorder value);

		protected abstract void ClearValuesInRange(CellRange range, ICompressedList collection);

		readonly CellBorders newValue;

		readonly CellRange cellRange;

		readonly CellRange leftBorderCellRange;

		readonly CellRange rightBorderCellRange;

		readonly CellRange topBorderCellRange;

		readonly CellRange bottomBorderCellRange;

		ICompressedList<CellBorder> newLeftBorderValues;

		ICompressedList<CellBorder> newRightBorderValues;

		ICompressedList<CellBorder> newTopBorderValues;

		ICompressedList<CellBorder> newBottomBorderValues;

		ICompressedList<CellBorder> newDiagonalUpBorderValues;

		ICompressedList<CellBorder> newDiagonalDownBorderValues;
	}
}
