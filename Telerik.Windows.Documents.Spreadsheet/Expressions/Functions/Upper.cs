using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Upper : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Upper.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Upper.Info;
			}
		}

		static Upper()
		{
			string description = "Converts text to uppercase.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Upper_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text you want converted to uppercase. Text can be a reference or text string.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Upper_TextInfo")
			};
			Upper.Info = new FunctionInfo(Upper.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string text = context.Arguments[0];
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char value = char.ToUpperInvariant(text[i]);
				stringBuilder.Append(value);
			}
			return new StringExpression(stringBuilder.ToString());
		}

		public static readonly string FunctionName = "UPPER";

		static readonly FunctionInfo Info;
	}
}
