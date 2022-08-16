using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	abstract class ExpressionLexerBase
	{
		protected ExpressionLexerContext Context
		{
			get
			{
				return this.context;
			}
		}

		protected SpreadsheetCultureHelper SpreadsheetCultureInfo
		{
			get
			{
				return this.spreadsheetCultureInfo;
			}
		}

		protected ExpressionToken PreviousNonSpaceToken
		{
			get
			{
				return this.previousNonSpaceToken;
			}
		}

		protected ExpressionLexerBase(string input, SpreadsheetCultureHelper cultureInfo)
		{
			this.tokenizer = new ExpressionTokenizer(input, cultureInfo);
			this.peekedTokens = new List<ExpressionToken>();
			this.context = new ExpressionLexerContext();
			this.spreadsheetCultureInfo = cultureInfo;
			this.modificationHandlers = new Dictionary<ExpressionTokenType, ExpressionTokenHandler>();
			this.validationHandlers = new List<Func<ExpressionToken, ParseResult>>();
		}

		public ParseResult Read(out ExpressionToken token)
		{
			ParseResult parseResult = this.PopToken(out token);
			if (parseResult == ParseResult.Successful)
			{
				this.UpdateContext(token);
				ExpressionTokenHandler expressionTokenHandler;
				if (this.modificationHandlers.TryGetValue(token.TokenType, out expressionTokenHandler))
				{
					ExpressionToken expressionToken;
					ParseResult result = expressionTokenHandler(token, out expressionToken);
					if (this.ShouldBreakOnResult(result))
					{
						return result;
					}
					token = expressionToken;
				}
				foreach (Func<ExpressionToken, ParseResult> func in this.validationHandlers)
				{
					ParseResult result2 = func(token);
					if (this.ShouldBreakOnResult(result2))
					{
						return result2;
					}
				}
				if (this.IsMissingValueTokenRequired(token.TokenType))
				{
					this.peekedTokens.Add(token);
					token = ExpressionToken.MissingValue;
				}
				this.UpdatePreviousToken(token);
				return parseResult;
			}
			return parseResult;
		}

		protected abstract bool ShouldBreakOnResult(ParseResult result);

		protected ParseResult CellReferenceRangeHandler(ExpressionToken token, out ExpressionToken modifiedToken)
		{
			modifiedToken = token;
			ParseResult parseResult = this.IsCellReferenceRangeToken();
			if (parseResult != ParseResult.Successful)
			{
				return parseResult;
			}
			CellReferenceExpressionToken cellReferenceExpressionToken = (CellReferenceExpressionToken)token;
			CellReferenceExpressionToken cellReferenceExpressionToken2 = (CellReferenceExpressionToken)this.peekedTokens[this.peekedTokens.Count - 1];
			string text = TextHelper.DecodeWorksheetName(cellReferenceExpressionToken.WorksheetName);
			string value = TextHelper.DecodeWorksheetName(cellReferenceExpressionToken2.WorksheetName);
			if (string.IsNullOrEmpty(value) || text.Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				modifiedToken = new CellReferenceRangeExpressionToken(cellReferenceExpressionToken, cellReferenceExpressionToken2);
				this.peekedTokens.RemoveRange(0, this.peekedTokens.Count);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult IsCellReferenceRangeToken()
		{
			ParseResult parseResult = this.ReadUntilTokenOfType(ExpressionTokenType.Range);
			if (parseResult == ParseResult.Successful)
			{
				parseResult = this.ReadUntilTokenOfType(ExpressionTokenType.A1CellReference);
				if (parseResult == ParseResult.Successful)
				{
					CellReferenceExpressionToken cellReferenceExpressionToken = this.peekedTokens[this.peekedTokens.Count - 1] as CellReferenceExpressionToken;
					if (cellReferenceExpressionToken == null || !string.IsNullOrEmpty(cellReferenceExpressionToken.WorksheetName))
					{
						parseResult = ParseResult.Unsuccessful;
					}
				}
			}
			return parseResult;
		}

		ParseResult ReadUntilTokenOfType(ExpressionTokenType tokenType)
		{
			ExpressionToken expressionToken;
			ParseResult parseResult = this.tokenizer.Read(out expressionToken);
			if (parseResult != ParseResult.Successful)
			{
				return parseResult;
			}
			this.peekedTokens.Add(expressionToken);
			if (expressionToken.TokenType == ExpressionTokenType.Space)
			{
				return this.ReadUntilTokenOfType(tokenType);
			}
			if (expressionToken.TokenType != tokenType)
			{
				return ParseResult.Unsuccessful;
			}
			return ParseResult.Successful;
		}

		protected ParseResult PeekToken(out ExpressionToken nextToken)
		{
			if (this.peekedTokens.Count > 0)
			{
				nextToken = this.peekedTokens[0];
				return ParseResult.Successful;
			}
			ParseResult parseResult = this.tokenizer.Read(out nextToken);
			if (parseResult == ParseResult.Successful && nextToken != null)
			{
				this.peekedTokens.Add(nextToken);
			}
			return parseResult;
		}

		protected void RegisterModificationHandler(ExpressionTokenType tokenType, ExpressionTokenHandler modificationHandler)
		{
			Guard.ThrowExceptionIfNull<ExpressionTokenHandler>(modificationHandler, "modificationHandler");
			this.modificationHandlers.Add(tokenType, modificationHandler);
		}

		protected void RegisterValidationHandler(Func<ExpressionToken, ParseResult> validationHandler)
		{
			Guard.ThrowExceptionIfNull<Func<ExpressionToken, ParseResult>>(validationHandler, "validationHandler");
			this.validationHandlers.Add(validationHandler);
		}

		ParseResult PopToken(out ExpressionToken nextToken)
		{
			if (this.peekedTokens.Count > 0)
			{
				nextToken = this.peekedTokens[0];
				this.peekedTokens.RemoveAt(0);
				return ParseResult.Successful;
			}
			return this.tokenizer.Read(out nextToken);
		}

		void UpdateContext(ExpressionToken token)
		{
			if (token.TokenType != ExpressionTokenType.LeftParenthesis)
			{
				if (token.TokenType == ExpressionTokenType.RightParenthesis)
				{
					this.context.RegisterRightParenthesis();
				}
				return;
			}
			if (this.previousNonSpaceToken != null && this.previousNonSpaceToken.TokenType == ExpressionTokenType.Function)
			{
				this.context.RegisterFunction();
				return;
			}
			this.context.RegisterLeftParenthesis();
		}

		void UpdatePreviousToken(ExpressionToken token)
		{
			if (token.TokenType != ExpressionTokenType.Space)
			{
				this.previousNonSpaceToken = token;
			}
		}

		bool IsMissingValueTokenRequired(ExpressionTokenType tokenType)
		{
			return this.previousNonSpaceToken != null && ((this.previousNonSpaceToken.TokenType == ExpressionTokenType.LeftParenthesis && tokenType == ExpressionTokenType.ListSeparator) || (this.previousNonSpaceToken.TokenType == ExpressionTokenType.ListSeparator && tokenType == ExpressionTokenType.ListSeparator) || (this.previousNonSpaceToken.TokenType == ExpressionTokenType.ListSeparator && tokenType == ExpressionTokenType.RightParenthesis));
		}

		readonly Dictionary<ExpressionTokenType, ExpressionTokenHandler> modificationHandlers;

		readonly List<Func<ExpressionToken, ParseResult>> validationHandlers;

		readonly ExpressionTokenizer tokenizer;

		readonly ExpressionLexerContext context;

		readonly SpreadsheetCultureHelper spreadsheetCultureInfo;

		readonly List<ExpressionToken> peekedTokens;

		ExpressionToken previousNonSpaceToken;
	}
}
