using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class CellReferenceExpressionToken : WorksheetDependentExpressionToken
	{
		public CellReferenceExpressionToken(string worksheetName, string cellName)
			: base(ExpressionTokenType.A1CellReference, worksheetName, cellName)
		{
		}
	}
}
