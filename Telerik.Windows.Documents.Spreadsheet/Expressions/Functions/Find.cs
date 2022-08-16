using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Find : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Find.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Find.Info;
			}
		}

		static Find()
		{
			string description = "Locate one text string within a second text string, and return the number of the starting position of the first text string from the first character of the second text string. FIND always counts each character, whether single-byte or double-byte, as 1, no matter what the default language setting is.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Find_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("FindText", "is the text you want to find.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_FindText", "Spreadsheet_Functions_FindSearch_FindTextInfo"),
				new ArgumentInfo("WithinText", "is the text containing the text you want to find.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_WithinText", "Spreadsheet_Functions_FindSearch_WithinTextInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("StartNumber", "specifies the character at which to start the search. The first character in within_text is character number 1. If you omit start_num, it is assumed to be 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_StartNumber", "Spreadsheet_Functions_FindSearch_StartNumberInfo")
			};
			Find.Info = new FunctionInfo(Find.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string value = context.Arguments[0].ToString();
			string text = context.Arguments[1].ToString();
			int num;
			if (context.Arguments.Length == 2)
			{
				num = text.IndexOf(value, StringComparison.Ordinal);
			}
			else
			{
				double num2 = (double)context.Arguments[2];
				if (num2 < 1.0 || num2 > (double)text.Length)
				{
					return ErrorExpressions.ValueError;
				}
				num = text.IndexOf(value, (int)num2 - 1, StringComparison.Ordinal);
			}
			if (num == -1)
			{
				return ErrorExpressions.ValueError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression((double)(num + 1));
		}

		public static readonly string FunctionName = "FIND";

		static readonly FunctionInfo Info;
	}
}
