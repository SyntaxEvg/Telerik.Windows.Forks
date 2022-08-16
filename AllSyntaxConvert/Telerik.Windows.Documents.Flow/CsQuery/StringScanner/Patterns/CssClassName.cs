using System;

namespace CsQuery.StringScanner.Patterns
{
	class CssClassName : EscapedString
	{
		public CssClassName()
			: base(new Func<int, char, bool>(CssClassName.IsValidClassName))
		{
		}

		protected static bool IsValidClassName(int index, char character)
		{
			if (index == 0)
			{
				return CharacterData.IsType(character, CharacterType.AlphaISO10646);
			}
			return CharacterData.IsType(character, CharacterType.Number | CharacterType.AlphaISO10646);
		}
	}
}
