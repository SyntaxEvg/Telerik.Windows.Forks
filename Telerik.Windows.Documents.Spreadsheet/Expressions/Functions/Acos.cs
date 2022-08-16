using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Acos : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Acos.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Acos.Info;
			}
		}

		static Acos()
		{
			string description = "Returns the arccosine, or inverse cosine, of a number. The arccosine is the angle whose cosine is number. The returned angle is given in radians in the range 0 (zero) to pi.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Acos_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the cosine of the angle you want and must be from -1 to 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Acos_NumberInfo")
			};
			Acos.Info = new FunctionInfo(Acos.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num > 1.0 || num < -1.0)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(Math.Acos(num));
		}

		public static readonly string FunctionName = "ACOS";

		static readonly FunctionInfo Info;
	}
}
