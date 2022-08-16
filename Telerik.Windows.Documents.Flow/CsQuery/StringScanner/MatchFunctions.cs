using System;
using CsQuery.StringScanner.Patterns;

namespace CsQuery.StringScanner
{
	static class MatchFunctions
	{
		public static bool Alpha(int index, char character)
		{
			return CharacterData.IsType(character, CharacterType.Alpha);
		}

		public static IExpectPattern Number(bool requireWhitespaceTerminator = false)
		{
			return new Number
			{
				RequireWhitespaceTerminator = requireWhitespaceTerminator
			};
		}

		public static bool Alphanumeric(int index, char character)
		{
			return CharacterData.IsType(character, CharacterType.Alpha | CharacterType.NumberPart);
		}

		public static IExpectPattern HtmlIDValue()
		{
			return new HtmlIDSelector();
		}

		public static IExpectPattern HTMLAttribute()
		{
			return new HTMLAttributeName();
		}

		public static IExpectPattern HTMLTagSelectorName()
		{
			return new HTMLTagSelectorName();
		}

		public static IExpectPattern BoundedBy(string boundStart = null, string boundEnd = null, bool honorInnerQuotes = false)
		{
			Bounded bounded = new Bounded();
			if (!string.IsNullOrEmpty(boundStart))
			{
				bounded.BoundStart = boundStart;
			}
			if (!string.IsNullOrEmpty(boundEnd))
			{
				bounded.BoundEnd = boundEnd;
			}
			bounded.HonorInnerQuotes = honorInnerQuotes;
			return bounded;
		}

		public static IExpectPattern Bounded
		{
			get
			{
				return new Bounded
				{
					HonorInnerQuotes = false
				};
			}
		}

		public static IExpectPattern BoundedWithQuotedContent
		{
			get
			{
				return new Bounded
				{
					HonorInnerQuotes = true
				};
			}
		}

		public static bool NonWhitespace(int index, char character)
		{
			return !CharacterData.IsType(character, CharacterType.Whitespace);
		}

		public static bool QuoteChar(int index, char character)
		{
			return CharacterData.IsType(character, CharacterType.Quote);
		}

		public static bool BoundChar(int index, char character)
		{
			return CharacterData.IsType(character, CharacterType.Enclosing | CharacterType.Quote);
		}

		public static IExpectPattern Quoted()
		{
			return new Quoted();
		}

		public static bool PseudoSelector(int index, char character)
		{
			if (index != 0)
			{
				return CharacterData.IsType(character, CharacterType.Alpha) || character == '-';
			}
			return CharacterData.IsType(character, CharacterType.Alpha);
		}

		public static IExpectPattern CssClassName
		{
			get
			{
				return new CssClassName();
			}
		}

		public static IExpectPattern OptionallyQuoted(string terminators = null)
		{
			return new OptionallyQuoted(terminators);
		}

		public static bool Operator(int index, char character)
		{
			return CharacterData.IsType(character, CharacterType.Operator);
		}
	}
}
