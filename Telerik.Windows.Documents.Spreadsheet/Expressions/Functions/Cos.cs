using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Cos : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Cos.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Cos.Info;
			}
		}

		static Cos()
		{
			string description = "Returns the cosine of an angle.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Cos_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the angle in radians of which you want the cosine.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Cos_NumberInfo")
			};
			Cos.Info = new FunctionInfo(Cos.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Cos(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "COS";

		static readonly FunctionInfo Info;
	}
}
