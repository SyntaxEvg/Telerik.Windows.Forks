using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Search : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Search.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Search.Info;
			}
		}

		static Search()
		{
			string description = "Locates one text string within a second text string, and return the number of the starting position of the first text string from the first character of the second text string. SEARCH is not case sensitive. If you want to do a case sensitive search, you can use FIND.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Search_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("FindText", "is the text you want to find.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_FindText", "Spreadsheet_Functions_FindSearch_FindTextInfo"),
				new ArgumentInfo("WithinText", "is the text containing the text you want to find.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_WithinText", "Spreadsheet_Functions_FindSearch_WithinTextInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("StartNumber", "specifies the character at which to start the search. The first character in within_text is character number 1. If you omit start_num, it is assumed to be 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_StartNumber", "Spreadsheet_Functions_FindSearch_StartNumberInfo")
			};
			Search.Info = new FunctionInfo(Search.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string text = context.Arguments[0].ToString();
			string text2 = context.Arguments[1].ToString();
			text = text.ToLowerInvariant();
			text2 = text2.ToLowerInvariant();
			int num;
			if (context.Arguments.Length == 2)
			{
				num = text2.IndexOf(text, StringComparison.Ordinal);
			}
			else
			{
				double num2 = (double)context.Arguments[2];
				if (num2 < 1.0 || num2 > (double)text2.Length)
				{
					return ErrorExpressions.ValueError;
				}
				num = text2.IndexOf(text, (int)num2 - 1, StringComparison.Ordinal);
			}
			if (num == -1)
			{
				return ErrorExpressions.ValueError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression((double)(num + 1));
		}

		public static readonly string FunctionName = "SEARCH";

		static readonly FunctionInfo Info;
	}
}
