using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class CeilingMath : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return CeilingMath.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return CeilingMath.Info;
			}
		}

		static CeilingMath()
		{
			string description = "Rounds a number up to the nearest integer or to the nearest multiple of significance.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_CeilingMath_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value you want to round. Number must be less than 9.99E+307 and greater than -2.229E-308.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_CeilingMath_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Significance", "is the multiple to which you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Significance", "Spreadsheet_Functions_CeilingMath_SignificanceInfo"),
				new ArgumentInfo("Mode", "For negative numbers, controls whether Number is rounded toward or away from zero.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Mode", "Spreadsheet_Functions_CeilingMath_ModeInfo")
			};
			CeilingMath.Info = new FunctionInfo(CeilingMath.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double significance = 1.0;
			if (context.Arguments.Length >= 2)
			{
				significance = context.Arguments[1];
			}
			bool flag = true;
			if (context.Arguments.Length > 2)
			{
				flag = context.Arguments[2] == 0.0;
			}
			if (num >= MathUtility.CeilingFloorUpperLimit || num <= MathUtility.CeilingFloorLowerLimit)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			if (num < 0.0 && !flag)
			{
				number = MathUtility.FloorPrecise(num, significance);
			}
			else
			{
				number = MathUtility.CeilingPrecise(num, significance);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "CEILING.MATH";

		static readonly FunctionInfo Info;
	}
}
