using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Acot : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Acot.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Acot.Info;
			}
		}

		static Acot()
		{
			string description = "Returns the principal value of the arccotangent, or inverse cotangent, of a number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Acot_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the cotangent of the angle you want. This must be a real number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Acot_NumberInfo")
			};
			Acot.Info = new FunctionInfo(Acot.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = Math.Atan(1.0 / num);
			if (num < 0.0)
			{
				num2 += 3.141592653589793;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num2);
		}

		public static readonly string FunctionName = "ACOT";

		static readonly FunctionInfo Info;
	}
}
