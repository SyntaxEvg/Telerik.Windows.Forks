using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Ceiling : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Ceiling.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Ceiling.Info;
			}
		}

		static Ceiling()
		{
			string description = "Returns number rounded up, away from zero, to the nearest multiple of significance. For example, if you want to avoid using pennies in your prices and your product is priced at $4.42, use the formula =CEILING(4.42,0.05) to round prices up to the nearest nickel.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Ceiling_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the value you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Ceiling_NumberInfo"),
				new ArgumentInfo("Significance", "is the multiple to which you want to round.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Significance", "Spreadsheet_Functions_Ceiling_NumberInfo")
			};
			Ceiling.Info = new FunctionInfo(Ceiling.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num > 0.0 && num2 < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Ceiling(num, num2));
		}

		public static readonly string FunctionName = "CEILING";

		static readonly FunctionInfo Info;
	}
}
