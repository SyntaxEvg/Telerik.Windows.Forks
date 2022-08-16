using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class MRound : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return MRound.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return MRound.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNumberFunctionConversion;
			}
		}

		static MRound()
		{
			string description = "Returns a number rounded to the desired multiple. MROUND rounds up, away from zero, if the remainder of dividing number by multiple is greater than or equal to half the value of multiple.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_MRound_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_MRound_NumberInfo"),
				new ArgumentInfo("Multiple", "is the multiple to which you want to round number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Multiple", "Spreadsheet_Functions_MRound_MultipleInfo")
			};
			MRound.Info = new FunctionInfo(MRound.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num * num2 < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			if (num == 0.0 || num2 == 0.0)
			{
				return NumberExpression.Zero;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.MRound(num, num2));
		}

		public static readonly string FunctionName = "MROUND";

		static readonly FunctionInfo Info;
	}
}
