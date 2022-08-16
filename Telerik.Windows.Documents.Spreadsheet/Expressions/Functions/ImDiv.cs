using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImDiv : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImDiv.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImDiv.Info;
			}
		}

		static ImDiv()
		{
			string description = "Returns the quotient of two complex numbers in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImDiv_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber1", "The complex numerator or dividend.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber1", "Spreadsheet_Functions_ImDiv_Inumber1Info"),
				new ArgumentInfo("Inumber2", "The complex denominator or divisor.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber2", "Spreadsheet_Functions_ImDiv_Inumber2Info")
			};
			ImDiv.Info = new FunctionInfo(ImDiv.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string complexSymbol;
				if (MathUtility.TryGetImaginarySymbol(context.Arguments, out complexSymbol))
				{
					Complex complex = EngineeringFunctions.IMDIV(context.Arguments[0], context.Arguments[1]);
					string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, complexSymbol);
					result = new StringExpression(value);
				}
				else
				{
					result = ErrorExpressions.ValueError;
				}
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMDIV";

		static readonly FunctionInfo Info;
	}
}
