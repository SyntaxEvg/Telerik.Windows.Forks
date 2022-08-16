using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class BesselJ : NumbersInFunction
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
				return BesselJ.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return BesselJ.Info;
			}
		}

		static BesselJ()
		{
			string description = "Returns the Bessel function.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_BesselJ_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("X", "The value at which to evaluate the function.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_X", "Spreadsheet_Functions_BesselJ_XInfo"),
				new ArgumentInfo("N", "The order of the Bessel function. If n is not an integer, it is truncated. Must be >= 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_N", "Spreadsheet_Functions_BesselJ_NInfo")
			};
			BesselJ.Info = new FunctionInfo(BesselJ.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double x = context.Arguments[0];
			double num = context.Arguments[1];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double num2 = EngineeringFunctions.BESSELJ(x, num);
			if (double.IsNaN(num2))
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num2);
		}

		public static readonly string FunctionName = "BESSELJ";

		static readonly FunctionInfo Info;
	}
}
