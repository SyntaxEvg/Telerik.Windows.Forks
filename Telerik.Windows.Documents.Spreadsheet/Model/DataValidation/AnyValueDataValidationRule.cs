using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public sealed class AnyValueDataValidationRule : DataValidationRuleBase
	{
		public AnyValueDataValidationRule(AnyValueDataValidationRuleContext context)
			: base(context)
		{
			Guard.ThrowExceptionIfNull<AnyValueDataValidationRuleContext>(context, "context");
		}

		protected override bool EvaluateOverride(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue)
		{
			return true;
		}
	}
}
