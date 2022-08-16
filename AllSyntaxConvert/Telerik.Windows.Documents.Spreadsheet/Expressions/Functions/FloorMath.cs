using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class FloorMath : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return FloorMath.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return FloorMath.Info;
			}
		}

		static FloorMath()
		{
			string description = "Round a number down to the nearest integer or to the nearest multiple of significance.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_FloorMath_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Floor_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Significance", "is the multiple to which you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Significance", "Spreadsheet_Functions_Floor_SignificanceInfo"),
				new ArgumentInfo("Mode", "is the direction (toward or away from 0) to round negative numbers.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Mode", "Spreadsheet_Functions_FloorMath_ModeInfo")
			};
			FloorMath.Info = new FunctionInfo(FloorMath.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
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
				number = MathUtility.CeilingPrecise(num, significance);
			}
			else
			{
				number = MathUtility.FloorPrecise(num, significance);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "FLOOR.MATH";

		static readonly FunctionInfo Info;
	}
}
