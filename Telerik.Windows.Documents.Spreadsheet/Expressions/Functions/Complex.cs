using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Complex : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Complex.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Complex.Info;
			}
		}

		static Complex()
		{
			string description = "Converts real and imaginary coefficients into a complex number of the form x + yi or x + yj.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Complex_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Real_num", "The real coefficient of the complex number.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Real_num", "Spreadsheet_Functions_Complex_Real_numInfo"),
				new ArgumentInfo("I_num", "The imaginary coefficient of the complex number.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_I_num", "Spreadsheet_Functions_Complex_I_numInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Suffix", "The suffix for the imaginary component of the complex number. If omitted, suffix is assumed to be \"i\".", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Suffix", "Spreadsheet_Functions_Complex_SuffixInfo")
			};
			Complex.Info = new FunctionInfo(Complex.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string text = ((context.Arguments.Length < 3 || string.IsNullOrEmpty(context.Arguments[2])) ? "i" : context.Arguments[2]);
			context.Arguments[0] = (string.IsNullOrEmpty(context.Arguments[0]) ? "0" : context.Arguments[0]);
			context.Arguments[1] = (string.IsNullOrEmpty(context.Arguments[1]) ? "0" : context.Arguments[1]);
			double a;
			bool flag = double.TryParse(context.Arguments[0], out a);
			double b;
			flag &= double.TryParse(context.Arguments[1], out b);
			if (!(flag & (text == "i" || text == "j")))
			{
				return ErrorExpressions.ValueError;
			}
			string value = EngineeringFunctions.COMPLEX(a, b, text);
			return new StringExpression(value);
		}

		public static readonly string FunctionName = "COMPLEX";

		static readonly FunctionInfo Info;
	}
}
