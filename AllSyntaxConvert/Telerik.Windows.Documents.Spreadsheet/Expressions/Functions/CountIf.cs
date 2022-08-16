using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class CountIf : FunctionWithArguments
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return CountIf.convertionRules;
			}
		}

		public override string Name
		{
			get
			{
				return CountIf.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return CountIf.Info;
			}
		}

		static CountIf()
		{
			string description = "The COUNTIF function counts the number of cells within a range that meet a single criteria that you specify. For example, you can count all the cells that start with a certain letter, or you can count all the cells that contain a number that is larger or smaller than a number you specify. For example, suppose you have a worksheet that contains a list of tasks in column A, and the first name of the person assigned to each task in column B. You can use the COUNTIF function to count how many times a person's name appears in column B and, in that way, determine how many tasks are assigned to that person. For example: =COUNTIF(B2:B25,\"Nancy\")";
			string descriptionLocalizationKey = "Spreadsheet_Functions_CountIf_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Range", "One or more cells to count, passed as reference argument.", ArgumentType.Reference, true, "Spreadsheet_Functions_Args_Range", "Spreadsheet_Functions_CountIf_Range"),
				new ArgumentInfo("Criteria", "A number, expression, cell reference, or text string that defines which cells will be counted. For example, criteria can be expressed as 32, \">32\", B4, \"apples\", or \"32\"", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Criteria", "Spreadsheet_Functions_CountIf_Criteria")
			};
			CountIf.Info = new FunctionInfo(CountIf.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression = (CellReferenceRangeExpression)context.Arguments[0];
			if (!cellReferenceRangeExpression.IsValid)
			{
				return ErrorExpressions.ReferenceError;
			}
			if (!cellReferenceRangeExpression.HasSingleRange)
			{
				return ErrorExpressions.ValueError;
			}
			ArrayExpression firstRangeArray = cellReferenceRangeExpression.FirstRangeArray;
			RadExpression criteriaExpression = context.Arguments[1] as RadExpression;
			CriteriaEvaluator criteriaEvaluator = new CriteriaEvaluator(criteriaExpression, cellReferenceRangeExpression.Worksheet);
			CellIndex cellIndex = new CellIndex(cellReferenceRangeExpression.CellReferenceRange.FromCellReference.ActualRowIndex, cellReferenceRangeExpression.CellReferenceRange.FromCellReference.ActualColumnIndex);
			double num = 0.0;
			for (int i = 0; i < firstRangeArray.RowCount; i++)
			{
				for (int j = 0; j < firstRangeArray.ColumnCount; j++)
				{
					if (criteriaEvaluator.Evaluate(firstRangeArray[i, j], i + cellIndex.RowIndex, j + cellIndex.ColumnIndex))
					{
						num += 1.0;
					}
				}
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "COUNTIF";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules convertionRules = new ArgumentConversionRules(ArgumentInterpretation.Ignore, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.Ignore, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArgumentInterpretation.UseAsIs, ArrayArgumentInterpretation.UseFirstElement);
	}
}
