﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImSech : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImSech.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImSech.Info;
			}
		}

		static ImSech()
		{
			string description = "Returns the hyperbolic secant of a complex number in x+yi or x+yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImSech_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the hyperbolic secant.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSech_InumberInfo")
			};
			ImSech.Info = new FunctionInfo(ImSech.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string imaginarySymbol = MathUtility.GetImaginarySymbol(context.Arguments[0]);
				Complex complex = EngineeringFunctions.IMSECH(context.Arguments[0]);
				string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new StringExpression(value);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMSECH";

		static readonly FunctionInfo Info;
	}
}
