using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class SumIf : FunctionWithArguments
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return SumIf.convertionRules;
			}
		}

		public override string Name
		{
			get
			{
				return SumIf.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return SumIf.Info;
			}
		}

		static SumIf()
		{
			string description = "You use the SUMIF function to sum the values in a range that meet criteria that you specify. For example, suppose that in a column that contains numbers, you want to sum only the values that are larger than 5. You can use the following formula: =SUMIF(B2:B25,\">5\"). In this example, the criteria is applied the same values that are being summed. If you want, you can apply the criteria to one range and sum the corresponding values in a different range. For example, the formula =SUMIF(B2:B5, \"John\", C2:C5) sums only the values in the range C2:C5, where the corresponding cells in the range B2:B5 equal \"John.\"";
			string descriptionLocalizationKey = "Spreadsheet_Functions_SumIf_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Range", "The range of cells that you want evaluated by criteria. Cells in each range must be numbers or names, arrays, or references that contain numbers. Blank and text values are ignored.", ArgumentType.Reference, true, "Spreadsheet_Functions_Args_Range", "Spreadsheet_Functions_SumIf_Range"),
				new ArgumentInfo("Criteria", "The criteria in the form of a number, expression, a cell reference, text, or a function that defines which cells will be added. For example, criteria can be expressed as 32, \">32\", B5, 32, \"32\", \"apples\", or TODAY().", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Criteria", "Spreadsheet_Functions_SumIf_Criteria")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Sum_range", "The actual cells to add, if you want to add cells other than those specified in the range argument. If the sum_range argument is omitted, RadSpreadsheet adds the cells that are specified in the range argument (the same cells to which the criteria is applied).", ArgumentType.Reference, true, "Spreadsheet_Functions_Args_SumRange", "Spreadsheet_Functions_SumIf_SumRange")
			};
			SumIf.Info = new FunctionInfo(SumIf.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression;
			CellReferenceRangeExpression cellReferenceRangeExpression2;
			if (!SumIf.TryGetValidReferenceRangeExpressions(context.Arguments, out cellReferenceRangeExpression, out cellReferenceRangeExpression2))
			{
				return ErrorExpressions.ReferenceError;
			}
			bool hasSingleRange = cellReferenceRangeExpression2.HasSingleRange;
			cellReferenceRangeExpression2 = CellReferenceRangeExpression.CreateRangeWithSpecificDimensions(cellReferenceRangeExpression2, cellReferenceRangeExpression.CellReferenceRange.RowsCount, cellReferenceRangeExpression.CellReferenceRange.ColumnsCount);
			ArrayExpression arrayExpression;
			ArrayExpression arrayExpression2;
			if (!hasSingleRange || !SumIf.TryGetArrayValues(cellReferenceRangeExpression, out arrayExpression) || !SumIf.TryGetArrayValues(cellReferenceRangeExpression2, out arrayExpression2))
			{
				return ErrorExpressions.ValueError;
			}
			RadExpression criteriaExpression = context.Arguments[1] as RadExpression;
			CriteriaEvaluator criteriaEvaluator = new CriteriaEvaluator(criteriaExpression, cellReferenceRangeExpression.Worksheet);
			CellIndex cellIndex = new CellIndex(cellReferenceRangeExpression.CellReferenceRange.FromCellReference.ActualRowIndex, cellReferenceRangeExpression.CellReferenceRange.FromCellReference.ActualColumnIndex);
			double num = 0.0;
			for (int i = 0; i < arrayExpression2.RowCount; i++)
			{
				for (int j = 0; j < arrayExpression2.ColumnCount; j++)
				{
					if (criteriaEvaluator.Evaluate(arrayExpression[i, j], i + cellIndex.RowIndex, j + cellIndex.ColumnIndex))
					{
						NumberExpression numberExpression = arrayExpression2[i, j] as NumberExpression;
						if (numberExpression != null)
						{
							num += numberExpression.Value;
						}
					}
				}
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		static bool TryGetValidReferenceRangeExpressions(object[] arguments, out CellReferenceRangeExpression criteriaRangeExpression, out CellReferenceRangeExpression sumRangeExpression)
		{
			criteriaRangeExpression = (CellReferenceRangeExpression)arguments[0];
			if (arguments.Length == 3)
			{
				sumRangeExpression = (CellReferenceRangeExpression)arguments[2];
			}
			else
			{
				sumRangeExpression = criteriaRangeExpression;
			}
			return criteriaRangeExpression.IsValid && sumRangeExpression.IsValid;
		}

		static bool TryGetArrayValues(CellReferenceRangeExpression referenceRangeExpression, out ArrayExpression arrayValues)
		{
			arrayValues = null;
			if (!referenceRangeExpression.HasSingleRange)
			{
				return false;
			}
			arrayValues = referenceRangeExpression.FirstRangeArray;
			return true;
		}

		public static readonly string FunctionName = "SUMIF";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules convertionRules = new ArgumentConversionRules(ArgumentInterpretation.Ignore, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.Ignore, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArrayArgumentInterpretation.UseFirstElement);
	}
}
