using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class DefinedNameExpressionToken : WorksheetDependentExpressionToken
	{
		public DefinedNameExpressionToken(string worksheetName, string variableName)
			: base(ExpressionTokenType.DefinedName, worksheetName, variableName)
		{
		}
	}
}
