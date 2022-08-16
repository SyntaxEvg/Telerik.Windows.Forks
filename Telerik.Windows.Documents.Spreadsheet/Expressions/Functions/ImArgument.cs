using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImArgument : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImArgument.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImArgument.Info;
			}
		}

		static ImArgument()
		{
			string description = "Returns the argument θ (theta), an angle expressed in radians, such that: x+yi = |x+yi|(cos(θ) + i sin(θ))";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImArgument_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the argument θ (theta).", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImArgument_InumberInfo")
			};
			ImArgument.Info = new FunctionInfo(ImArgument.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				double num = EngineeringFunctions.IMABS(context.Arguments[0]);
				if (num < 1E-300)
				{
					result = ErrorExpressions.DivisionByZero;
				}
				else
				{
					double number = EngineeringFunctions.IMARGUMENT(context.Arguments[0]);
					result = NumberExpression.CreateValidNumberOrErrorExpression(number);
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		const double ZeroPrecision = 1E-300;

		public static readonly string FunctionName = "IMARGUMENT";

		static readonly FunctionInfo Info;
	}
}
