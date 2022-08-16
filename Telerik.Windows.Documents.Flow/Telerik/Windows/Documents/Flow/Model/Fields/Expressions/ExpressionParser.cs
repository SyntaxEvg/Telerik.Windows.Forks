using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions;
using Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	static class ExpressionParser
	{
		public static Expression Parse(string input, RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNullOrEmpty(input, "input");
			Expression result;
			try
			{
				Stack<Expression> stack = new Stack<Expression>();
				ExpressionTokenizer tokenizer = new ExpressionTokenizer(input);
				IEnumerable<ExpressionToken> enumerable = ExpressionParser.InfixToReversedPolishNotation(tokenizer);
				foreach (ExpressionToken token in enumerable)
				{
					ExpressionParser.ParseToken(token, stack, document);
				}
				if (stack.Count != 1)
				{
					throw new ExpressionException("Spreadsheet_ErrorExpressions_GeneralErrorMessage");
				}
				result = stack.Pop();
			}
			catch (ExpressionException error)
			{
				result = new ErrorExpression(error);
			}
			return result;
		}

		static IEnumerable<ExpressionToken> InfixToReversedPolishNotation(ExpressionTokenizer tokenizer)
		{
			Stack<ExpressionToken> stack = new Stack<ExpressionToken>();
			for (ExpressionToken token = tokenizer.Read(); token != null; token = tokenizer.Read())
			{
				if (token.TokenType == ExpressionTokenType.Function)
				{
					yield return ExpressionToken.FunctionStart;
					stack.Push(token);
				}
				else if (token.TokenType == ExpressionTokenType.ListSeparator)
				{
					foreach (ExpressionToken item in ExpressionParser.PopUntilTokenOfType(stack, ExpressionTokenType.LeftParenthesis))
					{
						yield return item;
					}
				}
				else if (OperatorInfos.IsOperator(token.TokenType))
				{
					foreach (ExpressionToken expressionToken in ExpressionParser.GetOperandsFromStack(stack, token))
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
					foreach (ExpressionToken item2 in ExpressionParser.PopUntilTokenOfType(stack, ExpressionTokenType.LeftParenthesis))
					{
						yield return item2;
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
			}
			while (stack.Count != 0)
			{
				yield return stack.Pop();
			}
			yield break;
		}

		static IEnumerable<ExpressionToken> PopUntilTokenOfType(Stack<ExpressionToken> stack, ExpressionTokenType expectedTokenType)
		{
			while (stack.Count > 0 && stack.Peek().TokenType != expectedTokenType)
			{
				yield return stack.Pop();
			}
			if (stack.Count == 0)
			{
				throw new ExpressionException(string.Format("Could not find token of type: {0}", expectedTokenType));
			}
			ExpressionParser.EnsureTokenOfType(stack.Peek(), expectedTokenType);
			yield break;
		}

		static void ParseToken(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			bool flag = false;
			foreach (ExpressionParser.TokenHandler tokenHandler in ExpressionParser.TokenHandlers)
			{
				if (tokenHandler(token, expressionStack, document))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				throw new ExpressionException("Could not handle token: " + token.TokenType);
			}
		}

		static bool ParseLiteral(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			ExpressionTokenType tokenType = token.TokenType;
			Expression item;
			if (tokenType != ExpressionTokenType.Number)
			{
				switch (tokenType)
				{
				case ExpressionTokenType.True:
					item = BooleanExpression.True;
					break;
				case ExpressionTokenType.False:
					item = BooleanExpression.False;
					break;
				default:
					return false;
				}
			}
			else
			{
				item = new NumberExpression(token.NumberValue);
			}
			expressionStack.Push(item);
			return true;
		}

		static bool ParseUnaryOperator(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			ExpressionTokenType tokenType = token.TokenType;
			Expression item;
			if (tokenType != ExpressionTokenType.Percent)
			{
				if (tokenType != ExpressionTokenType.UnaryMinus)
				{
					return false;
				}
				Expression[] array = ExpressionParser.PopNOperands(expressionStack, 1);
				item = new UnaryMinusExpression(array[0]);
			}
			else
			{
				Expression[] array = ExpressionParser.PopNOperands(expressionStack, 1);
				item = new PercentExpression(array[0]);
			}
			expressionStack.Push(item);
			return true;
		}

		static bool ParseBinaryOperator(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			Func<Expression[]> getArgsFunc = () => ExpressionParser.PopNOperands(expressionStack, 2);
			BinaryOperatorExpression item;
			if (BinaryOperatorExpressionFactory.TryCreateExpression(token.TokenType, getArgsFunc, out item))
			{
				expressionStack.Push(item);
				return true;
			}
			return false;
		}

		static bool ParseFunctionStart(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			if (token.TokenType == ExpressionTokenType.FunctionStart)
			{
				expressionStack.Push(new FunctionStartExpression());
				return true;
			}
			return false;
		}

		static bool ParseFunction(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			if (token.TokenType != ExpressionTokenType.Function)
			{
				return false;
			}
			Expression[] arguments = ExpressionParser.ReadFunctionArguments(expressionStack);
			FunctionExpression item = FunctionExpressionFactory.CreateFunctionExpression(token.Value, arguments);
			expressionStack.Push(item);
			return true;
		}

		static bool ParseBookmark(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document)
		{
			if (token.TokenType == ExpressionTokenType.Bookmark)
			{
				BookmarkExpression item = new BookmarkExpression(token.Value, document);
				expressionStack.Push(item);
				return true;
			}
			return false;
		}

		static Expression[] ReadFunctionArguments(Stack<Expression> expressionStack)
		{
			Stack<Expression> stack = new Stack<Expression>();
			while (expressionStack.Count > 0 && !(expressionStack.Peek() is FunctionStartExpression))
			{
				stack.Push(expressionStack.Pop());
			}
			if (expressionStack.Count == 0 || !(expressionStack.Peek() is FunctionStartExpression))
			{
				throw new ExpressionException("Invalid function.");
			}
			expressionStack.Pop();
			return stack.ToArray();
		}

		static void EnsureTokenOfType(ExpressionToken token, ExpressionTokenType tokenType)
		{
			if (token.TokenType != tokenType)
			{
				throw new ExpressionException(string.Format("Expected token not found: {0}", tokenType));
			}
		}

		static Expression[] PopNOperands(Stack<Expression> expressionStack, int n)
		{
			Stack<Expression> stack = new Stack<Expression>();
			while (expressionStack.Count > 0 && n > 0)
			{
				stack.Push(expressionStack.Pop());
				n--;
			}
			if (n != 0)
			{
				throw new ExpressionException("Not enough operands.");
			}
			return stack.ToArray();
		}

		static IEnumerable<ExpressionToken> GetOperandsFromStack(Stack<ExpressionToken> stack, ExpressionToken token)
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

		static readonly ExpressionParser.TokenHandler[] TokenHandlers = new ExpressionParser.TokenHandler[]
		{
			new ExpressionParser.TokenHandler(ExpressionParser.ParseLiteral),
			new ExpressionParser.TokenHandler(ExpressionParser.ParseBookmark),
			new ExpressionParser.TokenHandler(ExpressionParser.ParseUnaryOperator),
			new ExpressionParser.TokenHandler(ExpressionParser.ParseBinaryOperator),
			new ExpressionParser.TokenHandler(ExpressionParser.ParseFunctionStart),
			new ExpressionParser.TokenHandler(ExpressionParser.ParseFunction)
		};

		delegate bool TokenHandler(ExpressionToken token, Stack<Expression> expressionStack, RadFlowDocument document);
	}
}
