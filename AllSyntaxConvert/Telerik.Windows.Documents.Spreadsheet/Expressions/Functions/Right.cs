using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Right : FunctionWithArguments
	{
		public override string Name
		{
			get
			{
				return Right.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Right.Info;
			}
		}

		static Right()
		{
			string description = "Returns the last character or characters in a text string, based on the number of characters you specify.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Right_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text string that contains the characters you want to extract.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_LeftRight_TextInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "specifies the number of characters you want RIGHT to extract.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Right_NumberInfo")
			};
			Right.Info = new FunctionInfo(Right.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<object> context)
		{
			string text = (string)context.Arguments[0];
			string value = string.Empty;
			if (context.Arguments.Length == 1)
			{
				if (text.Length > 0)
				{
					value = text[text.Length - 1].ToString();
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
				value = text.Substring(text.Length - num2, num2);
			}
			return new StringExpression(value);
		}

		public static readonly string FunctionName = "RIGHT";

		static readonly FunctionInfo Info;
	}
}
