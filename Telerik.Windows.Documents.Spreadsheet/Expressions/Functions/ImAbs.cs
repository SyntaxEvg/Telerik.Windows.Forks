using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImAbs : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImAbs.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImAbs.Info;
			}
		}

		static ImAbs()
		{
			string description = "Returns the absolute value (modulus) of a complex number in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImAbs_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the absolute value.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImAbs_InumberInfo")
			};
			ImAbs.Info = new FunctionInfo(ImAbs.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				double number = EngineeringFunctions.IMABS(context.Arguments[0]);
				result = NumberExpression.CreateValidNumberOrErrorExpression(number);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMABS";

		static readonly FunctionInfo Info;
	}
}
