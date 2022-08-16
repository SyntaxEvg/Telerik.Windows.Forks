using System;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	static class FieldCodeParser
	{
		public static FieldCodeParseResult ParseField(FieldInfo fieldInfo)
		{
			Guard.ThrowExceptionIfNull<FieldInfo>(fieldInfo, "field");
			if (fieldInfo.Start == null || fieldInfo.End == null)
			{
				throw new InvalidOperationException("There are no field characters attached to field");
			}
			FieldCharacter end;
			if (fieldInfo.Separator != null && fieldInfo.Separator.Parent != null)
			{
				end = fieldInfo.Separator;
			}
			else
			{
				end = fieldInfo.End;
			}
			FieldTokenizer tokenizer = new FieldTokenizer(fieldInfo.Start, end);
			Field field = FieldCodeParser.CreateField(tokenizer, fieldInfo.Document);
			FieldParameters fieldParameters = FieldCodeParser.ReadFieldArguments(field, tokenizer);
			return new FieldCodeParseResult(field, fieldParameters);
		}

		static Field CreateField(FieldTokenizer tokenizer, RadFlowDocument document)
		{
			FieldToken fieldToken = tokenizer.Read(true);
			if (fieldToken == null || (fieldToken.TokenType != FieldTokenType.Argument && fieldToken.TokenType != FieldTokenType.ExpressionStart))
			{
				return new ParseErrorField(document, "Unknown token type parsed.");
			}
			Field result;
			if (fieldToken.TokenType == FieldTokenType.ExpressionStart)
			{
				result = new ExpressionField(document);
			}
			else
			{
				result = FieldFactory.CreateField(fieldToken.Value, document);
			}
			return result;
		}

		static FieldParameters ReadFieldArguments(Field field, FieldTokenizer tokenizer)
		{
			FieldParameters fieldParameters = new FieldParameters();
			if (field is ParseErrorField)
			{
				return fieldParameters;
			}
			if (field is ExpressionField)
			{
				fieldParameters.Expression = FieldCodeParser.ReadExpression(tokenizer);
			}
			if (field.ExpectComparisonArgument)
			{
				fieldParameters.Comparison = FieldCodeParser.ReadComparison(tokenizer);
			}
			FieldToken fieldToken = tokenizer.Read(true);
			FieldSwitch fieldSwitch = null;
			while (fieldToken != null && fieldToken.TokenType != FieldTokenType.Error)
			{
				if (fieldToken.TokenType == FieldTokenType.Switch)
				{
					fieldSwitch = null;
					FieldSwitch fieldSwitch2 = new FieldSwitch(fieldToken.Value);
					fieldParameters.AddSwitch(fieldSwitch2);
					if (field.IsSwitchWithArgument(fieldSwitch2.SwitchValue))
					{
						fieldSwitch = fieldSwitch2;
					}
				}
				else if (fieldToken.TokenType == FieldTokenType.Argument)
				{
					if (fieldSwitch != null)
					{
						fieldSwitch.Argument = new FieldArgument(fieldToken.Value);
						fieldSwitch = null;
					}
					else
					{
						fieldParameters.AddArgument(new FieldArgument(fieldToken.Value));
					}
				}
				fieldToken = tokenizer.Read(true);
			}
			return fieldParameters;
		}

		static string ReadExpression(FieldTokenizer tokenizer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			FieldToken fieldToken = tokenizer.Read(true);
			while (fieldToken != null && fieldToken.TokenType != FieldTokenType.Error && FieldCodeParser.IsTokenPartOfExpression(fieldToken, true))
			{
				stringBuilder.Append(fieldToken.Value);
				fieldToken = tokenizer.Read(true);
			}
			return stringBuilder.ToString();
		}

		static bool IsTokenPartOfExpression(FieldToken token, bool acceptWhiteSpaces)
		{
			return token.TokenType == FieldTokenType.Argument || (acceptWhiteSpaces && token.TokenType == FieldTokenType.WhiteSpace);
		}

		static FieldComparison ReadComparison(FieldTokenizer tokenizer)
		{
			FieldToken fieldToken = tokenizer.Read(true);
			FieldToken fieldToken2 = tokenizer.Read(true);
			FieldToken fieldToken3 = tokenizer.Read(true);
			string left = string.Empty;
			bool isLeftQuoted = false;
			string op = string.Empty;
			string right = string.Empty;
			bool isRightQuoted = false;
			if (fieldToken != null && fieldToken.TokenType == FieldTokenType.Argument)
			{
				left = fieldToken.Value;
				isLeftQuoted = fieldToken.IsQuoted;
			}
			if (fieldToken2 != null && fieldToken2.TokenType == FieldTokenType.Argument)
			{
				op = fieldToken2.Value;
			}
			if (fieldToken3 != null && fieldToken3.TokenType == FieldTokenType.Argument)
			{
				right = fieldToken3.Value;
				isRightQuoted = fieldToken3.IsQuoted;
			}
			return new FieldComparison(left, isLeftQuoted, op, right, isRightQuoted);
		}
	}
}
