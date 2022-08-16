using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Sign : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sign.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sign.Info;
			}
		}

		static Sign()
		{
			string description = "Returns the sign of a number: 1 if the number is positive, zero if the number is zero, or -1 if the number is negative.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sign_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is any real number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Sign_NumberInfo")
			};
			Sign.Info = new FunctionInfo(Sign.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = (double)Math.Sign(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SIGN";

		static readonly FunctionInfo Info;
	}
}
