using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImSub : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImSub.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImSub.Info;
			}
		}

		static ImSub()
		{
			string description = "Returns the difference of two complex numbers in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImSub_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber1", "The complex number from which to subtract inumber2.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber1", "Spreadsheet_Functions_ImSub_Inumber1Info"),
				new ArgumentInfo("Inumber2", "The complex number to subtract from inumber1.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber2", "Spreadsheet_Functions_ImSub_Inumber2Info")
			};
			ImSub.Info = new FunctionInfo(ImSub.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string complexSymbol;
				if (MathUtility.TryGetImaginarySymbol(context.Arguments, out complexSymbol))
				{
					Complex complex = EngineeringFunctions.IMSUB(context.Arguments[0], context.Arguments[1]);
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

		public static readonly string FunctionName = "IMSUB";

		static readonly FunctionInfo Info;
	}
}
