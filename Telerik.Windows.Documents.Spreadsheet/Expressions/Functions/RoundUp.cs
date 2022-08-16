using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class RoundUp : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return RoundUp.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return RoundUp.Info;
			}
		}

		static RoundUp()
		{
			string description = "Rounds a number up, away from zero.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_RoundUp_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number that you want rounded up.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_RoundUp_NumberInfo"),
				new ArgumentInfo("Num_digits", "is the number of digits to which you want to round. Negative rounds to the left of the decimal point; zero or omitted, to the nearest integer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NumDigits", "Spreadsheet_Functions_RoundUpDown_NumDigitsInfo")
			};
			RoundUp.Info = new FunctionInfo(RoundUp.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double value = context.Arguments[0];
			int decimals = (int)context.Arguments[1];
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.RoundUp(value, decimals));
		}

		public static readonly string FunctionName = "ROUNDUP";

		static readonly FunctionInfo Info;
	}
}
