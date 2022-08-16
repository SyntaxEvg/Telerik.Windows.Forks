﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImDiv : global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.StringsInFunction
	{
		public override string Name
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImDiv.FunctionName;
			}
		}

		public override global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo FunctionInfo
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImDiv.Info;
			}
		}

		static ImDiv()
		{
			string description = "Returns the quotient of two complex numbers in x + yi or x + yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImDiv_Info";
			global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo> requiredArgumentsInfos = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo[]
			{
				new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo("Inumber1", "The complex numerator or dividend.", global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber1", "Spreadsheet_Functions_ImDiv_Inumber1Info"),
				new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentInfo("Inumber2", "The complex denominator or divisor.", global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber2", "Spreadsheet_Functions_ImDiv_Inumber2Info")
			};
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImDiv.Info = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.ImDiv.FunctionName, global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression EvaluateOverride(global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionEvaluationContext<string> context)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Expressions.RadExpression result;
			try
			{
				string complexSymbol;
				if (global::Telerik.Windows.Documents.Spreadsheet.Expressions.MathUtility.TryGetImaginarySymbol(context.Arguments, out complexSymbol))
				{
					global::System.Numerics.Complex complex = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.IMDIV(context.Arguments[0], context.Arguments[1]);
					string value = global::Telerik.Windows.Documents.Spreadsheet.Maths.EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, complexSymbol);
					result = new global::Telerik.Windows.Documents.Spreadsheet.Expressions.StringExpression(value);
				}
				else
				{
					result = global::Telerik.Windows.Documents.Spreadsheet.Expressions.ErrorExpressions.ValueError;
				}
			}
			catch (global::System.Exception)
			{
				result = global::Telerik.Windows.Documents.Spreadsheet.Expressions.ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMDIV";

		private static readonly global::Telerik.Windows.Documents.Spreadsheet.Expressions.Functions.FunctionInfo Info;
	}
}
