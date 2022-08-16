using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsFormula : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return IsFormula.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsFormula.Info;
			}
		}

		static IsFormula()
		{
			string description = "Checks whether there is a reference to a cell that contains a formula, and returns TRUE or FALSE.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsFormula_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Reference", "Reference is a reference to the cell you want to test. Reference can be a cell reference, a formula, or a name that refers to a cell.", ArgumentType.Reference, true, "Spreadsheet_Functions_Args_Reference", "Spreadsheet_Functions_IsFormula_ReferenceInfo")
			};
			IsFormula.Info = new FunctionInfo(IsFormula.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression = context.Arguments[0] as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression != null && cellReferenceRangeExpression.IsValid)
			{
				int actualRowIndex = cellReferenceRangeExpression.CellReferenceRange.FromCellReference.ActualRowIndex;
				int actualColumnIndex = cellReferenceRangeExpression.CellReferenceRange.FromCellReference.ActualColumnIndex;
				FormulaCellValue formulaCellValue = cellReferenceRangeExpression.Worksheet.Cells[actualRowIndex, actualColumnIndex].GetValue().Value as FormulaCellValue;
				if (formulaCellValue != null)
				{
					return BooleanExpression.True;
				}
			}
			return BooleanExpression.False;
		}

		public static readonly string FunctionName = "ISFORMULA";

		static readonly FunctionInfo Info;
	}
}
