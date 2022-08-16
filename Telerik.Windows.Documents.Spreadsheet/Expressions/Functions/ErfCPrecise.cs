using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ErfCPrecise : NumbersInFunction
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
				return ErfCPrecise.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ErfCPrecise.Info;
			}
		}

		static ErfCPrecise()
		{
			string description = "Returns the complementary ERFC.PRECISE function integrated between x and infinity.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ErfCPrecise_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("X", "The lower bound for integrating ERFC.PRECISE.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_X", "Spreadsheet_Functions_ErfCPrecise_XInfo")
			};
			ErfCPrecise.Info = new FunctionInfo(ErfCPrecise.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Functions.ErfC(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ERFC.PRECISE";

		static readonly FunctionInfo Info;
	}
}
