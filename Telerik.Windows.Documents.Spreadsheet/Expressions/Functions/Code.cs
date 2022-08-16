using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Code : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Code.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Code.Info;
			}
		}

		static Code()
		{
			string description = "Returns a numeric code for the first character in a text string. The returned code corresponds to the character set used by your computer.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Code_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text for which you want the code of the first character.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Code_TextInfo")
			};
			Code.Info = new FunctionInfo(Code.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string text = context.Arguments[0];
			if (text.Length == 0)
			{
				return ErrorExpressions.ValueError;
			}
			int num = (int)text[0];
			return NumberExpression.CreateValidNumberOrErrorExpression((double)num);
		}

		public static readonly string FunctionName = "CODE";

		static readonly FunctionInfo Info;
	}
}
