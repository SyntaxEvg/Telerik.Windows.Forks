using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImSec : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImSec.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImSec.Info;
			}
		}

		static ImSec()
		{
			string description = "Returns the secant of a complex number in x+yi or x+yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImSec_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the secant.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSec_InumberInfo")
			};
			ImSec.Info = new FunctionInfo(ImSec.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string imaginarySymbol = MathUtility.GetImaginarySymbol(context.Arguments[0]);
				Complex complex = EngineeringFunctions.IMSEC(context.Arguments[0]);
				string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new StringExpression(value);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMSEC";

		static readonly FunctionInfo Info;
	}
}
