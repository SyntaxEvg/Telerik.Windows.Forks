using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class FormulaSelectionExpressionLexer : ExpressionLexerBase
	{
		public FormulaSelectionExpressionLexer(string input, SpreadsheetCultureHelper cultureInfo)
			: base(input, cultureInfo)
		{
			base.RegisterModificationHandler(ExpressionTokenType.A1CellReference, new ExpressionTokenHandler(base.CellReferenceRangeHandler));
		}

		protected override bool ShouldBreakOnResult(ParseResult result)
		{
			return false;
		}

		public IEnumerable<ExpressionToken> GetTokens()
		{
			ExpressionToken token;
			base.Read(out token);
			while (token != null)
			{
				yield return token;
				base.Read(out token);
			}
			yield break;
		}
	}
}
