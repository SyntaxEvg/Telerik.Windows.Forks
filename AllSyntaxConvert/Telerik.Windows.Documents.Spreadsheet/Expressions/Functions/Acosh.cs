using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Acosh : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Acosh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Acosh.Info;
			}
		}

		static Acosh()
		{
			string description = "Returns the inverse hyperbolic cosine of a number. The number must be greater than or equal to 1. The inverse hyperbolic cosine is the value whose hyperbolic cosine is number, so ACOSH(COSH(number)) equals number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Acosh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number equal to or greater than 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Acosh_NumberInfo")
			};
			Acosh.Info = new FunctionInfo(Acosh.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < 1.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Acosh(num));
		}

		public static readonly string FunctionName = "ACOSH";

		static readonly FunctionInfo Info;
	}
}
