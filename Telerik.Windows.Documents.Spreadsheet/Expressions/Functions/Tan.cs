using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Tan : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Tan.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Tan.Info;
			}
		}

		static Tan()
		{
			string description = "Returns the tangent of an angle.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Tan_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians for which you want the tangent. Degrees * Pi()/180 = radians.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Tan_NumberInfo")
			};
			Tan.Info = new FunctionInfo(Tan.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Tan(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "TAN";

		static readonly FunctionInfo Info;
	}
}
