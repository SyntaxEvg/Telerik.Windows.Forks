using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Imaginary : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Imaginary.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Imaginary.Info;
			}
		}

		static Imaginary()
		{
			string description = "Returns the imaginary coefficient of a complex number in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Imaginary_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the imaginary coefficient.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_Imaginary_InumberInfo")
			};
			Imaginary.Info = new FunctionInfo(Imaginary.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				double number = EngineeringFunctions.IMAGINARY(context.Arguments[0]);
				result = NumberExpression.CreateValidNumberOrErrorExpression(number);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMAGINARY";

		static readonly FunctionInfo Info;
	}
}
