using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	class FieldTokenizer
	{
		public FieldTokenizer(FieldCharacter start, FieldCharacter end)
		{
			this.tokenReader = new TokenStringReader(start, end);
			this.tokenHandlers = new List<Func<FieldToken>>();
			this.tokenHandlers.Add(() => this.ReadSwitchToken());
			this.tokenHandlers.Add(() => this.ReadArgumentToken());
			this.tokenHandlers.Add(() => this.ReadWhiteSpaceToken());
			this.tokenHandlers.Add(() => this.ReadExpressionStart());
		}

		public FieldToken Read(bool skipWhitesapce = true)
		{
			FieldToken fieldToken = this.ReadInternal();
			if (skipWhitesapce)
			{
				while (fieldToken != null && fieldToken.TokenType == FieldTokenType.WhiteSpace)
				{
					fieldToken = this.ReadInternal();
				}
			}
			return fieldToken;
		}

		static bool IsValidSwitchChar(char nextChar)
		{
			return (nextChar >= 'a' && nextChar <= 'z') || FieldTokenizer.IsCharEqualTo(nextChar, new char[]
			{
				FieldParserConstants.DateFormattingSwitch,
				FieldParserConstants.NumericFormattingSwitch,
				FieldParserConstants.GeneralFormattingSwitch,
				FieldParserConstants.PreventUpdateSwitch
			});
		}

		static bool IsCharEqualTo(char character, params char[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (character == values[i])
				{
					return true;
				}
			}
			return false;
		}

		FieldToken ReadInternal()
		{
			FieldToken fieldToken = null;
			if (this.tokenReader.EndOfFile)
			{
				return null;
			}
			foreach (Func<FieldToken> func in this.tokenHandlers)
			{
				fieldToken = func();
				if (fieldToken != null)
				{
					break;
				}
			}
			if (this.isFirstNonWhitespaceToken && fieldToken != null && fieldToken.TokenType != FieldTokenType.WhiteSpace)
			{
				this.isFirstNonWhitespaceToken = false;
			}
			if (!this.tokenReader.EndOfFile && fieldToken == null)
			{
				fieldToken = new FieldToken(FieldTokenType.Error, string.Empty, false);
				this.tokenReader.MoveToEnd();
			}
			return fieldToken;
		}

		FieldToken ReadSwitchToken()
		{
			FieldToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (FieldTokenizer.IsCharEqualTo(c, new char[] { FieldParserConstants.Backslash }))
			{
				this.tokenReader.Read();
				c = (char)this.tokenReader.Peek();
				StringBuilder stringBuilder = new StringBuilder();
				while (FieldTokenizer.IsValidSwitchChar(c))
				{
					this.tokenReader.Read();
					stringBuilder.Append(c);
					c = (char)this.tokenReader.Peek();
				}
				result = new FieldToken(FieldTokenType.Switch, stringBuilder.ToString(), false);
			}
			return result;
		}

		FieldToken ReadArgumentToken()
		{
			FieldToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (FieldTokenizer.IsCharEqualTo(c, new char[] { FieldParserConstants.Quote }))
			{
				this.tokenReader.Read();
				string value = this.ReadArgumentEscapedSequence();
				result = new FieldToken(FieldTokenType.Argument, value, true);
			}
			else if (this.IsValidNonEscapedArgumentChar(c))
			{
				string value2 = this.ReadArgumentNonEscapedSequence();
				result = new FieldToken(FieldTokenType.Argument, value2, false);
			}
			return result;
		}

		FieldToken ReadWhiteSpaceToken()
		{
			FieldToken result = null;
			char c = (char)this.tokenReader.Peek();
			if (char.IsWhiteSpace(c))
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (!this.tokenReader.EndOfFile && char.IsWhiteSpace(c))
				{
					stringBuilder.Append(c);
					this.tokenReader.Read();
					c = (char)this.tokenReader.Peek();
				}
				result = new FieldToken(FieldTokenType.WhiteSpace, stringBuilder.ToString(), false);
			}
			return result;
		}

		FieldToken ReadExpressionStart()
		{
			FieldToken result = null;
			if (this.isFirstNonWhitespaceToken)
			{
				char character = (char)this.tokenReader.Peek();
				if (FieldTokenizer.IsCharEqualTo(character, new char[] { FieldParserConstants.ExpressionStart }))
				{
					this.tokenReader.Read();
					result = new FieldToken(FieldTokenType.ExpressionStart, character.ToString(), false);
				}
			}
			return result;
		}

		string ReadArgumentNonEscapedSequence()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char c = (char)this.tokenReader.Peek();
			while (!this.tokenReader.EndOfFile && this.IsValidNonEscapedArgumentChar(c))
			{
				stringBuilder.Append(c);
				this.tokenReader.Read();
				c = (char)this.tokenReader.Peek();
			}
			return stringBuilder.ToString();
		}

		string ReadArgumentEscapedSequence()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			while (!this.tokenReader.EndOfFile)
			{
				char c = (char)this.tokenReader.Read();
				if (c == FieldParserConstants.Quote && !flag)
				{
					break;
				}
				if (c == FieldParserConstants.Backslash)
				{
					if (flag)
					{
						stringBuilder.Append(c);
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					stringBuilder.Append(c);
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}

		bool IsValidNonEscapedArgumentChar(char nextChar)
		{
			return (!this.isFirstNonWhitespaceToken || !FieldTokenizer.IsCharEqualTo(nextChar, new char[] { FieldParserConstants.ExpressionStart })) && !char.IsWhiteSpace(nextChar) && !FieldTokenizer.IsCharEqualTo(nextChar, new char[] { FieldParserConstants.Quote });
		}

		readonly TokenStringReader tokenReader;

		readonly List<Func<FieldToken>> tokenHandlers;

		bool isFirstNonWhitespaceToken = true;
	}
}
