using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Lower : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Lower.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Lower.Info;
			}
		}

		static Lower()
		{
			string description = "Converts all uppercase letters in a text string to lowercase.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Lower_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text you want to convert to lowercase. LOWER does not change characters in text that are not letters.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Lower_TextInfo")
			};
			Lower.Info = new FunctionInfo(Lower.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string text = context.Arguments[0];
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char value = char.ToLowerInvariant(text[i]);
				stringBuilder.Append(value);
			}
			return new StringExpression(stringBuilder.ToString());
		}

		public static readonly string FunctionName = "LOWER";

		static readonly FunctionInfo Info;
	}
}
