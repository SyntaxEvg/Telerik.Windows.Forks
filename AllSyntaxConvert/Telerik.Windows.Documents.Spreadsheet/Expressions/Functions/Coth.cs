using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Coth : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Coth.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Coth.Info;
			}
		}

		static Coth()
		{
			string description = "Return the hyperbolic cotangent of a hyperbolic angle.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Coth_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the hyperbolic cotangent.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Coth_NumberInfo")
			};
			Coth.Info = new FunctionInfo(Coth.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num >= MathUtility.CotangentLimit)
			{
				return ErrorExpressions.NumberError;
			}
			if (num == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(1.0 / Math.Tanh(num));
		}

		public static readonly string FunctionName = "COTH";

		static readonly FunctionInfo Info;
	}
}
