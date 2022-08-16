using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Exp : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Exp.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Exp.Info;
			}
		}

		static Exp()
		{
			string description = "Returns e raised to the power of number. The constant e equals 2.71828182845904, the base of the natural logarithm.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Exp_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the exponent applied to the base e.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Exp_NumberInfo")
			};
			Exp.Info = new FunctionInfo(Exp.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Exp(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "EXP";

		static readonly FunctionInfo Info;
	}
}
