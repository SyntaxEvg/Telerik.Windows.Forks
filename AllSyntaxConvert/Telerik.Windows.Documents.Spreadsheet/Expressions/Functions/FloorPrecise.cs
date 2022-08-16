using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class FloorPrecise : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return FloorPrecise.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return FloorPrecise.Info;
			}
		}

		static FloorPrecise()
		{
			string description = "Returns a number that is rounded down to the nearest integer or to the nearest multiple of significance. Regardless of the sign of the number, the number is rounded down. However, if the number or the significance is zero, zero is returned.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_FloorPrecise_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Floor_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Significance", "is the multiple to which number is to be rounded.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Significance", "Spreadsheet_Functions_Floor_SignificanceInfo")
			};
			FloorPrecise.Info = new FunctionInfo(FloorPrecise.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.FloorPrecise(num, significance));
		}

		public static readonly string FunctionName = "FLOOR.PRECISE";

		static readonly FunctionInfo Info;
	}
}
