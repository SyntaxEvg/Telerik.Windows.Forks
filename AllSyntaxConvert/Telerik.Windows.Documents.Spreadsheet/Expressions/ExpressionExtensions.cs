using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public static class ExpressionExtensions
	{
		public static double NumberValue(this RadExpression expression)
		{
			NumberExpression numberExpression = expression as NumberExpression;
			if (numberExpression != null)
			{
				return numberExpression.Value;
			}
			return 0.0;
		}

		public static ConstantExpression GetValueAsConstantExpression(this RadExpression expression)
		{
			return expression.GetValueAsNonArrayConstantExpression(true);
		}

		internal static ConstantExpression GetValueAsNonArrayConstantExpression(this RadExpression expression, bool popupArrayErrors)
		{
			ArrayExpression arrayExpression = expression as ArrayExpression;
			if (arrayExpression != null)
			{
				if (popupArrayErrors)
				{
					ErrorExpression containingError = arrayExpression.GetContainingError();
					if (containingError != null)
					{
						return containingError;
					}
				}
				return arrayExpression[0, 0].GetValueAsNonArrayConstantExpression(false);
			}
			ConstantExpression constantExpression = expression as ConstantExpression;
			if (constantExpression != null)
			{
				return constantExpression;
			}
			RadExpression value = expression.GetValue();
			return value.GetValueAsNonArrayConstantExpression(popupArrayErrors);
		}

		internal static RadExpression GetValueAsConstantOrCellReference(this RadExpression expression)
		{
			if (expression is ConstantExpression || expression is CellReferenceRangeExpression)
			{
				return expression;
			}
			RadExpression value = expression.GetValue();
			return value.GetValueAsConstantOrCellReference();
		}

		internal static ConstantExpression GetValueAsConstant(this RadExpression expression)
		{
			ConstantExpression constantExpression = expression as ConstantExpression;
			if (constantExpression != null)
			{
				return constantExpression;
			}
			RadExpression value = expression.GetValue();
			return value.GetValueAsConstant();
		}

		public static int IntegerValue(this RadExpression expression)
		{
			NumberExpression numberExpression = expression as NumberExpression;
			Guard.ThrowExceptionIfNull<NumberExpression>(numberExpression, "numberExpression");
			return numberExpression.IntValue;
		}

		public static bool IsValidFunctionOrCellReferenceNamePart(this char character)
		{
			return char.IsLetterOrDigit(character) || ExpressionExtensions.AllowedCharsInFunctionOrCellReferenceName.Contains(character);
		}

		public static BooleanExpression ToBooleanExpression(this bool value)
		{
			if (!value)
			{
				return BooleanExpression.False;
			}
			return BooleanExpression.True;
		}

		public static CellRange ToCellRange(this CellReferenceRange cellReferenceRange)
		{
			return new CellRange(cellReferenceRange.FromCellReference.ActualRowIndex, cellReferenceRange.FromCellReference.ActualColumnIndex, cellReferenceRange.ToCellReference.ActualRowIndex, cellReferenceRange.ToCellReference.ActualColumnIndex);
		}

		static readonly HashSet<char> AllowedCharsInFunctionOrCellReferenceName = new HashSet<char> { '.', '_', '!', '$', '[', ']' };
	}
}
