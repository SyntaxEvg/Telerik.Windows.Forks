using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Rept : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Rept.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Rept.Info;
			}
		}

		static Rept()
		{
			string description = "Repeats text a given number of times. Use REPT to fill a cell with a number of instances of a text string.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Rept_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text you want to repeat.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Rept_TextInfo"),
				new ArgumentInfo("NumberTimes", "is a positive number specifying the number of times to repeat text.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NumberTimes", "Spreadsheet_Functions_Rept_NumberTimesInfo")
			};
			Rept.Info = new FunctionInfo(Rept.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string text = context.Arguments[0].ToString();
			double num = (double)context.Arguments[1];
			StringBuilder stringBuilder = new StringBuilder();
			int num2 = text.Length * (int)num;
			if (num < 0.0 || num2 > 32767)
			{
				return ErrorExpressions.ValueError;
			}
			int num3 = 0;
			while ((double)num3 < num)
			{
				stringBuilder.Append(text);
				num3++;
			}
			return new StringExpression(stringBuilder.ToString());
		}

		public static readonly string FunctionName = "REPT";

		static readonly FunctionInfo Info;
	}
}
