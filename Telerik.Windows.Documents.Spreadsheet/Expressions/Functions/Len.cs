using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Len : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return Len.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Len.Info;
			}
		}

		static Len()
		{
			string description = "Returns the number of characters in a text string.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Len_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Text", "is the text whose length you want to find. Spaces count as characters.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_Text", "Spreadsheet_Functions_Len_TextInfo")
			};
			Len.Info = new FunctionInfo(Len.FunctionName, FunctionCategory.Text, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			return NumberExpression.CreateValidNumberOrErrorExpression((double)context.Arguments[0].Length);
		}

		public static readonly string FunctionName = "LEN";

		static readonly FunctionInfo Info;
	}
}
