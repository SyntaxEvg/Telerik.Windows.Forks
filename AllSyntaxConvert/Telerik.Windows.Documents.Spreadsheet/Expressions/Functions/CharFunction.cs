using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class CharFunction : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return CharFunction.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return CharFunction.Info;
			}
		}

		static CharFunction()
		{
			string description = "Returns the character specified by a number. Use CHAR to translate code page numbers you might get from files on other types of computers into characters.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Char_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is a number between 1 and 255 specifying which character you want. The character is from the character set used by your computer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Char_NumberInfo")
			};
			CharFunction.Info = new FunctionInfo(CharFunction.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			int num = (int)context.Arguments[0];
			if (num < 1 || num > 255)
			{
				return ErrorExpressions.ValueError;
			}
			return new StringExpression(((char)num).ToString());
		}

		public static readonly string FunctionName = "CHAR";

		static readonly FunctionInfo Info;
	}
}
