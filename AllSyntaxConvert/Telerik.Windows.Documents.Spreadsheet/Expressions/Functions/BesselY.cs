using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class BesselY : NumbersInFunction
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
				return BesselY.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return BesselY.Info;
			}
		}

		static BesselY()
		{
			string description = "Returns the Bessel function, which is also called the Weber function or the Neumann function.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_BesselY_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("X", "The value at which to evaluate the function.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_X", "Spreadsheet_Functions_BesselY_XInfo"),
				new ArgumentInfo("N", "The order of the function. If n is not an integer, it is truncated. Must be >= 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_N", "Spreadsheet_Functions_BesselY_NInfo")
			};
			BesselY.Info = new FunctionInfo(BesselY.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double x = context.Arguments[0];
			double num = context.Arguments[1];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double num2 = EngineeringFunctions.BESSELY(x, num);
			if (double.IsNaN(num2))
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num2);
		}

		public static readonly string FunctionName = "BESSELY";

		static readonly FunctionInfo Info;
	}
}
