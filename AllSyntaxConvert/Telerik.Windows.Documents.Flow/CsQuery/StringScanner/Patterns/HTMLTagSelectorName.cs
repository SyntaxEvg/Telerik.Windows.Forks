using System;

namespace CsQuery.StringScanner.Patterns
{
	class HTMLTagSelectorName : EscapedString
	{
		public HTMLTagSelectorName()
			: base(new Func<int, char, bool>(HTMLTagSelectorName.IsValidTagName))
		{
		}

		static bool IsValidTagName(int index, char character)
		{
			if (index == 0)
			{
				return CharacterData.IsType(character, CharacterType.HtmlTagSelectorStart);
			}
			return CharacterData.IsType(character, CharacterType.HtmlTagSelectorExceptStart);
		}
	}
}
