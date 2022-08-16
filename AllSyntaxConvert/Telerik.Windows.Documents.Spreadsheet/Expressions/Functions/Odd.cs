using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Odd : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Odd.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Odd.Info;
			}
		}

		static Odd()
		{
			string description = "Rounds a positive number up and negative number down to the nearest odd integer.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Odd_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Odd_NumberInfo")
			};
			Odd.Info = new FunctionInfo(Odd.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < 0.0)
			{
				num = Math.Floor(num);
				if (num % 2.0 == 0.0)
				{
					num -= 1.0;
				}
			}
			else
			{
				num = Math.Ceiling(num);
				if (num % 2.0 == 0.0)
				{
					num += 1.0;
				}
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "ODD";

		static readonly FunctionInfo Info;
	}
}
