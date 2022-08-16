using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Atanh : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Atanh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Atanh.Info;
			}
		}

		static Atanh()
		{
			string description = "Returns the inverse hyperbolic tangent of a number. Number must be between -1 and 1 (excluding -1 and 1). The inverse hyperbolic tangent is the value whose hyperbolic tangent is number, so ATANH(TANH(number)) equals number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Atanh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number between -1 and 1 excluding -1 and 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Atanh_NumberInfo")
			};
			Atanh.Info = new FunctionInfo(Atanh.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num >= 1.0 || num <= 1.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(MathUtility.Atanh(num));
		}

		public static readonly string FunctionName = "ATANH";

		static readonly FunctionInfo Info;
	}
}
