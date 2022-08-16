using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	delegate ParseResult ExpressionTokenHandler(ExpressionToken token, out ExpressionToken modifiedToken);
}
