using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Atan : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Atan.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Atan.Info;
			}
		}

		static Atan()
		{
			string description = "Returns the arctangent, or inverse tangent, of a number. The arctangent is the angle whose tangent is number. The returned angle is given in radians in the range -pi/2 to pi/2.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Atan_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the tangent of the angle you want.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Atan_NumberInfo")
			};
			Atan.Info = new FunctionInfo(Atan.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = Math.Atan(context.Arguments[0]);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ATAN";

		static readonly FunctionInfo Info;
	}
}
