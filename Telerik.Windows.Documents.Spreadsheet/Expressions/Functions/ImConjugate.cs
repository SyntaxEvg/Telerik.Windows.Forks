using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImConjugate : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImConjugate.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImConjugate.Info;
			}
		}

		static ImConjugate()
		{
			string description = "Returns the complex conjugate of a complex number in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImConjugate_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the conjugate.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImConjugate_InumberInfo")
			};
			ImConjugate.Info = new FunctionInfo(ImConjugate.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				Complex complex = EngineeringFunctions.IMCONJUGATE(context.Arguments[0]);
				string imaginarySymbol = MathUtility.GetImaginarySymbol(context.Arguments[0]);
				string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new StringExpression(value);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMCONJUGATE";

		static readonly FunctionInfo Info;
	}
}
