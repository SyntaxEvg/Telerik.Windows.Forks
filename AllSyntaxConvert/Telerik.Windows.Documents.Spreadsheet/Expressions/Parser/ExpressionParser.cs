using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ExpressionParser
	{
		public static ExpressionParser ExpressionParserInstance { get; set; } = new ExpressionParser();

		ExpressionParser()
		{
			this.tokenHandlers = new ExpressionParser.TokenHandler[]
			{
				new ExpressionParser.TokenHandler(this.ParseLiteral),
				new ExpressionParser.TokenHandler(this.ParseNumber),
				new ExpressionParser.TokenHandler(this.ParseText),
				new ExpressionParser.TokenHandler(this.ParseError),
				new ExpressionParser.TokenHandler(this.ParseUnaryOperator),
				new ExpressionParser.TokenHandler(this.ParseBinaryOperator),
				new ExpressionParser.TokenHandler(this.ParseCellReference),
				new ExpressionParser.TokenHandler(this.ParseCellReferenceRange),
				new ExpressionParser.TokenHandler(this.ParseVariable),
				new ExpressionParser.TokenHandler(this.ParseFunction),
				new ExpressionParser.TokenHandler(this.ParseArray)
			};
			this.tokenToExpression = new Dictionary<ExpressionTokenType, Func<RadExpression>>();
			this.tokenToExpression.Add(ExpressionTokenType.True, () => BooleanExpression.True);
			this.tokenToExpression.Add(ExpressionTokenType.False, () => BooleanExpression.False);
			this.tokenToExpression.Add(ExpressionTokenType.MissingValue, () => EmptyExpression.Empty);
			this.tokenToExpression.Add(ExpressionTokenType.FunctionStart, () => new FunctionStartExpression());
			this.unaryOperatorToExpression = new Dictionary<ExpressionTokenType, Func<RadExpression, RadExpression>>();
			this.unaryOperatorToExpression.Add(ExpressionTokenType.Percent, (RadExpression p) => new PercentExpression(p));
			this.unaryOperatorToExpression.Add(ExpressionTokenType.UnaryPlus, (RadExpression p) => new UnaryPlusExpression(p));
			this.unaryOperatorToExpression.Add(ExpressionTokenType.UnaryMinus, (RadExpression p) => new UnaryMinusExpression(p));
			this.binaryOperatorToExpression = new Dictionary<ExpressionTokenType, Func<RadExpression, RadExpression, RadExpression>>();
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Plus, (RadExpression p, RadExpression q) => new AdditionExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Minus, (RadExpression p, RadExpression q) => new SubtractionExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Multiply, (RadExpression p, RadExpression q) => new MultiplicationExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Divide, (RadExpression p, RadExpression q) => new DivisionExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Power, (RadExpression p, RadExpression q) => new PowerExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Ampersand, (RadExpression p, RadExpression q) => new AmpersandExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Equal, (RadExpression p, RadExpression q) => new EqualExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.NotEqual, (RadExpression p, RadExpression q) => new NotEqualExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.GreaterThan, (RadExpression p, RadExpression q) => new GreaterThanExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.GreaterThanOrEqualTo, (RadExpression p, RadExpression q) => new GreaterThanOrEqualToExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.LessThan, (RadExpression p, RadExpression q) => new LessThanExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.LessThanOrEqualTo, (RadExpression p, RadExpression q) => new LessThanOrEqualToExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Range, (RadExpression p, RadExpression q) => new RangeExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Intersection, (RadExpression p, RadExpression q) => new IntersectionExpression(p, q));
			this.binaryOperatorToExpression.Add(ExpressionTokenType.Union, (RadExpression p, RadExpression q) => new UnionExpression(p, q));
			this.inputTokenToUserInput = new Dictionary<ExpressionTokenType, Func<ExpressionParser.InputStringToken, UserInputStringBase>>();
			this.inputTokenToUserInput.Add(ExpressionTokenType.ListSeparator, (ExpressionParser.InputStringToken token) => new ListSeparatorInputString());
			this.inputTokenToUserInput.Add(ExpressionTokenType.Union, (ExpressionParser.InputStringToken token) => new ListSeparatorInputString());
			this.inputTokenToUserInput.Add(ExpressionTokenType.Number, (ExpressionParser.InputStringToken token) => new ExpressionInputString(token.TranslatableExpression));
			this.inputTokenToUserInput.Add(ExpressionTokenType.Array, (ExpressionParser.InputStringToken token) => new ExpressionInputString(token.TranslatableExpression));
			this.inputTokenToUserInput.Add(ExpressionTokenType.A1CellReference, (ExpressionParser.InputStringToken token) => new CellReferenceInputString((CellReferenceRangeExpression)token.TranslatableExpression));
			this.inputTokenToUserInput.Add(ExpressionTokenType.A1CellReferenceRange, (ExpressionParser.InputStringToken token) => new CellReferenceInputString((CellReferenceRangeExpression)token.TranslatableExpression));
			this.inputTokenToUserInput.Add(ExpressionTokenType.DefinedName, (ExpressionParser.InputStringToken token) => new DefinedNameInputString((SpreadsheetNameExpression)token.TranslatableExpression));
		}

		public ParseResult Parse(string input, Worksheet worksheet, int rowIndex, int columnIndex, SpreadsheetCultureHelper spreadsheetCultureInfo, out InputStringCollection stringExpressionCollection, out RadExpression expression, bool appendWorksheetName = false)
		{
			Guard.ThrowExceptionIfNullOrEmpty(input, "input");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			Guard.ThrowExceptionIfNull<SpreadsheetCultureHelper>(spreadsheetCultureInfo, "spreadsheetCultureInfo");
			expression = null;
			this.isParsingGlobalDefinedName = appendWorksheetName;
			stringExpressionCollection = null;
			Stack<RadExpression> stack = new Stack<RadExpression>();
			ExpressionLexer phraseBuilder = new ExpressionLexer(input, spreadsheetCultureInfo);
			ExpressionParser.InputStringTokenCollection inputStringTokenCollection = new ExpressionParser.InputStringTokenCollection();
			IEnumerable<ExpressionToken> enumerable = this.InfixToReversedPolishNotation(phraseBuilder, inputStringTokenCollection);
			ParseResult parseResult = ParseResult.Unsuccessful;
			foreach (ExpressionToken token in enumerable)
			{
				parseResult = this.ParseToken(token, stack, worksheet, rowIndex, columnIndex, inputStringTokenCollection);
				if (parseResult == ParseResult.Error)
				{
					return ParseResult.Error;
				}
			}
			if (this.errorEncountered || parseResult == ParseResult.Unsuccessful || stack.Count != 1)
			{
				this.errorEncountered = false;
				return ParseResult.Error;
			}
			stringExpressionCollection = this.BuildStringExpressionCollection(inputStringTokenCollection);
			expression = stack.Pop();
			return ParseResult.Successful;
		}

		IEnumerable<ExpressionToken> InfixToReversedPolishNotation(ExpressionLexer phraseBuilder, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			Stack<ExpressionToken> stack = new Stack<ExpressionToken>();
			ExpressionToken token;
			if (this.ReadAndSaveExpressionToken(phraseBuilder, inputStringTokenCollection, out token) == ParseResult.Error)
			{
				this.errorEncountered = true;
			}
			else
			{
				while (token != null)
				{
					if (token.TokenType == ExpressionTokenType.Function)
					{
						yield return ExpressionToken.FunctionStart;
						stack.Push(token);
					}
					else if (token.TokenType == ExpressionTokenType.ListSeparator)
					{
						while (stack.Count > 0 && stack.Peek().TokenType != ExpressionTokenType.LeftParenthesis)
						{
							yield return stack.Pop();
						}
						if (stack.Count == 0 || stack.Peek().TokenType != ExpressionTokenType.LeftParenthesis)
						{
							this.errorEncountered = true;
							goto IL_389;
						}
					}
					else if (OperatorInfos.IsOperator(token.TokenType))
					{
						foreach (ExpressionToken expressionToken in this.GetOperandsFromStack(stack, token))
						{
							yield return expressionToken;
						}
						stack.Push(token);
					}
					else if (token.TokenType == ExpressionTokenType.LeftParenthesis)
					{
						stack.Push(token);
					}
					else if (token.TokenType == ExpressionTokenType.RightParenthesis)
					{
						while (stack.Count != 0 && stack.Peek().TokenType != ExpressionTokenType.LeftParenthesis)
						{
							yield return stack.Pop();
						}
						if (stack.Count == 0 || stack.Peek().TokenType != ExpressionTokenType.LeftParenthesis)
						{
							this.errorEncountered = true;
							goto IL_389;
						}
						stack.Pop();
						if (stack.Count != 0 && stack.Peek().TokenType == ExpressionTokenType.Function)
						{
							yield return stack.Pop();
						}
					}
					else if (token.TokenType != ExpressionTokenType.Space)
					{
						yield return token;
					}
					if (this.ReadAndSaveExpressionToken(phraseBuilder, inputStringTokenCollection, out token) == ParseResult.Error)
					{
						this.errorEncountered = true;
						goto IL_389;
					}
				}
				while (stack.Count != 0)
				{
					yield return stack.Pop();
				}
			}
			IL_389:
			yield break;
		}

		ParseResult ReadAndSaveExpressionToken(ExpressionLexer phraseBuilder, ExpressionParser.InputStringTokenCollection inputStringTokenCollection, out ExpressionToken token)
		{
			ParseResult parseResult = phraseBuilder.Read(out token);
			if (parseResult == ParseResult.Successful)
			{
				inputStringTokenCollection.Add(token);
			}
			return parseResult;
		}

		ParseResult ParseToken(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			ParseResult parseResult = ParseResult.Unsuccessful;
			foreach (ExpressionParser.TokenHandler tokenHandler in this.tokenHandlers)
			{
				parseResult = tokenHandler(token, expressionStack, worksheet, rowIndex, columnIndex, inputStringTokenCollection);
				if (parseResult != ParseResult.Unsuccessful)
				{
					break;
				}
			}
			return parseResult;
		}

		ParseResult ParseLiteral(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			Func<RadExpression> func;
			if (this.tokenToExpression.TryGetValue(token.TokenType, out func))
			{
				RadExpression item = func();
				expressionStack.Push(item);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseNumber(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			if (token.TokenType == ExpressionTokenType.Number)
			{
				RadExpression radExpression = NumberExpression.CreateValidNumberOrErrorExpression(token.NumberValue);
				inputStringTokenCollection.UpdateTokenExpression(token, radExpression);
				expressionStack.Push(radExpression);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseText(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			if (token.TokenType == ExpressionTokenType.Text)
			{
				string value = TextHelper.DecodeValue(token.Value, "\"");
				RadExpression radExpression = new StringExpression(value);
				inputStringTokenCollection.UpdateTokenExpression(token, radExpression);
				expressionStack.Push(radExpression);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseError(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			if (token.TokenType == ExpressionTokenType.Error)
			{
				RadExpression item = ErrorExpressions.FindErrorExpression(token.Value);
				expressionStack.Push(item);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseUnaryOperator(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			RadExpression[] array = null;
			if (OperatorInfos.IsUnaryOperator(token.TokenType) && !this.TryPopNOperands(expressionStack, 1, out array))
			{
				return ParseResult.Error;
			}
			Func<RadExpression, RadExpression> func;
			if (this.unaryOperatorToExpression.TryGetValue(token.TokenType, out func))
			{
				RadExpression item = func(array[0]);
				expressionStack.Push(item);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseBinaryOperator(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			RadExpression[] array = null;
			if (OperatorInfos.IsBinaryOperator(token.TokenType) && !this.TryPopNOperands(expressionStack, 2, out array))
			{
				return ParseResult.Error;
			}
			Func<RadExpression, RadExpression, RadExpression> func;
			if (this.binaryOperatorToExpression.TryGetValue(token.TokenType, out func))
			{
				RadExpression item = func(array[0], array[1]);
				expressionStack.Push(item);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseFunction(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			if (token.TokenType != ExpressionTokenType.Function)
			{
				return ParseResult.Unsuccessful;
			}
			RadExpression[] arguments;
			if (this.ReadFunctionArguments(expressionStack, out arguments))
			{
				return ParseResult.Error;
			}
			try
			{
				FunctionExpression item = new FunctionExpression(token.Value, arguments, worksheet, rowIndex, columnIndex);
				expressionStack.Push(item);
			}
			catch (ExpressionException)
			{
				return ParseResult.Error;
			}
			inputStringTokenCollection.UpdateTokenString(token, token.Value.ToUpper());
			return ParseResult.Successful;
		}

		void GetWorksheetReference(CellReferenceExpressionToken token, Worksheet worksheet, out bool isWorksheetAbsolute, out string worksheetName)
		{
			isWorksheetAbsolute = !string.IsNullOrEmpty(token.WorksheetName);
			worksheetName = (isWorksheetAbsolute ? token.WorksheetName : worksheet.Name);
			bool encodeRegardlessOfContent = worksheetName[0] == '\'';
			string text = TextHelper.DecodeWorksheetName(worksheetName);
			Worksheet worksheetByName = SpreadsheetHelper.GetWorksheetByName(worksheet.Workbook, text);
			if (worksheetByName != null)
			{
				worksheetName = TextHelper.EncodeWorksheetName(worksheetByName.Name, encodeRegardlessOfContent);
			}
			if (string.Equals(text, worksheet.Workbook.Name, StringComparison.CurrentCultureIgnoreCase))
			{
				worksheetName = TextHelper.EncodeWorksheetName(worksheet.Name, false);
				isWorksheetAbsolute = true;
				return;
			}
			isWorksheetAbsolute |= this.isParsingGlobalDefinedName;
		}

		ParseResult ParseCellReference(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			CellReferenceExpressionToken cellReferenceExpressionToken = token as CellReferenceExpressionToken;
			if (cellReferenceExpressionToken != null)
			{
				bool isWorksheetAbsolute;
				string worksheetName;
				this.GetWorksheetReference(cellReferenceExpressionToken, worksheet, out isWorksheetAbsolute, out worksheetName);
				CellReferenceRange cellReferenceRange = new CellReferenceRange(worksheet.Workbook, worksheetName, isWorksheetAbsolute, CellAreaReference.CreateFromCellName(cellReferenceExpressionToken.CellOrName, rowIndex, columnIndex));
				RadExpression radExpression = new CellReferenceRangeExpression(cellReferenceRange);
				inputStringTokenCollection.UpdateTokenStringAndExpression(token, radExpression.ToString(), radExpression);
				expressionStack.Push(radExpression);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseCellReferenceRange(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			CellReferenceRangeExpressionToken cellReferenceRangeExpressionToken = token as CellReferenceRangeExpressionToken;
			if (cellReferenceRangeExpressionToken != null)
			{
				bool isWorksheetAbsolute;
				string worksheetName;
				this.GetWorksheetReference(cellReferenceRangeExpressionToken.Left, worksheet, out isWorksheetAbsolute, out worksheetName);
				CellReferenceRange cellReferenceRange = new CellReferenceRange(worksheet.Workbook, worksheetName, isWorksheetAbsolute, CellAreaReference.CreateFromTwoCellNames(cellReferenceRangeExpressionToken.Left.CellOrName, cellReferenceRangeExpressionToken.Right.CellOrName, rowIndex, columnIndex, true));
				RadExpression radExpression = new CellReferenceRangeExpression(cellReferenceRange);
				inputStringTokenCollection.UpdateTokenStringAndExpression(token, radExpression.ToString(), radExpression);
				expressionStack.Push(radExpression);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseVariable(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			DefinedNameExpressionToken definedNameExpressionToken = token as DefinedNameExpressionToken;
			if (definedNameExpressionToken != null)
			{
				string spreadsheetName = definedNameExpressionToken.CellOrName;
				DefinedName definedName = worksheet.Workbook.NameManager.FindSpreadsheetName(definedNameExpressionToken.WorksheetName, definedNameExpressionToken.CellOrName, worksheet, worksheet.Workbook) as DefinedName;
				if (definedName != null)
				{
					spreadsheetName = definedName.Name;
				}
				ExpressionQualifierInfo qualifierInfo = new ExpressionQualifierInfo(worksheet.Workbook, worksheet, definedNameExpressionToken.WorksheetName);
				RadExpression radExpression = new SpreadsheetNameExpression(qualifierInfo, rowIndex, columnIndex, spreadsheetName);
				inputStringTokenCollection.UpdateTokenStringAndExpression(token, radExpression.ToString(), radExpression);
				expressionStack.Push(radExpression);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		ParseResult ParseArray(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			ArrayExpressionToken arrayExpressionToken = token as ArrayExpressionToken;
			if (token.TokenType == ExpressionTokenType.Array && arrayExpressionToken != null)
			{
				RadExpression[,] array = new RadExpression[arrayExpressionToken.Array.GetLength(0), arrayExpressionToken.Array.GetLength(1)];
				for (int i = 0; i < arrayExpressionToken.Array.GetLength(0); i++)
				{
					for (int j = 0; j < arrayExpressionToken.Array.GetLength(1); j++)
					{
						ExpressionTokenType tokenType = arrayExpressionToken.Array[i, j].TokenType;
						RadExpression radExpression;
						switch (tokenType)
						{
						case ExpressionTokenType.Number:
							radExpression = NumberExpression.CreateValidNumberOrErrorExpression(arrayExpressionToken.Array[i, j].NumberValue);
							break;
						case ExpressionTokenType.Text:
							radExpression = new StringExpression(arrayExpressionToken.Array[i, j].Value);
							break;
						default:
							switch (tokenType)
							{
							case ExpressionTokenType.True:
								radExpression = BooleanExpression.True;
								goto IL_ED;
							case ExpressionTokenType.False:
								radExpression = BooleanExpression.False;
								goto IL_ED;
							case ExpressionTokenType.Error:
								radExpression = ErrorExpressions.FindErrorExpression(arrayExpressionToken.Array[i, j].Value);
								goto IL_ED;
							}
							return ParseResult.Error;
						}
						IL_ED:
						array[i, j] = radExpression;
					}
				}
				RadExpression radExpression2 = new ArrayExpression(array);
				inputStringTokenCollection.UpdateTokenStringAndExpression(token, radExpression2.ToString(), radExpression2);
				expressionStack.Push(radExpression2);
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		bool ReadFunctionArguments(Stack<RadExpression> expressionStack, out RadExpression[] expressions)
		{
			expressions = null;
			Stack<RadExpression> stack = new Stack<RadExpression>();
			while (expressionStack.Count > 0 && !(expressionStack.Peek() is FunctionStartExpression))
			{
				stack.Push(expressionStack.Pop());
			}
			if (expressionStack.Count == 0 || !(expressionStack.Peek() is FunctionStartExpression))
			{
				return true;
			}
			expressionStack.Pop();
			expressions = stack.ToArray();
			return false;
		}

		IEnumerable<ExpressionToken> GetOperandsFromStack(Stack<ExpressionToken> stack, ExpressionToken token)
		{
			while (stack.Count > 0 && OperatorInfos.IsOperator(stack.Peek().TokenType))
			{
				OperatorAssociativity tokenAssociativity = OperatorInfos.GetAssociativity(token.TokenType);
				int precedenceCompare = OperatorInfos.ComparePrecedence(token.TokenType, stack.Peek().TokenType);
				if ((tokenAssociativity != OperatorAssociativity.Left || precedenceCompare < 0) && (tokenAssociativity != OperatorAssociativity.Right || precedenceCompare <= 0))
				{
					break;
				}
				yield return stack.Pop();
			}
			yield break;
		}

		bool TryPopNOperands(Stack<RadExpression> expressionStack, int n, out RadExpression[] operands)
		{
			operands = null;
			Stack<RadExpression> stack = new Stack<RadExpression>();
			while (expressionStack.Count > 0 && n > 0)
			{
				stack.Push(expressionStack.Pop());
				n--;
			}
			if (n != 0)
			{
				return false;
			}
			operands = stack.ToArray();
			return true;
		}

		InputStringCollection BuildStringExpressionCollection(ExpressionParser.InputStringTokenCollection inputStringTokenCollection)
		{
			InputStringCollection inputStringCollection = new InputStringCollection();
			for (int i = 0; i < inputStringTokenCollection.Count; i++)
			{
				ExpressionParser.InputStringToken inputStringToken = inputStringTokenCollection[i];
				Func<ExpressionParser.InputStringToken, UserInputStringBase> func;
				UserInputStringBase stringExpression;
				if (this.inputTokenToUserInput.TryGetValue(inputStringToken.Token.TokenType, out func))
				{
					stringExpression = func(inputStringToken);
				}
				else
				{
					stringExpression = new FormatInputString(inputStringToken.StringValue);
				}
				inputStringCollection.Add(stringExpression);
			}
			return inputStringCollection;
		}

		readonly ExpressionParser.TokenHandler[] tokenHandlers;

		readonly Dictionary<ExpressionTokenType, Func<RadExpression>> tokenToExpression;

		readonly Dictionary<ExpressionTokenType, Func<RadExpression, RadExpression>> unaryOperatorToExpression;

		readonly Dictionary<ExpressionTokenType, Func<RadExpression, RadExpression, RadExpression>> binaryOperatorToExpression;

		readonly Dictionary<ExpressionTokenType, Func<ExpressionParser.InputStringToken, UserInputStringBase>> inputTokenToUserInput;

		bool errorEncountered;

		bool isParsingGlobalDefinedName;

		delegate ParseResult TokenHandler(ExpressionToken token, Stack<RadExpression> expressionStack, Worksheet worksheet, int rowIndex, int columnIndex, ExpressionParser.InputStringTokenCollection expressionTokenCollection);

		class InputStringToken
		{
			public ExpressionToken Token { get; set; }

			public string StringValue { get; set; }

			public RadExpression TranslatableExpression { get; set; }

			public override bool Equals(object obj)
			{
				ExpressionParser.InputStringToken inputStringToken = obj as ExpressionParser.InputStringToken;
				return inputStringToken != null && this.Token.Equals(inputStringToken.Token);
			}

			public override int GetHashCode()
			{
				return this.Token.GetHashCode();
			}
		}

		class InputStringTokenCollection
		{
			public InputStringTokenCollection()
			{
				this.innerList = new List<ExpressionParser.InputStringToken>();
			}

			public ExpressionParser.InputStringToken this[int i]
			{
				get
				{
					return this.innerList[i];
				}
			}

			public int Count
			{
				get
				{
					return this.innerList.Count;
				}
			}

			public void Add(ExpressionToken token)
			{
				this.innerList.Add(new ExpressionParser.InputStringToken
				{
					Token = token,
					StringValue = token.Value
				});
			}

			public void UpdateTokenString(ExpressionToken token, string updatedString)
			{
				ExpressionParser.InputStringToken inputStringToken = this.IndexOfStringExpressionToken(token);
				inputStringToken.StringValue = updatedString;
			}

			public void UpdateTokenStringAndExpression(ExpressionToken token, string updatedString, RadExpression updatedExpression)
			{
				ExpressionParser.InputStringToken inputStringToken = this.IndexOfStringExpressionToken(token);
				inputStringToken.StringValue = updatedString;
				inputStringToken.TranslatableExpression = updatedExpression;
			}

			public void UpdateTokenExpression(ExpressionToken token, RadExpression expression)
			{
				ExpressionParser.InputStringToken inputStringToken = this.IndexOfStringExpressionToken(token);
				inputStringToken.TranslatableExpression = expression;
			}

			ExpressionParser.InputStringToken IndexOfStringExpressionToken(ExpressionToken token)
			{
				ExpressionParser.InputStringToken inputStringToken = this.innerList.First((ExpressionParser.InputStringToken t) => t.Token.Equals(token));
				Guard.ThrowExceptionIfNull<ExpressionParser.InputStringToken>(inputStringToken, "inputStringToken");
				return inputStringToken;
			}

			readonly List<ExpressionParser.InputStringToken> innerList;
		}
	}
}
