using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsEven : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return IsEven.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsEven.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNumberFunctionConversion;
			}
		}

		static IsEven()
		{
			string description = "Returns TRUE if number is even, or FALSE if number is odd.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsEven_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Required. The value to test. If number is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_IsEven_NumberInfo")
			};
			IsEven.Info = new FunctionInfo(IsEven.FunctionName, FunctionCategory.Information, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			bool value = context.Arguments[0] % 2.0 == 0.0;
			return value.ToBooleanExpression();
		}

		public static readonly string FunctionName = "ISEVEN";

		static readonly FunctionInfo Info;
	}
}
