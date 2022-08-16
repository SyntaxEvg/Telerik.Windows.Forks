using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Floor : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Floor.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Floor.Info;
			}
		}

		static Floor()
		{
			string description = "Rounds number down, toward zero, to the nearest multiple of significance.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Floor_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Floor_NumberInfo"),
				new ArgumentInfo("Significance", "is the multiple to which you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Significance", "Spreadsheet_Functions_Floor_SignificanceInfo")
			};
			Floor.Info = new FunctionInfo(Floor.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num > 0.0 && num2 < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			if (num2 == 0.0 && num != 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Floor(num, num2));
		}

		public static readonly string FunctionName = "FLOOR";

		static readonly FunctionInfo Info;
	}
}
