using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Acoth : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Acoth.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Acoth.Info;
			}
		}

		static Acoth()
		{
			string description = "Returns the inverse hyperbolic cotangent of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Acoth_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "The absolute value of Number must be greater than 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Acoth_NumberInfo")
			};
			Acoth.Info = new FunctionInfo(Acoth.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num <= 1.0 && num >= 1.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Acoth(num));
		}

		public static readonly string FunctionName = "ACOTH";

		static readonly FunctionInfo Info;
	}
}
