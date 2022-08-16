using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImLog10 : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImLog10.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImLog10.Info;
			}
		}

		static ImLog10()
		{
			string description = "Returns the common logarithm (base 10) of a complex number in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImLog10_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the common logarithm.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImLog10_InumberInfo")
			};
			ImLog10.Info = new FunctionInfo(ImLog10.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string imaginarySymbol = MathUtility.GetImaginarySymbol(context.Arguments[0]);
				global::System.Numerics.Complex complex = EngineeringFunctions.IMLOG10(context.Arguments[0]);
				string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new StringExpression(value);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMLOG10";

		static readonly FunctionInfo Info;
	}
}
