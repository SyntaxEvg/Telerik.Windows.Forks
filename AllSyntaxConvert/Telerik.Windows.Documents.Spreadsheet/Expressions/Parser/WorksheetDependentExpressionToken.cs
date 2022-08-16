using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class WorksheetDependentExpressionToken : ExpressionToken
	{
		public string WorksheetName
		{
			get
			{
				return this.worksheetName;
			}
		}

		public string CellOrName
		{
			get
			{
				return this.cellOrName;
			}
		}

		public WorksheetDependentExpressionToken(ExpressionTokenType expressionTokenType, string worksheetName, string cellOrVariableName)
			: base(expressionTokenType, string.IsNullOrEmpty(worksheetName) ? cellOrVariableName : (worksheetName + "!" + cellOrVariableName))
		{
			Guard.ThrowExceptionIfNull<string>(worksheetName, "worksheetName");
			Guard.ThrowExceptionIfNullOrEmpty(cellOrVariableName, "cellOrVariableName");
			this.worksheetName = worksheetName;
			this.cellOrName = cellOrVariableName;
		}

		readonly string worksheetName;

		readonly string cellOrName;
	}
}
