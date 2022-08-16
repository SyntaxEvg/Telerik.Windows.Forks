using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Left : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Left.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Left.Info;
			}
		}

		static Left()
		{
			string description = "Returns the first character or characters in a text string, based on the number of characters you specify.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Left_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text string that contains the characters you want to extract.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_LeftRight_TextInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "specifies the number of characters you want LEFT to extract.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Left_NumberInfo")
			};
			Left.Info = new FunctionInfo(Left.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string text = context.Arguments[0].ToString();
			string value = string.Empty;
			if (context.Arguments.Length == 1)
			{
				if (text.Length > 0)
				{
					value = text[0].ToString();
				}
			}
			else
			{
				double num = (double)context.Arguments[1];
				if (num < 0.0)
				{
					return ErrorExpressions.ValueError;
				}
				int num2 = (int)num;
				if (text.Length < num2)
				{
					num2 = text.Length;
				}
				value = text.Substring(0, num2);
			}
			return new StringExpression(value);
		}

		public static readonly string FunctionName = "LEFT";

		static readonly FunctionInfo Info;
	}
}
