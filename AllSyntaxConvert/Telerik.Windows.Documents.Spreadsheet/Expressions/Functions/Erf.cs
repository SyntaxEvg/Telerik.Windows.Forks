using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Erf : NumbersInFunction
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
				return Erf.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Erf.Info;
			}
		}

		static Erf()
		{
			string description = "Returns the error function integrated between lower_limit and upper_limit.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Erf_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("lower_limit", "The lower bound for integrating ERF.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_LowerLimit", "Spreadsheet_Functions_Erf_LowerLimitInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("upper_limit", "The upper bound for integrating ERF. If omitted, ERF integrates between zero and lower_limit.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_UpperLimit", "Spreadsheet_Functions_Erf_UpperLimitInfo")
			};
			Erf.Info = new FunctionInfo(Erf.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = ((context.Arguments.Length == 1) ? EngineeringFunctions.ERF(context.Arguments[0]) : EngineeringFunctions.ERF(context.Arguments[0], context.Arguments[1]));
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ERF";

		static readonly FunctionInfo Info;
	}
}
