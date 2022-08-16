using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ErfC : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return ErfC.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ErfC.Info;
			}
		}

		static ErfC()
		{
			string description = "Returns the complementary ERF function integrated between x and infinity.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ErfC_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("X", "The lower bound for integrating ERFC.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_X", "Spreadsheet_Functions_ErfC_XInfo")
			};
			ErfC.Info = new FunctionInfo(ErfC.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Functions.ErfC(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ERFC";

		static readonly FunctionInfo Info;
	}
}
