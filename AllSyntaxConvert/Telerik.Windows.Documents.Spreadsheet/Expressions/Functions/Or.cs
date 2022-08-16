using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Or : BooleansInFunction
	{
		public override string Name
		{
			get
			{
				return Or.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Or.Info;
			}
		}

		static Or()
		{
			string description = "Returns TRUE if any argument is TRUE; returns FALSE if all arguments are FALSE.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Or_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "Logical1, logical2, ...     are 1 to 30 conditions you want to test that can be either TRUE or FALSE.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_Or_LogicalInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "Logical1, logical2, ...     are 1 to 30 conditions you want to test that can be either TRUE or FALSE.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_Or_LogicalInfo")
			};
			Or.Info = new FunctionInfo(Or.FunctionName, FunctionCategory.Logical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<bool> context)
		{
			bool flag = false;
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				if (context.Arguments[i])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return BooleanExpression.False;
			}
			return BooleanExpression.True;
		}

		public static readonly string FunctionName = "OR";

		static readonly FunctionInfo Info;
	}
}
