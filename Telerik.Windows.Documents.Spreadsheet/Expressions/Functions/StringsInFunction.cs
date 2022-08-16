using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class StringsInFunction : FunctionWithSameTypeArguments<string>
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NumberFunctionConversion;
			}
		}
	}
}
