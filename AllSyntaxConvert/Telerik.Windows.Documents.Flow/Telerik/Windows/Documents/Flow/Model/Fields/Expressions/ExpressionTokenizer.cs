using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	class ExpressionTokenizer
	{
		static ExpressionTokenizer()
		{
			ExpressionTokenizer.StringToExpressionToken.Add(ExpressionToken.LeftParenthesis.Value, ExpressionToken.LeftParenthesis);
			ExpressionTokenizer.StringToExpressionToken.Add(ExpressionToken.RightParenthesis.Value, ExpressionToken.RightParenthesis);
			ExpressionTokenizer.StringToBinaryOperatorExpressionToken = new Dictionary<string, ExpressionToken>();
			ExpressionTokenizer.StringToBinaryOperatorExpressionToken.Add(ExpressionToken.Plus.Value, ExpressionToken.Plus);
			ExpressionTokenizer.StringToBinaryOperatorExpressionToken.Add(ExpressionToken.Minus.Value, ExpressionToken.Minus);
			ExpressionTokenizer.StringToBinaryOperatorExpressionToken.Add(ExpressionToken.Multiply.Value, ExpressionToken.Multiply);
			ExpressionTokenizer.StringToBinaryOperatorExpressionToken.Add(ExpressionToken.Divide.Value, ExpressionToken.Divide);
			ExpressionTokenizer.StringToBinaryOperatorExpressionToken.Add(ExpressionToken.Power.Value, ExpressionToken.Power);
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken = new Dictionary<string, ExpressionToken>();
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken.Add(ExpressionToken.Equal.Value, ExpressionToken.Equal);
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken.Add(ExpressionToken.NotEqual.Value, ExpressionToken.NotEqual);
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken.Add(ExpressionToken.GreaterThan.Value, ExpressionToken.GreaterThan);
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken.Add(ExpressionToken.GreaterThanOrEqualTo.Value, ExpressionToken.GreaterThanOrEqualTo);
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken.Add(ExpressionToken.LessThan.Value, ExpressionToken.LessThan);
			ExpressionTokenizer.StringToComparisonOperatorExpressionToken.Add(ExpressionToken.LessThanOrEqualTo.Value, ExpressionToken.LessThanOrEqualTo);
		}

		public ExpressionTokenizer(string input)
		{
			Guard.ThrowExceptionIfNullOrEmpty(input, "input");
			this.tokenReader = new TokenStringReader(input);
			this.tokenHandlers = new List<Func<ExpressionToken>>();
			this.textTokenHandlers = new List<Func<string, ExpressionToken>>();
			this.InitTokenHandlers();
		}

		public ExpressionToken Read()
		{
			ExpressionToken expressionToken = this.ReadInternal();
			if (expressionToken != null && expressionToken.TokenType != ExpressionTokenType.Space)
			{
				this.previousNonSpaceToken = expressionToken;
			}
			return expressionToken;
		}

		static bool IsLiteral(string input, params string[] literals)
		{
			string a = input.ToUpper(CultureInfo.InvariantCulture);
			for (int i = 0; i < literals.Length; i++)
			{
				if (a == literals[i])
				{
					return true;
				}
			}
			return false;
		}

		static bool IsValidFunctionCellReferenceOrVariableChar(char nextChar)
		{
			return !ExpressionTokenizer.IsCharEqualTo(nextChar, new string[] { "(" }) && (ExpressionTokenizer.IsValidFunctionChar(nextChar) || ExpressionTokenizer.IsValidNonEscapedCellReferenceChar(nextChar) || ExpressionTokenizer.IsValidBookmarkChar(nextChar));
		}

		static bool IsValidBookmarkChar(char nextChar)
		{
			return !ExpressionTokenizer.IsComparisonOperatorFirstChar(nextChar) && !ExpressionTokenizer.StringToBinaryOperatorExpressionToken.ContainsKey(nextChar.ToString()) && !ExpressionTokenizer.StringToExpressionToken.ContainsKey(nextChar.ToString()) && !ExpressionTokenizer.IsCharEqualTo(nextChar, new string[]
			{
				ExpressionConstants.TextQualifier,
				"%",
				" ",
				".",
				"'",
				"#",
				"!",
				"|",
				"{",
				"}",
				"[",
				"]",
				"`",
				"~",
				";",
				"@",
				","
			});
		}

		static bool IsValidFunctionChar(char nextChar)
		{
			return char.IsLetter(nextChar);
		}

		static bool IsValidNonEscapedCellReferenceChar(char nextChar)
		{
			return char.IsLetterOrDigit(nextChar) || ExpressionTokenizer.IsCharEqualTo(nextChar, new string[] { "_", "#", "!", "$" });
		}

		static bool IsComparisonOperatorFirstChar(char firstChar)
		{
			return ExpressionTokenizer.IsCharEqualTo(firstChar, new string[] { "<", ">", "=" });
		}

		static bool IsComparisonOperatorSecondChar(char firstChar, char secondChar)
		{
			return (firstChar != "="[0] && secondChar == "="[0]) || (firstChar == "<"[0] && secondChar == ">"[0]);
		}

		static bool TryParseNumber(string input, out double number)
		{
			return double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out number);
		}

		static bool IsDecimalSeparator(char ch)
		{
			return ch.ToString() == ".";
		}

		static bool IsCharEqualTo(char ch, params string[] values)
		{
			string a = ch.ToString();
			for (int i = 0; i < values.Length; i++)
			{
				if (a == values[i])
				{
					return true;
				}
			}
			return false;
		}

		void InitTokenHandlers()
		{
			this.tokenHandlers.Add(() => this.ReadSpaceToken());
			this.tokenHandlers.Add(() => this.ReadNumberToken());
			this.tokenHandlers.Add(() => this.ReadUnaryPlusMinusToken());
			this.tokenHandlers.Add(() => this.ReadPercentOperatorToken());
			this.tokenHandlers.Add(() => this.ReadBinaryOperatorToken());
			this.tokenHandlers.Add(() => this.ReadParenthesisToken());
			this.tokenHandlers.Add(() => this.ReadListSeparatorToken());
			this.tokenHandlers.Add(() => this.ReadFunctionOrBookmarkToken());
			this.textTokenHandlers.Add((string value) => this.ReadLiteralToken(value));
			this.textTokenHandlers.Add((string value) => this.ReadFunctionToken(value));
			this.textTokenHandlers.Add((string value) => this.ReadBookmarkToken(value));
		}

		ExpressionToken ReadInternal()
		{
			ExpressionToken expressionToken = null;
			if (this.tokenReader.EndOfFile)
			{
				return null;
			}
			foreach (Func<ExpressionToken> func in this.tokenHandlers)
			{
				expressionToken = func();
				if (expressionToken != null)
				{
					break;
				}
			}
			if (!this.tokenReader.EndOfFile && expressionToken == null)
			{
				throw new ExpressionException("Unexpected symbol.");
			}
			return expressionToken;
		}

		ExpressionToken ReadSpaceToken()
		{
			ExpressionToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.IsCharEqualTo(c, new string[] { " " }))
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (!this.tokenReader.EndOfFile && ExpressionTokenizer.IsCharEqualTo(c, new string[] { " " }))
				{
					stringBuilder.Append(c);
					this.tokenReader.Read();
					c = (char)this.tokenReader.Peek();
				}
				result = new ExpressionToken(ExpressionTokenType.Space, stringBuilder.ToString());
			}
			return result;
		}

		ExpressionToken ReadNumberToken()
		{
			ExpressionToken result = null;
			double value;
			if (this.TryReadNumber(out value))
			{
				string valueAsString = value.ToString(CultureInfo.InvariantCulture);
				result = new ExpressionToken(value, valueAsString);
			}
			return result;
		}

		ExpressionToken ReadUnaryPlusMinusToken()
		{
			ExpressionToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (this.IsValidUnaryPlusMinusOperator() && c.ToString() == ExpressionToken.UnaryMinus.Value)
			{
				result = ExpressionToken.UnaryMinus;
				this.tokenReader.Read();
				if (this.tokenReader.EndOfFile)
				{
					throw new ExpressionException("Unexpected end of expression.");
				}
			}
			return result;
		}

		ExpressionToken ReadPercentOperatorToken()
		{
			ExpressionToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (this.IsValidPercentOperator() && c.ToString() == ExpressionToken.Percent.Value)
			{
				result = ExpressionToken.Percent;
				this.tokenReader.Read();
			}
			return result;
		}

		ExpressionToken ReadBinaryOperatorToken()
		{
			ExpressionToken expressionToken = null;
			char c = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.StringToBinaryOperatorExpressionToken.TryGetValue(c.ToString(), out expressionToken))
			{
				this.tokenReader.Read();
			}
			else if (ExpressionTokenizer.IsComparisonOperatorFirstChar(c) && !this.IsValidUnaryPlusMinusOperator())
			{
				StringBuilder stringBuilder = new StringBuilder();
				c = this.ReadAppendAndGetNextChar(c, stringBuilder);
				if (ExpressionTokenizer.IsComparisonOperatorSecondChar(stringBuilder[0], c))
				{
					stringBuilder.Append(c);
					this.tokenReader.Read();
				}
				ExpressionTokenizer.StringToComparisonOperatorExpressionToken.TryGetValue(stringBuilder.ToString(), out expressionToken);
				if (expressionToken == null)
				{
					throw new ExpressionException("Unexpected binary operator token: " + stringBuilder.ToString());
				}
			}
			if (expressionToken != null && !this.IsValidBinaryOperator())
			{
				throw new ExpressionException("Invalid binary operator usage.");
			}
			return expressionToken;
		}

		ExpressionToken ReadParenthesisToken()
		{
			ExpressionToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.StringToExpressionToken.TryGetValue(c.ToString(), out result))
			{
				this.tokenReader.Read();
			}
			return result;
		}

		ExpressionToken ReadListSeparatorToken()
		{
			ExpressionToken result = null;
			char ch = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.IsCharEqualTo(ch, new string[] { "," }))
			{
				result = new ExpressionToken(ExpressionTokenType.ListSeparator, ",");
				this.tokenReader.Read();
			}
			return result;
		}

		ExpressionToken ReadFunctionOrBookmarkToken()
		{
			ExpressionToken expressionToken = null;
			char nextChar = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.IsValidFunctionChar(nextChar) || ExpressionTokenizer.IsValidNonEscapedCellReferenceChar(nextChar) || ExpressionTokenizer.IsValidBookmarkChar(nextChar))
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				while (!this.tokenReader.EndOfFile && flag)
				{
					nextChar = this.ReadAppendAndGetNextChar(nextChar, stringBuilder);
					flag = ExpressionTokenizer.IsValidFunctionCellReferenceOrVariableChar(nextChar);
				}
				string text = stringBuilder.ToString();
				Guard.ThrowExceptionIfNullOrEmpty(text, "value");
				foreach (Func<string, ExpressionToken> func in this.textTokenHandlers)
				{
					expressionToken = func(text);
					if (expressionToken != null)
					{
						break;
					}
				}
				if (expressionToken == null)
				{
					throw new ExpressionException("Could not read function or bookmark token.");
				}
			}
			return expressionToken;
		}

		ExpressionToken ReadFunctionToken(string value)
		{
			ExpressionToken result = null;
			char ch = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.IsCharEqualTo(ch, new string[] { " " }))
			{
				this.ReadSpaceToken();
			}
			ch = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.IsCharEqualTo(ch, new string[] { "(" }))
			{
				result = new ExpressionToken(ExpressionTokenType.Function, value);
			}
			return result;
		}

		ExpressionToken ReadLiteralToken(string value)
		{
			if (ExpressionTokenizer.IsLiteral(value, new string[] { "TRUE" }))
			{
				return ExpressionToken.True;
			}
			if (ExpressionTokenizer.IsLiteral(value, new string[] { "FALSE" }))
			{
				return ExpressionToken.False;
			}
			return null;
		}

		ExpressionToken ReadBookmarkToken(string value)
		{
			ExpressionToken result = null;
			if (Bookmark.IsValidBookmarkName(value, true))
			{
				result = new ExpressionToken(ExpressionTokenType.Bookmark, value);
			}
			return result;
		}

		bool IsValidUnaryPlusMinusOperator()
		{
			return this.previousNonSpaceToken == null || this.IsValidUnaryOperatorPrecedingToken();
		}

		bool IsValidUnaryOperatorPrecedingToken()
		{
			return this.previousNonSpaceToken.TokenType != ExpressionTokenType.Number && this.previousNonSpaceToken.TokenType != ExpressionTokenType.True && this.previousNonSpaceToken.TokenType != ExpressionTokenType.False && this.previousNonSpaceToken.TokenType != ExpressionTokenType.RightParenthesis && this.previousNonSpaceToken.TokenType != ExpressionTokenType.Percent && this.previousNonSpaceToken.TokenType != ExpressionTokenType.Bookmark;
		}

		bool IsValidPercentOperator()
		{
			return this.previousNonSpaceToken != null && this.previousNonSpaceToken.TokenType != ExpressionTokenType.Bookmark && !this.IsValidUnaryOperatorPrecedingToken();
		}

		bool IsValidBinaryOperator()
		{
			if (this.tokenReader.EndOfFile || this.previousNonSpaceToken == null)
			{
				return false;
			}
			char ch = (char)this.tokenReader.Peek();
			return !ExpressionTokenizer.IsCharEqualTo(ch, new string[] { ")", "*", "/", "^" });
		}

		bool TryReadNumber(out double numberResult)
		{
			bool result = false;
			numberResult = 0.0;
			char c = (char)this.tokenReader.Peek();
			if (char.IsDigit(c) || ExpressionTokenizer.IsDecimalSeparator(c))
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (char.IsDigit(c) || ExpressionTokenizer.IsDecimalSeparator(c))
				{
					c = this.ReadAppendAndGetNextChar(c, stringBuilder);
				}
				if (!ExpressionTokenizer.TryParseNumber(stringBuilder.ToString(), out numberResult))
				{
					throw new ExpressionException("Could not parse number: " + stringBuilder.ToString());
				}
				result = true;
			}
			return result;
		}

		char ReadAppendAndGetNextChar(char nextChar, StringBuilder builder)
		{
			this.tokenReader.Read();
			builder.Append(nextChar);
			return (char)this.tokenReader.Peek();
		}

		static readonly Dictionary<string, ExpressionToken> StringToExpressionToken = new Dictionary<string, ExpressionToken>();

		static readonly Dictionary<string, ExpressionToken> StringToBinaryOperatorExpressionToken;

		static readonly Dictionary<string, ExpressionToken> StringToComparisonOperatorExpressionToken;

		readonly TokenStringReader tokenReader;

		readonly List<Func<ExpressionToken>> tokenHandlers;

		readonly List<Func<string, ExpressionToken>> textTokenHandlers;

		ExpressionToken previousNonSpaceToken;
	}
}
