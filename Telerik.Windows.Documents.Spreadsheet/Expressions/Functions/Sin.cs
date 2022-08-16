using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Sin : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sin.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sin.Info;
			}
		}

		static Sin()
		{
			string description = "Returns the sine of an angle.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sin_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the sine. Degrees * Pi()/180 = radians.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Sin_NumberInfo")
			};
			Sin.Info = new FunctionInfo(Sin.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Sin(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SIN";

		static readonly FunctionInfo Info;
	}
}
