using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Cosh : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Cosh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Cosh.Info;
			}
		}

		static Cosh()
		{
			string description = "Returns the hyperbolic cosine of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Cosh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Cosh_NumberInfo")
			};
			Cosh.Info = new FunctionInfo(Cosh.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Cosh(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "COSH";

		static readonly FunctionInfo Info;
	}
}
