using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class GEStep : NumbersInFunction
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
				return GEStep.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return GEStep.Info;
			}
		}

		static GEStep()
		{
			string description = "Returns 1 if number ≥ step; returns 0 (zero) otherwise. Use this function to filter a set of values. For example, by summing several GESTEP functions you calculate the count of values that exceed a threshold.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_GEStep_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The value to test against step.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_GEStep_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Step", "The threshold value. If you omit a value for step, GESTEP uses zero.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Step", "Spreadsheet_Functions_GEStep_StepInfo")
			};
			GEStep.Info = new FunctionInfo(GEStep.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = (double)((context.Arguments.Length == 1) ? EngineeringFunctions.GESTEP(context.Arguments[0]) : EngineeringFunctions.GESTEP(context.Arguments[0], context.Arguments[1]));
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "GESTEP";

		static readonly FunctionInfo Info;
	}
}
