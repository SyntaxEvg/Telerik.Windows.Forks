using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.LookupAndReference
{
	public class Choose : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Choose.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Choose.Info;
			}
		}

		static Choose()
		{
			string description = "Uses the number parameter to return a value from the list of value arguments.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Choose_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Index", "Specifies which value argument is selected. Must be a number between 1 and 254, or a formula or reference to a cell containing a number between 1 and 254.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Index", "Spreadsheet_Functions_Choose_IndexInfo"),
				new ArgumentInfo("Value", "1 to 254 value arguments from which CHOOSE selects a value or an action to perform based on index_num. The arguments can be numbers, cell references, defined names, formulas, functions, or text.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Choose_ValueInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value", "Value 1 to 254 value arguments from which CHOOSE selects a value or an action to perform based on index_num. The arguments can be numbers, cell references, defined names, formulas, functions, or text.", ArgumentType.Any, true, "Spreadsheet_Functions_Args_Value", "Spreadsheet_Functions_Choose_ValueInfo")
			};
			Choose.Info = new FunctionInfo(Choose.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			object[] arguments = context.Arguments;
			int num = Convert.ToInt32(arguments[0]);
			RadExpression result;
			try
			{
				result = (RadExpression)arguments[num];
			}
			catch (IndexOutOfRangeException)
			{
				return ErrorExpressions.ValueError;
			}
			return result;
		}

		public static readonly string FunctionName = "CHOOSE";

		static readonly FunctionInfo Info;
	}
}
