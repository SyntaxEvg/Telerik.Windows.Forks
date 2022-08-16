using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Trunc : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Trunc.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Trunc.Info;
			}
		}

		static Trunc()
		{
			string description = "Truncates a number to an integer by removing the fractional part of the number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Trunc_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the number you want to truncate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Trunc_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Num_digits", "is a number specifying the precision of the truncation. The default value for num_digits is 0 (zero).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NumDigits", "Spreadsheet_Functions_Trunc_NumDigitsInfo")
			};
			Trunc.Info = new FunctionInfo(Trunc.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double value = context.Arguments[0];
			int decimals = 0;
			if (context.Arguments.Length > 1)
			{
				decimals = (int)context.Arguments[1];
			}
			double number = MathUtility.RoundDown(value, decimals);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "TRUNC";

		static readonly FunctionInfo Info;
	}
}
