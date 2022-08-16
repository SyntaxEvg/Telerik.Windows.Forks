using System;

namespace CsQuery.Engine
{
	class CaseSensitiveCharacterEqualityComparer : CharacterEqualityComparer
	{
		public override bool Equals(char x, char y)
		{
			return char.ToLower(x).Equals(char.ToLower(y));
		}

		public override int GetHashCode(char obj)
		{
			return char.ToLower(obj).GetHashCode();
		}
	}
}
