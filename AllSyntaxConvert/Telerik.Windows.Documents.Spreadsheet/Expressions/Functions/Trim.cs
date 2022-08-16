using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Trim : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Trim.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Trim.Info;
			}
		}

		static Trim()
		{
			string description = "Removes all spaces from text except for single spaces between words.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Trim_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text you want to trim.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Trim_TextInfo")
			};
			Trim.Info = new FunctionInfo(Trim.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			object[] arguments = context.Arguments;
			string input = arguments[0].ToString();
			RegexOptions options = RegexOptions.None;
			string pattern = "[ ]{2,}";
			Regex regex = new Regex(pattern, options);
			char c = ' ';
			string value = regex.Replace(input, c.ToString()).Trim(new char[] { c });
			return new StringExpression(value);
		}

		public static readonly string FunctionName = "TRIM";

		static readonly FunctionInfo Info;
	}
}
