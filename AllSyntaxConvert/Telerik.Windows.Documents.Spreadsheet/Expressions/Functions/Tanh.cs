using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Tanh : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Tanh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Tanh.Info;
			}
		}

		static Tanh()
		{
			string description = "Returns the hyperbolic tangent of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Tanh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Tanh_NumberInfo")
			};
			Tanh.Info = new FunctionInfo(Tanh.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Tanh(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "TANH";

		static readonly FunctionInfo Info;
	}
}
