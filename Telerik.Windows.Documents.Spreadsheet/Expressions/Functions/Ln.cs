using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Ln : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Ln.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Ln.Info;
			}
		}

		static Ln()
		{
			string description = "Returns the natural logarithm of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Ln_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the positive real number for which you want the natural logarithm.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Ln_NumberInfo")
			};
			Ln.Info = new FunctionInfo(Ln.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num <= 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(Math.Log(num));
		}

		public static readonly string FunctionName = "LN";

		static readonly FunctionInfo Info;
	}
}
