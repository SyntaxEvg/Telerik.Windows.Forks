using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class NumbersInFunction : FunctionWithSameTypeArguments<double>
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
