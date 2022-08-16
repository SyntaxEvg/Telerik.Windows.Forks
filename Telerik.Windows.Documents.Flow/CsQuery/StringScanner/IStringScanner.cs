using System;
using System.Collections.Generic;

namespace CsQuery.StringScanner
{
	interface IStringScanner
	{
		string Text { get; set; }

		char[] Chars { get; set; }

		bool IgnoreWhitespace { get; set; }

		int Length { get; }

		int Index { get; }

		int LastIndex { get; }

		char Current { get; }

		char Peek();

		string CurrentOrEmpty { get; }

		string Match { get; }

		string LastMatch { get; }

		bool Finished { get; }

		bool AtEnd { get; }

		bool Success { get; }

		string LastError { get; }

		bool AllowQuoting();

		ICharacterInfo Info { get; }

		void SkipWhitespace();

		void NextNonWhitespace();

		bool Next();

		bool Previous();

		bool Move(int count);

		void Undo();

		void End();

		void AssertFinished(string errorMessage = null);

		void AssertNotFinished(string errorMessage = null);

		void Reset();

		bool Matches(string text);

		bool MatchesAny(IEnumerable<string> text);

		IStringScanner Seek(char character, bool orEnd);

		IStringScanner ToNewScanner();

		IStringScanner ToNewScanner(string template);

		IStringScanner Expect(string text);

		IStringScanner Expect(IExpectPattern pattern);

		IStringScanner Expect(Func<int, char, bool> validate);

		IStringScanner ExpectChar(char character);

		IStringScanner ExpectChar(IEnumerable<char> characters);

		IStringScanner ExpectNumber(bool requireWhitespaceTerminator = false);

		IStringScanner ExpectAlpha();

		IStringScanner ExpectBoundedBy(string start, string end, bool allowQuoting = false);

		IStringScanner ExpectBoundedBy(char bound, bool allowQuoting = false);

		bool TryGet(IEnumerable<string> stringList, out string result);

		bool TryGet(IExpectPattern pattern, out string result);

		bool TryGet(Func<int, char, bool> validate, out string result);

		bool TryGetChar(char character, out string result);

		bool TryGetChar(string characters, out string result);

		bool TryGetChar(IEnumerable<char> characters, out string result);

		bool TryGetNumber(out string result);

		bool TryGetNumber<T>(out T result) where T : IConvertible;

		bool TryGetNumber(out int result);

		bool TryGetAlpha(out string result);

		bool TryGetBoundedBy(string start, string end, bool allowQuoting, out string result);

		string Get(params string[] values);

		string Get(IEnumerable<string> stringList);

		string Get(IExpectPattern pattern);

		string Get(Func<int, char, bool> validate);

		string GetNumber();

		string GetAlpha();

		string GetBoundedBy(string start, string end, bool allowQuoting = false);

		string GetBoundedBy(char bound, bool allowQuoting = false);

		char GetChar(char character);

		char GetChar(string characters);

		char GetChar(IEnumerable<char> characters);

		void Expect(params string[] values);

		void Expect(IEnumerable<string> stringList);
	}
}
