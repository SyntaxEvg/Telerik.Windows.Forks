using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Combin : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Combin.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Combin.Info;
			}
		}

		static Combin()
		{
			string description = "Returns the number of combinations for a given number of items. Use COMBIN to determine the total possible number of groups for a given number of items.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Combin_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the number of items", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Combin_NumberInfo"),
				new ArgumentInfo("NumberChosen", "is the number of items in each combination.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NumberChosen", "Spreadsheet_Functions_Combin_NumberChosenInfo")
			};
			Combin.Info = new FunctionInfo(Combin.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = MathUtility.RoundDown(context.Arguments[0], 0);
			double num2 = MathUtility.RoundDown(context.Arguments[1], 0);
			if (num < 0.0 || num2 < 0.0 || num < num2)
			{
				return ErrorExpressions.NumberError;
			}
			double num3 = MathUtility.Factorial(num);
			double num4 = MathUtility.Factorial(num2) * MathUtility.Factorial(num - num2);
			return NumberExpression.CreateValidNumberOrErrorExpression(num3 / num4);
		}

		public static readonly string FunctionName = "COMBIN";

		static readonly FunctionInfo Info;
	}
}
