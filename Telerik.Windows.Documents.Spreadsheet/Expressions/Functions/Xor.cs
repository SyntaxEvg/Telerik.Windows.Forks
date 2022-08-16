using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Xor : BooleansInFunction
	{
		public override string Name
		{
			get
			{
				return Xor.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Xor.Info;
			}
		}

		static Xor()
		{
			string description = "The result of XOR is TRUE when the number of TRUE inputs is odd and FALSE when the number of TRUE inputs is even.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Xor_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "Logical1 is required, subsequent logical values are optional. 1 to 254 conditions you want to test that can be either TRUE or FALSE, and can be logical values, arrays, or references.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_Xor_LogicalInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Logical", "Logical1 is required, subsequent logical values are optional. 1 to 254 conditions you want to test that can be either TRUE or FALSE, and can be logical values, arrays, or references.", ArgumentType.Logical, true, "Spreadsheet_Functions_Args_Logical", "Spreadsheet_Functions_Xor_LogicalInfo")
			};
			Xor.Info = new FunctionInfo(Xor.FunctionName, FunctionCategory.Logical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<bool> context)
		{
			int num = 0;
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				if (context.Arguments[i])
				{
					num++;
				}
			}
			if (num % 2 <= 0)
			{
				return BooleanExpression.False;
			}
			return BooleanExpression.True;
		}

		public static readonly string FunctionName = "XOR";

		static readonly FunctionInfo Info;
	}
}
