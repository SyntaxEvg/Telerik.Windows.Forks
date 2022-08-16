using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Concatenate : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Concatenate.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Concatenate.Info;
			}
		}

		static Concatenate()
		{
			string description = "Joins several text strings into one text string.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Concatenate_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "Text1, text2, ...     are 1 to 30 text items to be joined into a single text item. The text items can be text strings, numbers, or single-cell references.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Concatenate_TextInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "Text1, text2, ...     are 1 to 30 text items to be joined into a single text item. The text items can be text strings, numbers, or single-cell references.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Concatenate_TextInfo")
			};
			Concatenate.Info = new FunctionInfo(Concatenate.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				stringBuilder.Append(context.Arguments[i]);
			}
			return new StringExpression(stringBuilder.ToString());
		}

		public static readonly string FunctionName = "CONCATENATE";

		static readonly FunctionInfo Info;
	}
}
