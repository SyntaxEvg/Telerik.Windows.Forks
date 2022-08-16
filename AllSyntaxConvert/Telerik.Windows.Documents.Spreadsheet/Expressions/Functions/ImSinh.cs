﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class ImSinh : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return ImSinh.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return ImSinh.Info;
			}
		}

		static ImSinh()
		{
			string description = "Returns the hyperbolic sine of a complex number in x+yi or x+yj text format.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_ImSinh_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Inumber", "A complex number for which you want the hyperbolic sine.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Inumber", "Spreadsheet_Functions_ImSinh_InumberInfo")
			};
			ImSinh.Info = new FunctionInfo(ImSinh.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			RadExpression result;
			try
			{
				string imaginarySymbol = MathUtility.GetImaginarySymbol(context.Arguments[0]);
				global::System.Numerics.Complex complex = EngineeringFunctions.IMSINH(context.Arguments[0]);
				string value = EngineeringFunctions.COMPLEX(complex.Real, complex.Imaginary, imaginarySymbol);
				result = new StringExpression(value);
			}
			catch (Exception)
			{
				result = ErrorExpressions.NumberError;
			}
			return result;
		}

		public static readonly string FunctionName = "IMSINH";

		static readonly FunctionInfo Info;
	}
}
