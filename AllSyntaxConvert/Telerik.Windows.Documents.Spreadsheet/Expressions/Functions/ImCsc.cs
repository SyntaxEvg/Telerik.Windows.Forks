using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImCsc : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImCsc.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImCsc.Info;
			}
		}

		static ImCsc()
		{
			string description = "Returns the cosecant of a complex number in x+yi or x+yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImCsc_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the cosecant.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImCsc_InumberInfo")
			};
			ImCsc.Info = new FunctionInfo(ImCsc.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression EvaluateOverride(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionEvaluationContext<string> context)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression result;
			try
			{
				string imaginarySymbol = global::Telerik.Windows.Documents.Spreadsheet.Expressions.MathUtility.GetImaginarySymbol(context.Arguments[0]);
				global::System.Numerics.Complex complex = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.IMCSC(context.Arguments[0]);
				string value = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.StringExpression(value);
			}
			catch (global::System.Exception)
			{
				result = global::Telerik.Windows.Documents.Spreadsheet.Expressions.ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMCSC";

		static readonly FunctionInfo Info;
	}
}
