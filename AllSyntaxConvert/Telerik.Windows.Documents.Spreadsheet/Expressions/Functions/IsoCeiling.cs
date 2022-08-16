using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IsoCeiling : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return IsoCeiling.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IsoCeiling.Info;
			}
		}

		static IsoCeiling()
		{
			string description = "Returns a number that is rounded up to the nearest integer or to the nearest multiple of significance. Regardless of the sign of the number, the number is rounded up. However, if the number or the significance is zero, zero is returned.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IsoCeiling_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value you want to round", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_IsoCeiling_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Significance", "is the multiple to which you want to round. If significance is omitted, its default value is 1", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Significance", "Spreadsheet_Functions_IsoCeiling_SignificanceInfo")
			};
			IsoCeiling.Info = new FunctionInfo(IsoCeiling.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double significance = 1.0;
			if (context.Arguments.Length >= 2)
			{
				significance = context.Arguments[1];
			}
			if (num >= MathUtility.CeilingFloorUpperLimit || num <= MathUtility.CeilingFloorLowerLimit)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.CeilingPrecise(num, significance));
		}

		public static readonly string FunctionName = "ISO.CEILING";

		static readonly FunctionInfo Info;
	}
}
