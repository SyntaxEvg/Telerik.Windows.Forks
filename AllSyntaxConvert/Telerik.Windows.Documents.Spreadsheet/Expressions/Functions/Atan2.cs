using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Atan2 : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Atan2.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Atan2.Info;
			}
		}

		static Atan2()
		{
			string description = "Returns the arctangent, or inverse tangent, of the specified x- and y-coordinates. The arctangent is the angle from the x-axis to a line containing the origin (0, 0) and a point with coordinates (x_num, y_num). The angle is given in radians between -pi and pi, excluding -pi.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Atan2_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("X_num", "is the x-coordinate of the point.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_XNum", "Spreadsheet_Functions_Atan2_XNumInfo"),
				new ArgumentInfo("Y_num", "is the y-coordinate of the point.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_YNum", "Spreadsheet_Functions_Atan2_YNumInfo")
			};
			Atan2.Info = new FunctionInfo(Atan2.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num == 0.0 && num2 == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = Math.Atan2(num2, num);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ATAN2";

		static readonly FunctionInfo Info;
	}
}
