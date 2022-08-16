using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public abstract class BooleansInFunction : FunctionWithSameTypeArguments<bool>
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.BoolFunctionConversion;
			}
		}
	}
}
