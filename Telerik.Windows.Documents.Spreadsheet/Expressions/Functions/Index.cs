using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Index : FunctionWithArguments
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return Index.convertionRules;
			}
		}

		public override string Name
		{
			get
			{
				return Index.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Index.Info;
			}
		}

		static Index()
		{
			string description = "Returns the reference of the cell at the intersection of a particular row and column. If the reference is made up of nonadjacent selections, you can pick the selection to look in.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Index_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Reference", "A reference to one or more cell ranges. If you are entering a nonadjacent range for the reference, enclose reference in parentheses. If each area in reference contains only one row or column, the Row_num or Column_num argument, respectively, is optional. For example, for a single row reference, use INDEX(reference,,column_num).", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Reference", "Spreadsheet_Functions_Index_Reference"),
				new ArgumentInfo("Row_num", "The number of the row in reference from which to return a reference.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_RowNum", "Spreadsheet_Functions_Index_RowNum")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Column_num", "The number of the column in reference from which to return a reference.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_ColumnNum", "Spreadsheet_Functions_Index_ColumnNum"),
				new ArgumentInfo("Area_num", "Selects a range in reference from which to return the intersection of Row_num and Column_num. The first area selected or entered is numbered 1, the second is 2, and so on. If Area_num is omitted, INDEX uses area 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_AreaNum", "Spreadsheet_Functions_Index_AreaNum")
			};
			Index.Info = new FunctionInfo(Index.FunctionName, FunctionCategory.LookupReference, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			int rowNum;
			int columnNum;
			int num;
			if (!Index.TryGetNonNegativeRowColumnAreaNumsArguments(context.Arguments, out rowNum, out columnNum, out num))
			{
				return ErrorExpressions.ValueError;
			}
			ArrayExpression arrayExpression;
			if (Index.TryGetArrayArgument(context.Arguments[0], out arrayExpression))
			{
				if (num > 1)
				{
					return ErrorExpressions.ReferenceError;
				}
				return Index.HandleArrayOverload(arrayExpression, rowNum, columnNum);
			}
			else
			{
				CellReferenceRangeExpression referenceRangeExpression;
				if (!Index.TryGetReferenceArgument(context.Arguments[0], out referenceRangeExpression))
				{
					return ErrorExpressions.ValueError;
				}
				CellReferenceRange referenceRange;
				ErrorExpression errorExpression = Index.TryGetCellReferenceRange(referenceRangeExpression, num, out referenceRange);
				if (errorExpression != null)
				{
					return errorExpression;
				}
				bool hasOptionalArguments = context.Arguments.Length > 2;
				errorExpression = Index.TryRecalculateZeroRowColumnArguments(referenceRange, hasOptionalArguments, ref rowNum, ref columnNum);
				if (errorExpression != null)
				{
					return errorExpression;
				}
				return Index.HandleReferenceRangeOverload(referenceRange, rowNum, columnNum);
			}
		}

		static ErrorExpression TryGetCellReferenceRange(CellReferenceRangeExpression referenceRangeExpression, int areaNum, out CellReferenceRange referenceRange)
		{
			referenceRange = null;
			Guard.ThrowExceptionIfLessThan<int>(1, areaNum, "areaNum");
			if (!referenceRangeExpression.IsInRange)
			{
				return ErrorExpressions.ReferenceError;
			}
			if (!referenceRangeExpression.IsValid)
			{
				return ErrorExpressions.ValueError;
			}
			if (areaNum > referenceRangeExpression.CellReferenceRanges.Count)
			{
				return ErrorExpressions.ReferenceError;
			}
			referenceRange = referenceRangeExpression.CellReferenceRanges[areaNum - 1];
			return null;
		}

		static ErrorExpression TryRecalculateZeroRowColumnArguments(CellReferenceRange referenceRange, bool hasOptionalArguments, ref int rowNum, ref int columnNum)
		{
			if (referenceRange.ColumnsCount <= 1 || referenceRange.RowsCount <= 1)
			{
				if (columnNum == 0)
				{
					if (referenceRange.ColumnsCount != 1 && hasOptionalArguments)
					{
						return ErrorExpressions.ValueError;
					}
					columnNum = 1;
				}
				if (rowNum == 0)
				{
					if (referenceRange.RowsCount != 1)
					{
						return ErrorExpressions.ValueError;
					}
					rowNum = 1;
				}
				return null;
			}
			if (rowNum != 0 && columnNum != 0)
			{
				return null;
			}
			if (!hasOptionalArguments)
			{
				return ErrorExpressions.ReferenceError;
			}
			return ErrorExpressions.ValueError;
		}

		static RadExpression HandleArrayOverload(ArrayExpression arrayExpression, int rowNum, int columnNum)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, rowNum, "rowNum");
			Guard.ThrowExceptionIfLessThan<int>(0, columnNum, "columnNum");
			if (rowNum > 0 && columnNum > 0)
			{
				if (rowNum <= arrayExpression.RowCount && columnNum <= arrayExpression.ColumnCount)
				{
					return arrayExpression[rowNum - 1, columnNum - 1];
				}
			}
			else if (rowNum > 0 && columnNum == 0)
			{
				if (rowNum <= arrayExpression.RowCount)
				{
					return arrayExpression.GetRowAsArrayExpression(rowNum - 1);
				}
			}
			else if (columnNum > 0 && rowNum == 0)
			{
				if (columnNum <= arrayExpression.ColumnCount)
				{
					return arrayExpression.GetColumnAsArrayExpression(columnNum - 1);
				}
			}
			else if (rowNum == 0 && columnNum == 0)
			{
				return arrayExpression;
			}
			return ErrorExpressions.ReferenceError;
		}

		static RadExpression HandleReferenceRangeOverload(CellReferenceRange referenceRange, int rowNum, int columnNum)
		{
			Guard.ThrowExceptionIfLessThan<int>(1, rowNum, "rowNum");
			Guard.ThrowExceptionIfLessThan<int>(1, columnNum, "columnNum");
			int actualRowIndex = referenceRange.FromCellReference.ActualRowIndex;
			int actualColumnIndex = referenceRange.FromCellReference.ActualColumnIndex;
			int targetIndex = actualRowIndex + rowNum - 1;
			int targetIndex2 = actualColumnIndex + columnNum - 1;
			if (rowNum <= referenceRange.RowsCount && columnNum <= referenceRange.ColumnsCount)
			{
				CellAreaReference cellReferenceRange = new CellAreaReference(new CellReference(RowColumnReference.CreateAbsoluteRowColumnReference(targetIndex), RowColumnReference.CreateAbsoluteRowColumnReference(targetIndex2)));
				return new CellReferenceRangeExpression(referenceRange.Worksheet.Workbook, referenceRange.Worksheet.Name, true, cellReferenceRange);
			}
			return ErrorExpressions.ReferenceError;
		}

		static bool TryGetReferenceArgument(object argument, out CellReferenceRangeExpression referenceRangeExpression)
		{
			referenceRangeExpression = argument as CellReferenceRangeExpression;
			return referenceRangeExpression != null;
		}

		static bool TryGetArrayArgument(object argument, out ArrayExpression arrayExpression)
		{
			arrayExpression = FunctionHelper.ConvertToArrayExpression(argument as ConstantExpression);
			return arrayExpression != null;
		}

		static bool TryGetNonNegativeRowColumnAreaNumsArguments(object[] arguments, out int rowNum, out int columnNum, out int areaNum)
		{
			rowNum = (int)Math.Floor((double)arguments[1]);
			columnNum = 0;
			areaNum = 1;
			if (arguments.Length > 2)
			{
				columnNum = (int)Math.Floor((double)arguments[2]);
				if (arguments.Length == 4)
				{
					areaNum = (int)Math.Floor((double)arguments[3]);
				}
			}
			return rowNum >= 0 && columnNum >= 0 && areaNum > 0;
		}

		public static readonly string FunctionName = "INDEX";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules convertionRules = new ArgumentConversionRules(ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.ConvertToDefault, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArrayArgumentInterpretation.UseFirstElement);
	}
}
