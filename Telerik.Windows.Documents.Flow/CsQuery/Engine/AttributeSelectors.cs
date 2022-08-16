using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.Implementation;
using CsQuery.StringScanner;

namespace CsQuery.Engine
{
	static class AttributeSelectors
	{
		public static bool Matches(IDomElement element, SelectorClause selector)
		{
			string text;
			if (!((DomElement)element).TryGetAttributeForMatching(selector.AttributeNameTokenID, out text))
			{
				AttributeSelectorType attributeSelectorType = selector.AttributeSelectorType;
				return attributeSelectorType != AttributeSelectorType.Exists && (attributeSelectorType == AttributeSelectorType.NotExists || attributeSelectorType == AttributeSelectorType.NotEquals);
			}
			switch (selector.AttributeSelectorType)
			{
			case AttributeSelectorType.Exists:
				return true;
			case AttributeSelectorType.Equals:
				return selector.AttributeValue.Equals(text, selector.AttributeValueStringComparison);
			case AttributeSelectorType.StartsWith:
				return text != null && text.Length >= selector.AttributeValue.Length && text.Substring(0, selector.AttributeValue.Length).Equals(selector.AttributeValue, selector.AttributeValueStringComparison);
			case AttributeSelectorType.Contains:
				return text != null && text.IndexOf(selector.AttributeValue, selector.AttributeValueStringComparison) >= 0;
			case AttributeSelectorType.NotExists:
				return false;
			case AttributeSelectorType.ContainsWord:
				return text != null && AttributeSelectors.ContainsWord(text, selector.AttributeValue, selector.AttributeValueStringComparer);
			case AttributeSelectorType.EndsWith:
			{
				int length = selector.AttributeValue.Length;
				return text != null && text.Length >= length && text.Substring(text.Length - length).Equals(selector.AttributeValue, selector.AttributeValueStringComparison);
			}
			case AttributeSelectorType.NotEquals:
				return !selector.AttributeValue.Equals(text, selector.AttributeValueStringComparison);
			case AttributeSelectorType.StartsWithOrHyphen:
			{
				if (text == null)
				{
					return false;
				}
				int num = text.IndexOf("-");
				string value = text;
				if (num >= 0)
				{
					value = text.Substring(0, num);
				}
				return selector.AttributeValue.Equals(value, selector.AttributeValueStringComparison) || selector.AttributeValue.Equals(text, selector.AttributeValueStringComparison);
			}
			default:
				throw new InvalidOperationException("No AttributeSelectorType set");
			}
		}

		static bool ContainsWord(string sentence, string word, StringComparer comparer)
		{
			HashSet<string> source = new HashSet<string>(sentence.Trim().Split(CharacterData.charsHtmlSpaceArray, StringSplitOptions.RemoveEmptyEntries));
			return source.Contains(word, comparer);
		}
	}
}
