using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class And : BooleansInFunction
	{
		public override string Name
		{
			get
			{
				return And.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return And.Info;
			}
		}

		static And()
		{
			string description = "Returns TRUE if all its arguments are TRUE; returns FALSE if one or more argument is FALSE.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_And_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "Logical1, logical2, ...     are 1 to 30 conditions you want to test that can be either TRUE or FALSE.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_And_LogicalInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "Logical1, logical2, ...     are 1 to 30 conditions you want to test that can be either TRUE or FALSE.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_And_LogicalInfo")
			};
			And.Info = new FunctionInfo(And.FunctionName, FunctionCategory.Logical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<bool> context)
		{
			bool flag = true;
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				if (!context.Arguments[i])
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				return BooleanExpression.False;
			}
			return BooleanExpression.True;
		}

		public static readonly string FunctionName = "AND";

		static readonly FunctionInfo Info;
	}
}
