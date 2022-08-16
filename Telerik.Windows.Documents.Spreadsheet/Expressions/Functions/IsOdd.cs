using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsOdd : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return IsOdd.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsOdd.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNumberFunctionConversion;
			}
		}

		static IsOdd()
		{
			string description = "Returns TRUE if number is odd, or FALSE if number is even.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsOdd_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Required. The value to test. If number is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_IsOdd_NumberInfo")
			};
			IsOdd.Info = new FunctionInfo(IsOdd.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			bool value = context.Arguments[0] % 2.0 != 0.0;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISODD";

		static readonly FunctionInfo Info;
	}
}
