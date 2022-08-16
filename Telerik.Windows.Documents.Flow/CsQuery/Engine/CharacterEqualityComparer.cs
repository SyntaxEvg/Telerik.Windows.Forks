using System;
using System.Collections.Generic;

namespace CsQuery.Engine
{
	class CharacterEqualityComparer : EqualityComparer<char>
	{
		public static CharacterEqualityComparer Create(bool isCaseSensitive)
		{
			if (!isCaseSensitive)
			{
				return new CharacterEqualityComparer();
			}
			return new CaseSensitiveCharacterEqualityComparer();
		}

		public override bool Equals(char x, char y)
		{
			return x.Equals(y);
		}

		public override int GetHashCode(char obj)
		{
			return obj.GetHashCode();
		}
	}
}
