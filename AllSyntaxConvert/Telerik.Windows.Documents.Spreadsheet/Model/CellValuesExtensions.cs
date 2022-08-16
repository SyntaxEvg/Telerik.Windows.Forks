using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	static class CellValuesExtensions
	{
		public static int CompareTo(this ICellValue value1, ICellValue value2)
		{
			FormulaCellValue formulaCellValue = value1 as FormulaCellValue;
			if (formulaCellValue != null)
			{
				return formulaCellValue.CompareTo(value2);
			}
			NumberCellValue numberCellValue = value1 as NumberCellValue;
			if (numberCellValue != null)
			{
				return numberCellValue.CompareTo(value2);
			}
			TextCellValue textCellValue = value1 as TextCellValue;
			if (textCellValue != null)
			{
				return textCellValue.CompareTo(value2);
			}
			BooleanCellValue booleanCellValue = value1 as BooleanCellValue;
			if (booleanCellValue != null)
			{
				return booleanCellValue.CompareTo(value2);
			}
			EmptyCellValue emptyCellValue = value1 as EmptyCellValue;
			if (emptyCellValue != null)
			{
				return emptyCellValue.CompareTo(value2);
			}
			ErrorCellValue errorCellValue = value1 as ErrorCellValue;
			if (errorCellValue != null)
			{
				return errorCellValue.CompareTo(value2);
			}
			throw new InvalidOperationException();
		}

		public static int CompareTo(this FormulaCellValue value1, ICellValue value2)
		{
			return value1.GetResultValueAsCellValue().CompareTo(value2);
		}

		public static int CompareTo(this NumberCellValue value1, ICellValue value2)
		{
			FormulaCellValue formulaCellValue = value2 as FormulaCellValue;
			if (formulaCellValue != null)
			{
				return -1 * formulaCellValue.CompareTo(value1);
			}
			NumberCellValue numberCellValue = value2 as NumberCellValue;
			if (numberCellValue != null)
			{
				return value1.CompareTo(numberCellValue);
			}
			if (value2 is TextCellValue)
			{
				return CellValuesExtensions.NumberCellValueComparedToTextCellValue;
			}
			if (value2 is BooleanCellValue)
			{
				return CellValuesExtensions.NumberCellValueComparedToBooleanCellValue;
			}
			if (value2 is EmptyCellValue)
			{
				return CellValuesExtensions.NumberCellValueComparedToEmptyCellValue;
			}
			if (value2 is ErrorCellValue)
			{
				return CellValuesExtensions.NumberCellValueComparedToErrorCellValue;
			}
			throw new InvalidOperationException();
		}

		public static int CompareTo(this TextCellValue value1, ICellValue value2)
		{
			FormulaCellValue formulaCellValue = value2 as FormulaCellValue;
			if (formulaCellValue != null)
			{
				return -1 * formulaCellValue.CompareTo(value1);
			}
			if (value2 is NumberCellValue)
			{
				return -1 * CellValuesExtensions.NumberCellValueComparedToTextCellValue;
			}
			TextCellValue textCellValue = value2 as TextCellValue;
			if (textCellValue != null)
			{
				return value1.CompareTo(textCellValue);
			}
			if (value2 is BooleanCellValue)
			{
				return CellValuesExtensions.TextCellValueComparedToBooleanCellValue;
			}
			if (value2 is EmptyCellValue)
			{
				return CellValuesExtensions.TextCellValueComparedToEmptyCellValue;
			}
			if (value2 is ErrorCellValue)
			{
				return CellValuesExtensions.TextCellValueComparedToErrorCellValue;
			}
			throw new InvalidOperationException();
		}

		public static int CompareTo(this BooleanCellValue value1, ICellValue value2)
		{
			FormulaCellValue formulaCellValue = value2 as FormulaCellValue;
			if (formulaCellValue != null)
			{
				return -1 * formulaCellValue.CompareTo(value1);
			}
			if (value2 is NumberCellValue)
			{
				return -1 * CellValuesExtensions.NumberCellValueComparedToBooleanCellValue;
			}
			if (value2 is TextCellValue)
			{
				return -1 * CellValuesExtensions.TextCellValueComparedToBooleanCellValue;
			}
			BooleanCellValue booleanCellValue = value2 as BooleanCellValue;
			if (booleanCellValue != null)
			{
				return value1.CompareTo(booleanCellValue);
			}
			if (value2 is EmptyCellValue)
			{
				return CellValuesExtensions.BooleanCellValueComparedToEmptyCellValue;
			}
			if (value2 is ErrorCellValue)
			{
				return CellValuesExtensions.BooleanCellValueComparedToErrorCellValue;
			}
			throw new InvalidOperationException();
		}

		public static int CompareTo(this EmptyCellValue value1, ICellValue value2)
		{
			FormulaCellValue formulaCellValue = value2 as FormulaCellValue;
			if (formulaCellValue != null)
			{
				return -1 * formulaCellValue.CompareTo(value1);
			}
			if (value2 is NumberCellValue)
			{
				return -1 * CellValuesExtensions.NumberCellValueComparedToEmptyCellValue;
			}
			if (value2 is TextCellValue)
			{
				return -1 * CellValuesExtensions.TextCellValueComparedToEmptyCellValue;
			}
			if (value2 is BooleanCellValue)
			{
				return -1 * CellValuesExtensions.BooleanCellValueComparedToEmptyCellValue;
			}
			if (value2 is EmptyCellValue)
			{
				return CellValuesExtensions.EmptyCellValueComparedToEmptyCellValue;
			}
			if (value2 is ErrorCellValue)
			{
				return CellValuesExtensions.EmptyCellValueComparedToErrorCellValue;
			}
			throw new InvalidOperationException();
		}

		public static int CompareTo(this ErrorCellValue value1, ICellValue value2)
		{
			FormulaCellValue formulaCellValue = value2 as FormulaCellValue;
			if (formulaCellValue != null)
			{
				return -1 * formulaCellValue.CompareTo(value1);
			}
			if (value2 is NumberCellValue)
			{
				return -1 * CellValuesExtensions.NumberCellValueComparedToErrorCellValue;
			}
			if (value2 is TextCellValue)
			{
				return -1 * CellValuesExtensions.TextCellValueComparedToErrorCellValue;
			}
			if (value2 is BooleanCellValue)
			{
				return -1 * CellValuesExtensions.BooleanCellValueComparedToErrorCellValue;
			}
			if (value2 is EmptyCellValue)
			{
				return -1 * CellValuesExtensions.EmptyCellValueComparedToErrorCellValue;
			}
			if (value2 is ErrorCellValue)
			{
				return CellValuesExtensions.ErrorCellValueComparedToErrorCellValue;
			}
			throw new InvalidOperationException();
		}

		public static int CompareTo(this NumberCellValue value1, NumberCellValue value2)
		{
			return value1.Value.CompareTo(value2.Value);
		}

		public static int CompareTo(this TextCellValue value1, TextCellValue value2)
		{
			return value1.Value.CompareTo(value2.Value);
		}

		public static int CompareTo(this BooleanCellValue value1, BooleanCellValue value2)
		{
			return value1.Value.CompareTo(value2.Value);
		}

		static int NumberCellValueComparedToTextCellValue = -1;

		static int NumberCellValueComparedToBooleanCellValue = -1;

		static int NumberCellValueComparedToEmptyCellValue = -1;

		static int NumberCellValueComparedToErrorCellValue = -1;

		static int TextCellValueComparedToBooleanCellValue = -1;

		static int TextCellValueComparedToEmptyCellValue = -1;

		static int TextCellValueComparedToErrorCellValue = -1;

		static int BooleanCellValueComparedToEmptyCellValue = -1;

		static int BooleanCellValueComparedToErrorCellValue = -1;

		static int EmptyCellValueComparedToEmptyCellValue = 0;

		static int EmptyCellValueComparedToErrorCellValue = 1;

		static int ErrorCellValueComparedToErrorCellValue = 0;
	}
}
