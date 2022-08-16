using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ExpressionTokenizer
	{
		static ExpressionTokenizer()
		{
			ExpressionTokenizer.stringToOneCharExpressionToken = new Dictionary<string, ExpressionToken>();
			ExpressionTokenizer.stringToOneCharExpressionToken.Add(ExpressionToken.Percent.Value, ExpressionToken.Percent);
			ExpressionTokenizer.stringToOneCharExpressionToken.Add(ExpressionToken.LeftParenthesis.Value, ExpressionToken.LeftParenthesis);
			ExpressionTokenizer.stringToOneCharExpressionToken.Add(ExpressionToken.RightParenthesis.Value, ExpressionToken.RightParenthesis);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken = new Dictionary<string, ExpressionToken>();
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Plus.Value, ExpressionToken.Plus);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Minus.Value, ExpressionToken.Minus);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Multiply.Value, ExpressionToken.Multiply);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Divide.Value, ExpressionToken.Divide);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Power.Value, ExpressionToken.Power);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Ampersand.Value, ExpressionToken.Ampersand);
			ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.Add(ExpressionToken.Range.Value, ExpressionToken.Range);
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken = new Dictionary<string, ExpressionToken>();
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken.Add(ExpressionToken.Equal.Value, ExpressionToken.Equal);
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken.Add(ExpressionToken.NotEqual.Value, ExpressionToken.NotEqual);
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken.Add(ExpressionToken.GreaterThan.Value, ExpressionToken.GreaterThan);
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken.Add(ExpressionToken.GreaterThanOrEqualTo.Value, ExpressionToken.GreaterThanOrEqualTo);
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken.Add(ExpressionToken.LessThan.Value, ExpressionToken.LessThan);
			ExpressionTokenizer.stringToComparisonOperatorExpressionToken.Add(ExpressionToken.LessThanOrEqualTo.Value, ExpressionToken.LessThanOrEqualTo);
			ExpressionTokenizer.stringToErrorExpressionToken = new Dictionary<string, ExpressionToken>();
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.NullError.Value, ExpressionToken.NullError);
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.DivisionByZeroError.Value, ExpressionToken.DivisionByZeroError);
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.ValueError.Value, ExpressionToken.ValueError);
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.ReferenceError.Value, ExpressionToken.ReferenceError);
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.NameError.Value, ExpressionToken.NameError);
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.NumberError.Value, ExpressionToken.NumberError);
			ExpressionTokenizer.stringToErrorExpressionToken.Add(ExpressionToken.NotAvailableError.Value, ExpressionToken.NotAvailableError);
		}

		public ExpressionTokenizer(string input, SpreadsheetCultureHelper spreadsheetCultureInfo)
		{
			Guard.ThrowExceptionIfNull<SpreadsheetCultureHelper>(spreadsheetCultureInfo, "spreadsheetCultureInfo");
			Guard.ThrowExceptionIfNullOrEmpty(input, "input");
			this.spreadsheetCultureInfo = spreadsheetCultureInfo;
			this.tokenReader = new ExpressionTokenReader(input);
			this.tokenHandlers = new List<ExpressionTokenizer.ExpressionTokenHandler>();
			this.textTokenHandlers = new List<ExpressionTokenizer.TextExpressionTokenHandler>();
			this.arrayTokenHandlers = new List<ExpressionTokenizer.ExpressionTokenHandler>();
			this.cellReferenceTokenHandlers = new List<ExpressionTokenizer.TextExpressionTokenHandler>();
			this.numberParseHandlers = new List<ExpressionTokenizer.NumberParserHandler>();
			this.InitTokenHandlers();
		}

		void InitTokenHandlers()
		{
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadSpaceToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadNumberToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadOneCharToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadArithmeticOperatorToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadComparisonOperatorToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadListSeparatorToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadErrorToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadTextToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadArrayToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadEscapedCellReferenceToken));
			this.tokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadFunctionCellReferenceOrVariableToken));
			this.textTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadLiteralToken));
			this.textTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadFunctionToken));
			this.textTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadCellReferenceToken));
			this.cellReferenceTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadWorksheetRefErrorCellReference));
			this.cellReferenceTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadWorksheetCellNameOrVariable));
			this.cellReferenceTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadCellNameOrVariable));
			this.cellReferenceTokenHandlers.Add(new ExpressionTokenizer.TextExpressionTokenHandler(this.ReadImportedVariable));
			this.arrayTokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadNumberToken));
			this.arrayTokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadSignedNumberToken));
			this.arrayTokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadErrorToken));
			this.arrayTokenHandlers.Add(new ExpressionTokenizer.ExpressionTokenHandler(this.ReadArrayTextToken));
			this.numberParseHandlers.Add(new ExpressionTokenizer.NumberParserHandler(this.spreadsheetCultureInfo.TryParseNumber));
			this.numberParseHandlers.Add(new ExpressionTokenizer.NumberParserHandler(this.spreadsheetCultureInfo.TryParseDouble));
			this.numberParseHandlers.Add(new ExpressionTokenizer.NumberParserHandler(this.spreadsheetCultureInfo.TryParseScientific));
		}

		public ParseResult Read(out ExpressionToken resultToken)
		{
			resultToken = null;
			if (this.tokenReader.EndOfFile)
			{
				return ParseResult.Unsuccessful;
			}
			foreach (ExpressionTokenizer.ExpressionTokenHandler expressionTokenHandler in this.tokenHandlers)
			{
				ParseResult parseResult = expressionTokenHandler(out resultToken);
				if (parseResult != ParseResult.Unsuccessful)
				{
					return parseResult;
				}
			}
			if (!this.tokenReader.EndOfFile && resultToken == null)
			{
				return ParseResult.Error;
			}
			return ParseResult.Unsuccessful;
		}

		char ReadAppendAndPeekNextChar(char nextChar, StringBuilder builder)
		{
			this.tokenReader.Read();
			builder.Append(nextChar);
			return (char)this.tokenReader.Peek();
		}

		ParseResult ReadSpaceToken(out ExpressionToken token)
		{
			token = null;
			char c = (char)this.tokenReader.Peek();
			if (SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { " " }))
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (!this.tokenReader.EndOfFile && SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { " " }))
				{
					stringBuilder.Append(c);
					this.tokenReader.Read();
					c = (char)this.tokenReader.Peek();
				}
				token = new ExpressionToken(ExpressionTokenType.Space, stringBuilder.ToString());
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadNumberToken(out ExpressionToken token)
		{
			token = null;
			double value;
			ParseResult parseResult = this.TryReadNumber(out value);
			if (parseResult == ParseResult.Successful)
			{
				string valueAsString = value.ToString(this.spreadsheetCultureInfo.CultureInfo);
				token = new ExpressionToken(value, valueAsString);
			}
			return parseResult;
		}

		ParseResult ReadOneCharToken(out ExpressionToken token)
		{
			token = null;
			char c = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.stringToOneCharExpressionToken.TryGetValue(c.ToString(), out token))
			{
				this.tokenReader.Read();
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadListSeparatorToken(out ExpressionToken token)
		{
			token = null;
			char ch = (char)this.tokenReader.Peek();
			if (SpreadsheetCultureHelper.IsCharEqualTo(ch, new string[] { this.spreadsheetCultureInfo.ListSeparator }))
			{
				token = new ExpressionToken(ExpressionTokenType.ListSeparator, this.spreadsheetCultureInfo.ListSeparator);
				this.tokenReader.Read();
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadArithmeticOperatorToken(out ExpressionToken token)
		{
			token = null;
			char c = (char)this.tokenReader.Peek();
			if (!ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.TryGetValue(c.ToString(), out token))
			{
				return ParseResult.Unsuccessful;
			}
			this.tokenReader.Read();
			if (!this.IsValidBinaryOperator())
			{
				return ParseResult.Error;
			}
			return ParseResult.Successful;
		}

		ParseResult ReadComparisonOperatorToken(out ExpressionToken token)
		{
			token = null;
			char c = (char)this.tokenReader.Peek();
			if (!ExpressionTokenizer.IsComparisonOperatorFirstChar(c))
			{
				return ParseResult.Unsuccessful;
			}
			StringBuilder stringBuilder = new StringBuilder();
			c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
			if (ExpressionTokenizer.IsComparisonOperatorSecondChar(stringBuilder[0], c))
			{
				stringBuilder.Append(c);
				this.tokenReader.Read();
			}
			if (ExpressionTokenizer.stringToComparisonOperatorExpressionToken.TryGetValue(stringBuilder.ToString(), out token) && this.IsValidBinaryOperator())
			{
				return ParseResult.Successful;
			}
			return ParseResult.Error;
		}

		ParseResult ReadErrorToken(out ExpressionToken token)
		{
			token = null;
			char c = (char)this.tokenReader.Peek();
			if (!SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { "#" }))
			{
				return ParseResult.Unsuccessful;
			}
			StringBuilder stringBuilder = new StringBuilder();
			c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
			bool flag = true;
			while (flag)
			{
				c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
				flag = ExpressionTokenizer.IsValidErrorChar(c, stringBuilder.ToString());
			}
			if (!ExpressionTokenizer.stringToErrorExpressionToken.TryGetValue(stringBuilder.ToString().ToUpper(), out token))
			{
				return ParseResult.Error;
			}
			return ParseResult.Successful;
		}

		ParseResult ReadTextToken(out ExpressionToken token)
		{
			token = null;
			string value;
			ParseResult parseResult = this.TryReadEscapedValue(this.spreadsheetCultureInfo.TextQualifier, out value);
			if (parseResult == ParseResult.Successful)
			{
				token = new ExpressionToken(ExpressionTokenType.Text, value);
			}
			return parseResult;
		}

		ParseResult ReadArrayTextToken(out ExpressionToken token)
		{
			token = null;
			string value;
			ParseResult parseResult = this.TryReadEscapedValue(this.spreadsheetCultureInfo.TextQualifier, out value);
			if (parseResult == ParseResult.Successful)
			{
				string value2 = TextHelper.DecodeValue(value, this.spreadsheetCultureInfo.TextQualifier);
				token = new ExpressionToken(ExpressionTokenType.Text, value2);
			}
			return parseResult;
		}

		ParseResult ReadArrayToken(out ExpressionToken token)
		{
			token = null;
			char c = (char)this.tokenReader.Peek();
			if (!SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { "{" }))
			{
				return ParseResult.Unsuccessful;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(c);
			this.tokenReader.Read();
			c = (char)this.tokenReader.Peek();
			List<List<ExpressionToken>> list = new List<List<ExpressionToken>>();
			list.Add(new List<ExpressionToken>());
			bool flag = false;
			while (!this.tokenReader.EndOfFile && !SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { "}" }))
			{
				stringBuilder.Append(this.ReadArrayWhitespace());
				ExpressionToken expressionToken;
				if (this.ReadArrayValue(out expressionToken))
				{
					return ParseResult.Error;
				}
				if (expressionToken.TokenType == ExpressionTokenType.Text)
				{
					stringBuilder.Append(TextHelper.GetEscapedTextValue(expressionToken.Value, "\""));
				}
				else
				{
					stringBuilder.Append(expressionToken.Value);
				}
				stringBuilder.Append(this.ReadArrayWhitespace());
				c = (char)this.tokenReader.Peek();
				if (!this.IsValidArrayChar(c))
				{
					return ParseResult.Error;
				}
				stringBuilder.Append(c);
				this.tokenReader.Read();
				if (flag)
				{
					list.Add(new List<ExpressionToken>());
				}
				list[list.Count - 1].Add(expressionToken);
				flag = SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { this.spreadsheetCultureInfo.ArrayRowSeparator });
			}
			ExpressionToken[,] array;
			if (ExpressionTokenizer.CreateArrayToken(list, out array))
			{
				return ParseResult.Error;
			}
			token = new ArrayExpressionToken(array, stringBuilder.ToString());
			return ParseResult.Successful;
		}

		string ReadArrayWhitespace()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char c = (char)this.tokenReader.Peek();
			while (char.IsWhiteSpace(c))
			{
				stringBuilder.Append(c);
				this.tokenReader.Read();
				c = (char)this.tokenReader.Peek();
			}
			return stringBuilder.ToString();
		}

		ParseResult ReadEscapedCellReferenceToken(out ExpressionToken token)
		{
			token = null;
			string value;
			ParseResult parseResult = this.TryReadEscapedValue("'", out value);
			if (parseResult == ParseResult.Successful)
			{
				StringBuilder stringBuilder = new StringBuilder(value);
				char c = (char)this.tokenReader.Peek();
				bool flag = false;
				while (c.IsValidFunctionOrCellReferenceNamePart() || flag)
				{
					c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
					flag = SpreadsheetCultureHelper.IsCharEqualTo(stringBuilder[stringBuilder.Length - 1], new string[] { "!" }) && SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { "#" });
				}
				if (this.ReadCellReferenceToken(stringBuilder.ToString(), out token) == ParseResult.Error || token == null)
				{
					return ParseResult.Error;
				}
			}
			return parseResult;
		}

		ParseResult ReadFunctionCellReferenceOrVariableToken(out ExpressionToken token)
		{
			token = null;
			char nextChar = (char)this.tokenReader.Peek();
			if (ExpressionTokenizer.IsValidFunctionChar(nextChar) || ExpressionTokenizer.IsValidNonEscapedCellReferenceChar(nextChar) || this.IsValidVariableChar(nextChar))
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				while (!this.tokenReader.EndOfFile && flag)
				{
					nextChar = this.ReadAppendAndPeekNextChar(nextChar, stringBuilder);
					flag = this.IsValidFunctionCellReferenceOrVariableChar(nextChar);
				}
				string text = stringBuilder.ToString();
				if (string.IsNullOrEmpty(text))
				{
					return ParseResult.Error;
				}
				foreach (ExpressionTokenizer.TextExpressionTokenHandler textExpressionTokenHandler in this.textTokenHandlers)
				{
					ParseResult parseResult = textExpressionTokenHandler(text, out token);
					if (parseResult != ParseResult.Unsuccessful)
					{
						return parseResult;
					}
				}
				if (token == null)
				{
					return ParseResult.Error;
				}
				return ParseResult.Unsuccessful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadLiteralToken(string value, out ExpressionToken token)
		{
			token = null;
			ParseResult result = ParseResult.Unsuccessful;
			if (SpreadsheetCultureHelper.IsLiteral(value, new string[] { "TRUE" }))
			{
				token = ExpressionToken.True;
				result = ParseResult.Successful;
			}
			else if (SpreadsheetCultureHelper.IsLiteral(value, new string[] { "FALSE" }))
			{
				token = ExpressionToken.False;
				result = ParseResult.Successful;
			}
			return result;
		}

		ParseResult ReadFunctionToken(string value, out ExpressionToken token)
		{
			token = null;
			char ch = (char)this.tokenReader.Peek();
			if (SpreadsheetCultureHelper.IsCharEqualTo(ch, new string[] { "(" }) && ExpressionTokenizer.IsValidFunctionName(value))
			{
				token = new ExpressionToken(ExpressionTokenType.Function, value);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadCellReferenceToken(string value, out ExpressionToken token)
		{
			token = null;
			foreach (ExpressionTokenizer.TextExpressionTokenHandler textExpressionTokenHandler in this.cellReferenceTokenHandlers)
			{
				ParseResult parseResult = textExpressionTokenHandler(value, out token);
				if (parseResult != ParseResult.Unsuccessful)
				{
					return parseResult;
				}
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadWorksheetRefErrorCellReference(string value, out ExpressionToken token)
		{
			token = null;
			if (!value.EndsWith("#REF!", StringComparison.CurrentCultureIgnoreCase))
			{
				return ParseResult.Unsuccessful;
			}
			string text = "#REF!";
			string text2 = value.Substring(0, value.Length - text.Length);
			if (string.IsNullOrEmpty(text2) || !text2.EndsWith("!"))
			{
				return ParseResult.Error;
			}
			string text3 = text2.Substring(0, text2.Length - 1);
			if (string.IsNullOrEmpty(text3) || !SheetCollection.IsValidSheetName(TextHelper.DecodeWorksheetName(text3)) || !ExpressionTokenizer.IsWorksheetNameEscapedCorrectly(text3))
			{
				return ParseResult.Error;
			}
			token = new CellReferenceExpressionToken(text3, text);
			return ParseResult.Successful;
		}

		ParseResult ReadWorksheetCellNameOrVariable(string value, out ExpressionToken token)
		{
			token = null;
			if (value.Contains("!"))
			{
				int num = value.LastIndexOf("!");
				string text = ((num == value.Length - 1) ? string.Empty : value.Substring(num + 1));
				string text2 = ((num == 0) ? string.Empty : value.Substring(0, num));
				if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text))
				{
					return ParseResult.Error;
				}
				string sheetName = TextHelper.DecodeWorksheetName(text2);
				if (SheetCollection.IsValidSheetName(sheetName) && ExpressionTokenizer.IsWorksheetNameEscapedCorrectly(text2))
				{
					if (NameConverter.IsValidA1CellName(text))
					{
						token = new CellReferenceExpressionToken(text2, text);
						return ParseResult.Successful;
					}
					if (DefinedName.IsNameValid(text))
					{
						token = new DefinedNameExpressionToken(text2, text);
						return ParseResult.Successful;
					}
				}
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadCellNameOrVariable(string value, out ExpressionToken token)
		{
			token = null;
			ParseResult result = ParseResult.Unsuccessful;
			if (NameConverter.IsValidA1CellName(value))
			{
				token = new CellReferenceExpressionToken(string.Empty, value);
				result = ParseResult.Successful;
			}
			else if (DefinedName.IsNameValid(value))
			{
				token = new DefinedNameExpressionToken(string.Empty, value);
				result = ParseResult.Successful;
			}
			return result;
		}

		ParseResult ReadImportedVariable(string value, out ExpressionToken token)
		{
			token = null;
			if (value.StartsWith("[") && value.Contains("]"))
			{
				if (value.StartsWith(ExpressionTokenizer.globalNamesImportString))
				{
					string variableName = value.Substring(ExpressionTokenizer.globalNamesImportString.Length);
					token = new DefinedNameExpressionToken("Book1", variableName);
					return ParseResult.Successful;
				}
				int num = value.IndexOf("]") + 1;
				if (num < value.Length)
				{
					string value2 = value.Substring(num);
					return this.ReadWorksheetCellNameOrVariable(value2, out token);
				}
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ReadSignedNumberToken(out ExpressionToken token)
		{
			token = null;
			char ch = (char)this.tokenReader.Peek();
			if (SpreadsheetCultureHelper.IsCharEqualTo(ch, new string[] { "-", "+" }))
			{
				int num = (SpreadsheetCultureHelper.IsCharEqualTo(ch, new string[] { "-" }) ? (-1) : 1);
				this.tokenReader.Read();
				double num2;
				ParseResult parseResult = this.TryReadNumber(out num2);
				if (parseResult == ParseResult.Successful)
				{
					num2 *= (double)num;
					string valueAsString = num2.ToString(this.spreadsheetCultureInfo.CultureInfo);
					token = new ExpressionToken(num2, valueAsString);
				}
				return parseResult;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult TryReadNumber(out double numberResult)
		{
			numberResult = 0.0;
			char c = (char)this.tokenReader.Peek();
			if (!char.IsDigit(c) && !this.spreadsheetCultureInfo.IsDecimalSeparator(c))
			{
				return ParseResult.Unsuccessful;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (char.IsDigit(c) || this.spreadsheetCultureInfo.IsDecimalSeparator(c))
			{
				c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
			}
			string value = this.ReadExponent();
			stringBuilder.Append(value);
			if (!this.TryParseNumber(stringBuilder.ToString(), out numberResult))
			{
				return ParseResult.Error;
			}
			return ParseResult.Successful;
		}

		string ReadExponent()
		{
			string result = string.Empty;
			char c = (char)this.tokenReader.Peek();
			if (SpreadsheetCultureHelper.IsExponent(c))
			{
				StringBuilder stringBuilder = new StringBuilder();
				c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
				if (SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { "+", "-" }))
				{
					c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
				}
				while (char.IsDigit(c))
				{
					c = this.ReadAppendAndPeekNextChar(c, stringBuilder);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		bool TryParseNumber(string input, out double number)
		{
			number = 0.0;
			foreach (ExpressionTokenizer.NumberParserHandler numberParserHandler in this.numberParseHandlers)
			{
				bool flag = numberParserHandler(input, out number);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		bool IsValidBinaryOperator()
		{
			if (this.tokenReader.EndOfFile)
			{
				return false;
			}
			char ch = (char)this.tokenReader.Peek();
			return !SpreadsheetCultureHelper.IsCharEqualTo(ch, new string[]
			{
				this.spreadsheetCultureInfo.ListSeparator,
				")",
				"*",
				"/",
				"^",
				"&",
				":"
			});
		}

		public static bool IsComparisonOperatorFirstChar(char firstChar)
		{
			return SpreadsheetCultureHelper.IsCharEqualTo(firstChar, new string[] { "<", ">", "=" });
		}

		static bool IsComparisonOperatorSecondChar(char firstChar, char secondChar)
		{
			return (firstChar != "="[0] && secondChar == "="[0]) || (firstChar == "<"[0] && secondChar == ">"[0]);
		}

		ParseResult TryReadEscapedValue(string escapeSymbol, out string escapedValue)
		{
			escapedValue = string.Empty;
			char c = (char)this.tokenReader.Peek();
			if (!SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { escapeSymbol }))
			{
				return ParseResult.Unsuccessful;
			}
			StringBuilder stringBuilder = new StringBuilder();
			this.tokenReader.Read();
			stringBuilder.Append(c);
			bool flag = false;
			bool flag2 = true;
			c = (char)this.tokenReader.Peek();
			while (this.IsValidEscapedValueChar(c, flag, flag2, escapeSymbol))
			{
				this.tokenReader.Read();
				stringBuilder.Append(c);
				flag2 = SpreadsheetCultureHelper.IsCharEqualTo(c, new string[] { escapeSymbol });
				if (flag2)
				{
					flag = !flag;
				}
				c = (char)this.tokenReader.Peek();
			}
			escapedValue = stringBuilder.ToString();
			if (!flag)
			{
				return ParseResult.Error;
			}
			return ParseResult.Successful;
		}

		bool IsValidEscapedValueChar(char character, bool isQuotesNumberEven, bool isPreviousCharQuote, string escapeSymbol)
		{
			bool result = true;
			bool flag = SpreadsheetCultureHelper.IsCharEqualTo(character, new string[] { escapeSymbol });
			if (this.tokenReader.EndOfFile)
			{
				result = false;
			}
			else if (!flag)
			{
				if (isQuotesNumberEven)
				{
					result = false;
				}
			}
			else if (flag && isQuotesNumberEven && !isPreviousCharQuote)
			{
				result = false;
			}
			return result;
		}

		static bool IsValueEscapedCorrectly(string value, char escapeSymbol)
		{
			bool result = false;
			bool flag = false;
			if (value[0] == escapeSymbol && value[value.Length - 1] == escapeSymbol)
			{
				result = true;
				string text = value.Substring(1, value.Length - 2);
				for (int i = 0; i < text.Length; i++)
				{
					bool flag2 = text[i] == escapeSymbol;
					if (!flag2 && flag)
					{
						result = false;
						break;
					}
					if (flag2)
					{
						flag = !flag;
					}
				}
			}
			return result;
		}

		static bool IsValidErrorChar(char character, string previousSymbols)
		{
			if (character == '/')
			{
				return string.Equals(previousSymbols, "#DIV", StringComparison.CurrentCultureIgnoreCase) || string.Equals(previousSymbols, "#N", StringComparison.CurrentCultureIgnoreCase);
			}
			return char.IsLetter(character) || character == '!' || character == '?' || character == '0';
		}

		bool IsValidArrayChar(char character)
		{
			return SpreadsheetCultureHelper.IsCharEqualTo(character, new string[]
			{
				this.spreadsheetCultureInfo.ArrayListSeparator,
				this.spreadsheetCultureInfo.ArrayRowSeparator,
				"}"
			});
		}

		bool ReadArrayValue(out ExpressionToken token)
		{
			token = null;
			ExpressionToken expressionToken = null;
			foreach (ExpressionTokenizer.ExpressionTokenHandler expressionTokenHandler in this.arrayTokenHandlers)
			{
				ParseResult parseResult = expressionTokenHandler(out expressionToken);
				if (parseResult == ParseResult.Error)
				{
					return true;
				}
				if (parseResult == ParseResult.Successful)
				{
					break;
				}
			}
			token = expressionToken;
			return token == null;
		}

		static bool CreateArrayToken(List<List<ExpressionToken>> arrayTokens, out ExpressionToken[,] array)
		{
			array = null;
			if (arrayTokens.Count < 1 || arrayTokens[0].Count < 1)
			{
				return true;
			}
			ExpressionToken[,] array2 = new ExpressionToken[arrayTokens.Count, arrayTokens[0].Count];
			for (int i = 0; i < arrayTokens.Count; i++)
			{
				if (arrayTokens[i].Count != arrayTokens[0].Count)
				{
					return true;
				}
				for (int j = 0; j < arrayTokens[i].Count; j++)
				{
					array2[i, j] = arrayTokens[i][j];
				}
			}
			array = array2;
			return false;
		}

		static bool IsWorksheetNameEscapedCorrectly(string worksheetName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(worksheetName, "worksheetName");
			if (!char.IsLetter(worksheetName[0]) && worksheetName[0] != '_' && worksheetName[0] != '\'')
			{
				return false;
			}
			for (int i = 0; i < worksheetName.Length; i++)
			{
				if (!char.IsLetterOrDigit(worksheetName[i]) && worksheetName[i] != '_' && worksheetName[i] != '\'' && worksheetName[i] != '.')
				{
					return ExpressionTokenizer.IsValueEscapedCorrectly(worksheetName, '\'');
				}
			}
			return true;
		}

		static bool IsValidFunctionChar(char nextChar)
		{
			return char.IsLetterOrDigit(nextChar) || SpreadsheetCultureHelper.IsCharEqualTo(nextChar, new string[] { ".", "_" });
		}

		static bool IsValidFunctionName(string function)
		{
			if (function.Length == 0 || char.IsDigit(function[0]))
			{
				return false;
			}
			for (int i = 0; i < function.Length; i++)
			{
				if (!ExpressionTokenizer.IsValidFunctionChar(function[i]))
				{
					return false;
				}
			}
			return true;
		}

		static bool IsValidNonEscapedCellReferenceChar(char nextChar)
		{
			return char.IsLetterOrDigit(nextChar) || SpreadsheetCultureHelper.IsCharEqualTo(nextChar, new string[] { "_", "#", "!", "$" });
		}

		bool IsValidFunctionCellReferenceOrVariableChar(char nextChar)
		{
			return !SpreadsheetCultureHelper.IsCharEqualTo(nextChar, new string[] { "(" }) && (ExpressionTokenizer.IsValidFunctionChar(nextChar) || ExpressionTokenizer.IsValidNonEscapedCellReferenceChar(nextChar) || this.IsValidVariableChar(nextChar));
		}

		bool IsValidVariableChar(char nextChar)
		{
			return !ExpressionTokenizer.IsComparisonOperatorFirstChar(nextChar) && !ExpressionTokenizer.stringToArithmeticOperatorExpressionToken.ContainsKey(nextChar.ToString()) && !SpreadsheetCultureHelper.IsCharEqualTo(nextChar, new string[]
			{
				this.spreadsheetCultureInfo.TextQualifier,
				this.spreadsheetCultureInfo.NumberDecimalSeparator,
				"{",
				"}",
				"(",
				")",
				"'",
				"#",
				"!",
				" ",
				"|",
				"%",
				"`",
				"~",
				";",
				"@",
				","
			});
		}

		static readonly Dictionary<string, ExpressionToken> stringToOneCharExpressionToken;

		static readonly Dictionary<string, ExpressionToken> stringToArithmeticOperatorExpressionToken;

		static readonly Dictionary<string, ExpressionToken> stringToComparisonOperatorExpressionToken;

		static readonly Dictionary<string, ExpressionToken> stringToErrorExpressionToken;

		static readonly string globalNamesImportString = "[0]!";

		readonly SpreadsheetCultureHelper spreadsheetCultureInfo;

		readonly ExpressionTokenReader tokenReader;

		readonly List<ExpressionTokenizer.ExpressionTokenHandler> tokenHandlers;

		readonly List<ExpressionTokenizer.ExpressionTokenHandler> arrayTokenHandlers;

		readonly List<ExpressionTokenizer.TextExpressionTokenHandler> textTokenHandlers;

		readonly List<ExpressionTokenizer.TextExpressionTokenHandler> cellReferenceTokenHandlers;

		readonly List<ExpressionTokenizer.NumberParserHandler> numberParseHandlers;

		delegate ParseResult ExpressionTokenHandler(out ExpressionToken token);

		delegate ParseResult TextExpressionTokenHandler(string input, out ExpressionToken token);

		delegate bool NumberParserHandler(string input, out double number);
	}
}
