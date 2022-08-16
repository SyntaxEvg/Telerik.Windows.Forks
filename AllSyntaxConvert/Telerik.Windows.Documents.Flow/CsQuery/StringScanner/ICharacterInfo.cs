using System;

namespace CsQuery.StringScanner
{
	interface ICharacterInfo : IValueInfo<char>, IValueInfo
	{
		bool Parenthesis { get; }

		bool Enclosing { get; }

		bool Bound { get; }

		bool Quote { get; }

		bool Separator { get; }
	}
}
