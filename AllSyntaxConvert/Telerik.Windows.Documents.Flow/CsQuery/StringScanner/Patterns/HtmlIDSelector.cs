using System;

namespace CsQuery.StringScanner.Patterns
{
	class HtmlIDSelector : EscapedString
	{
		public HtmlIDSelector()
			: base(new Func<int, char, bool>(HtmlIDSelector.IsValidIDSelector))
		{
		}

		static bool IsValidIDSelector(int index, char character)
		{
			return !CharacterData.IsType(character, CharacterType.SelectorTerminator) && !CharacterData.IsType(character, CharacterType.HtmlSpace);
		}
	}
}
