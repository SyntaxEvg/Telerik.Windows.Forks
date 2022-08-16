using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Round : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Round.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Round.Info;
			}
		}

		static Round()
		{
			string description = "Rounds a number to a specified number of digits.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Round_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the number you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Round_NumberInfo"),
				new ArgumentInfo("Num_digits", "is the number of digits to which you want to round. Negative rounds to the left of the decimal point; zero to the nearest integer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NumDigits", "Spreadsheet_Functions_Round_NumDigitsInfo")
			};
			Round.Info = new FunctionInfo(Round.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double value = context.Arguments[0];
			int numDigits = (int)context.Arguments[1];
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Round(value, numDigits));
		}

		public static readonly string FunctionName = "ROUND";

		static readonly FunctionInfo Info;
	}
}
