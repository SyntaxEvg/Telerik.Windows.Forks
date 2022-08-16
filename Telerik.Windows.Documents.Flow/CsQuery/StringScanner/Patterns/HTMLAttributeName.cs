using System;

namespace CsQuery.StringScanner.Patterns
{
	class HTMLAttributeName : EscapedString
	{
		public HTMLAttributeName()
			: base(new Func<int, char, bool>(HTMLAttributeName.IsValidAttributeName))
		{
		}

		protected static bool IsValidAttributeName(int index, char character)
		{
			if (index == 0)
			{
				return CharacterData.IsType(character, CharacterType.Alpha);
			}
			return CharacterData.IsType(character, CharacterType.HtmlAttributeName);
		}
	}
}
