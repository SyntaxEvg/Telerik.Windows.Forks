using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImExp : global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.StringsInFunction
	{
		public override string Name
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImExp.FunctionName;
			}
		}

		public override global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo FunctionInfo
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImExp.Info;
			}
		}

		static ImExp()
		{
			string description = "Returns the exponential of a complex number in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImExp_Info";
			global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo> requiredArgumentsInfos = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo[]
			{
				new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo("Inumber", "A complex number for which you want the exponential.", global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImExp_InumberInfo")
			};
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImExp.Info = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImExp.FunctionName, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression EvaluateOverride(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionEvaluationContext<string> context)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression result;
			try
			{
				string imaginarySymbol = global::Telerik.Windows.Documents.Spreadsheet.Expressions.MathUtility.GetImaginarySymbol(context.Arguments[0]);
				global::System.Numerics.Complex complex = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.IMEXP(context.Arguments[0]);
				string value = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.StringExpression(value);
			}
			catch (global::System.Exception)
			{
				result = global::Telerik.Windows.Documents.Spreadsheet.Expressions.ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMEXP";

		private static readonly global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo Info;
	}
}
