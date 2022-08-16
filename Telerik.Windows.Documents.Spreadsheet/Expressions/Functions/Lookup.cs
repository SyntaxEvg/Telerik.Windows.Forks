using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Lookup : LookupFunctionBase
	{
		public override string Name
		{
			get
			{
				return Lookup.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Lookup.Info;
			}
		}

		static Lookup()
		{
			string description = "The LOOKUP function returns a value either from a one-row or one-column range or from an array. The LOOKUP function has two syntax forms: the vector form and the array form.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Lookup_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Lookup_value", "A value that LOOKUP searches for in the first vector. Lookup_value can be a number, text, a logical value, or a name or reference that refers to a value.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_LookupValue", "Spreadsheet_Functions_Lookup_LookupValue"),
				new ArgumentInfo("Lookup_vector", "A range that contains only one row or one column. The values in lookup_vector can be text, numbers, or logical values.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_LookupVector", "Spreadsheet_Functions_Lookup_LookupVector")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Result_vector", "A range that contains only one row or column. The result_vector argument must be the same size as lookup_vector.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_ResultVector", "Spreadsheet_Functions_Lookup_ResultVector")
			};
			Lookup.Info = new FunctionInfo(Lookup.FunctionName, FunctionCategory.LookupReference, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		internal override ErrorExpression TryGetResultVector(ArrayExpression lookupMatrix, FunctionEvaluationContext<object> context, out ArrayExpression resultVector)
		{
			resultVector = null;
			if (context.Arguments.Length == 3)
			{
				int maxVectorDimension = Math.Max(lookupMatrix.ColumnCount, lookupMatrix.RowCount);
				ErrorExpression errorExpression = Lookup.TryConstructResultVector(context.Arguments[2], maxVectorDimension, out resultVector);
				if (errorExpression != null)
				{
					return errorExpression;
				}
			}
			else
			{
				resultVector = Lookup.GetRowOrColumnVectorFromArray(lookupMatrix, false);
			}
			return null;
		}

		internal override ErrorExpression TryGetLookupVector(ArrayExpression lookupMatrix, out ArrayExpression lookupVector)
		{
			lookupVector = Lookup.GetRowOrColumnVectorFromArray(lookupMatrix, true);
			return null;
		}

		internal override bool GetSearchForApproximateMatch(FunctionEvaluationContext<object> context)
		{
			return true;
		}

		static ArrayExpression GetRowOrColumnVectorFromArray(ArrayExpression array, bool getVectorFromArrayStart)
		{
			int rowCount = array.RowCount;
			int columnCount = array.ColumnCount;
			ArrayExpression result;
			if (rowCount >= columnCount)
			{
				int columnIndex = (getVectorFromArrayStart ? 0 : (columnCount - 1));
				result = array.GetColumnAsArrayExpression(columnIndex);
			}
			else
			{
				int rowIndex = (getVectorFromArrayStart ? 0 : (rowCount - 1));
				result = array.GetRowAsArrayExpression(rowIndex);
			}
			return result;
		}

		static ErrorExpression TryConstructResultVector(object argument, int maxVectorDimension, out ArrayExpression resultVector)
		{
			resultVector = null;
			ArrayExpression arrayExpression = FunctionHelper.ConvertToArrayExpression(argument as ConstantExpression);
			CellReferenceRangeExpression cellReferenceRangeExpression = argument as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression != null)
			{
				if (!cellReferenceRangeExpression.IsValid)
				{
					return ErrorExpressions.ReferenceError;
				}
				if (!cellReferenceRangeExpression.HasSingleRange)
				{
					return ErrorExpressions.NotAvailableError;
				}
				cellReferenceRangeExpression = Lookup.GetRangeWithSpecificDimensions(maxVectorDimension, cellReferenceRangeExpression);
				arrayExpression = cellReferenceRangeExpression.FirstRangeArray;
			}
			if (arrayExpression == null)
			{
				return ErrorExpressions.ValueError;
			}
			if (arrayExpression.RowCount > 1 && arrayExpression.ColumnCount > 1)
			{
				return ErrorExpressions.NotAvailableError;
			}
			resultVector = arrayExpression;
			return null;
		}

		static CellReferenceRangeExpression GetRangeWithSpecificDimensions(int maxVectorDimension, CellReferenceRangeExpression referenceExpression)
		{
			int columnsCount = referenceExpression.CellReferenceRange.ColumnsCount;
			int rowsCount = referenceExpression.CellReferenceRange.RowsCount;
			if (rowsCount == 1)
			{
				return CellReferenceRangeExpression.CreateRangeWithSpecificDimensions(referenceExpression, 1, maxVectorDimension);
			}
			if (columnsCount == 1)
			{
				return CellReferenceRangeExpression.CreateRangeWithSpecificDimensions(referenceExpression, maxVectorDimension, 1);
			}
			return referenceExpression;
		}

		public static readonly string FunctionName = "LOOKUP";

		static readonly FunctionInfo Info;
	}
}
