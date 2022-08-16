using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Int : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Int.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Int.Info;
			}
		}

		static Int()
		{
			string description = "Rounds a number down to the nearest integer.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Int_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the real number you want to round down to an integer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Int_NumberInfo")
			};
			Int.Info = new FunctionInfo(Int.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Floor(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "INT";

		static readonly FunctionInfo Info;
	}
}
