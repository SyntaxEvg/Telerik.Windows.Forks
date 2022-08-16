using System;

namespace CsQuery.StringScanner
{
	interface IExpectPattern
	{
		void Initialize(int startIndex, char[] source);

		bool Validate();

		string Result { get; }

		int EndIndex { get; }
	}
}
