using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ExpressionLexer : ExpressionLexerBase
	{
		public ExpressionLexer(string input, SpreadsheetCultureHelper cultureInfo)
			: base(input, cultureInfo)
		{
			base.RegisterModificationHandler(ExpressionTokenType.A1CellReference, new ExpressionTokenHandler(base.CellReferenceRangeHandler));
			base.RegisterModificationHandler(ExpressionTokenType.Space, new ExpressionTokenHandler(this.SpaceTokenIntersectionOperatorHandler));
			base.RegisterModificationHandler(ExpressionTokenType.ListSeparator, new ExpressionTokenHandler(this.ListSeparatorUnionOperatorHandler));
			base.RegisterModificationHandler(ExpressionTokenType.Plus, new ExpressionTokenHandler(this.UnaryPlusMinusOperatorsHandler));
			base.RegisterModificationHandler(ExpressionTokenType.Minus, new ExpressionTokenHandler(this.UnaryPlusMinusOperatorsHandler));
			base.RegisterValidationHandler(new Func<ExpressionToken, ParseResult>(this.ValidateBinaryOperator));
			base.RegisterValidationHandler(new Func<ExpressionToken, ParseResult>(this.ValidatePostfixOperator));
		}

		ParseResult ListSeparatorUnionOperatorHandler(ExpressionToken token, out ExpressionToken modifiedToken)
		{
			modifiedToken = token;
			if (base.Context.IsUnionOperator)
			{
				modifiedToken = new ExpressionToken(ExpressionTokenType.Union, base.SpreadsheetCultureInfo.ListSeparator);
			}
			return ParseResult.Successful;
		}

		ParseResult SpaceTokenIntersectionOperatorHandler(ExpressionToken token, out ExpressionToken modifiedToken)
		{
			modifiedToken = token;
			if (this.IsIntersectionOperator())
			{
				modifiedToken = new ExpressionToken(ExpressionTokenType.Intersection, token.Value);
			}
			return ParseResult.Successful;
		}

		ParseResult UnaryPlusMinusOperatorsHandler(ExpressionToken token, out ExpressionToken modifiedToken)
		{
			modifiedToken = token;
			if (this.IsValidUnaryPlusMinusOperator())
			{
				if (token.TokenType == ExpressionTokenType.Minus)
				{
					modifiedToken = ExpressionToken.UnaryMinus;
				}
				else
				{
					modifiedToken = ExpressionToken.UnaryPlus;
				}
			}
			return ParseResult.Successful;
		}

		ParseResult ValidateBinaryOperator(ExpressionToken token)
		{
			if (OperatorInfos.IsBinaryOperator(token.TokenType) && (base.PreviousNonSpaceToken == null || !this.IsValidBinaryOperator()))
			{
				return ParseResult.Error;
			}
			return ParseResult.Successful;
		}

		ParseResult ValidatePostfixOperator(ExpressionToken token)
		{
			if (token.TokenType == ExpressionTokenType.Percent && !this.IsValidPostFixOperator())
			{
				return ParseResult.Error;
			}
			return ParseResult.Successful;
		}

		bool IsIntersectionOperator()
		{
			ExpressionToken expressionToken;
			return base.PreviousNonSpaceToken != null && base.PeekToken(out expressionToken) == ParseResult.Successful && !OperatorInfos.IsOperator(base.PreviousNonSpaceToken.TokenType) && !OperatorInfos.IsOperator(expressionToken.TokenType) && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.ListSeparator && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Number && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Text && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Array && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.LeftParenthesis && expressionToken.TokenType != ExpressionTokenType.ListSeparator && expressionToken.TokenType != ExpressionTokenType.Number && expressionToken.TokenType != ExpressionTokenType.Text && expressionToken.TokenType != ExpressionTokenType.Array && expressionToken.TokenType != ExpressionTokenType.RightParenthesis;
		}

		bool IsValidBinaryOperator()
		{
			ExpressionToken expressionToken;
			return base.PeekToken(out expressionToken) == ParseResult.Successful && expressionToken != null && (expressionToken.TokenType != ExpressionTokenType.ListSeparator && expressionToken.TokenType != ExpressionTokenType.RightParenthesis && expressionToken.TokenType != ExpressionTokenType.Multiply && expressionToken.TokenType != ExpressionTokenType.Divide && expressionToken.TokenType != ExpressionTokenType.Power && expressionToken.TokenType != ExpressionTokenType.Ampersand) && expressionToken.TokenType != ExpressionTokenType.Range;
		}

		bool IsValidPostFixOperator()
		{
			return base.PreviousNonSpaceToken != null && !this.IsValidUnaryOperatorPrecedingToken();
		}

		bool IsValidUnaryPlusMinusOperator()
		{
			return base.PreviousNonSpaceToken == null || this.IsValidUnaryOperatorPrecedingToken();
		}

		bool IsValidUnaryOperatorPrecedingToken()
		{
			return base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Number && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Array && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Error && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.True && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.False && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Text && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.A1CellReference && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.A1CellReferenceRange && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.RightParenthesis && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.DefinedName && base.PreviousNonSpaceToken.TokenType != ExpressionTokenType.Percent;
		}

		protected override bool ShouldBreakOnResult(ParseResult result)
		{
			return result == ParseResult.Error;
		}
	}
}
