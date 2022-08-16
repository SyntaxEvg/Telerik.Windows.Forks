using System;
using System.Collections.Generic;
using System.Diagnostics;
using CsQuery.StringScanner.ExtensionMethods;
using CsQuery.StringScanner.Patterns;

namespace CsQuery.StringScanner.Implementation
{
	class StringScannerEngine : IStringScanner
	{
		public StringScannerEngine()
		{
			this.Init();
		}

		public StringScannerEngine(string text)
		{
			this.Text = text;
			this.Init();
		}

		public static implicit operator StringScannerEngine(string text)
		{
			return new StringScannerEngine(text);
		}

		protected void Init()
		{
			this.IgnoreWhitespace = true;
			this.Reset();
		}

		protected bool QuotingActive { get; set; }

		protected char QuoteChar { get; set; }

		protected CharacterInfo characterInfo
		{
			get
			{
				if (this._characterInfo == null)
				{
					this._characterInfo = new CharacterInfo();
				}
				return this._characterInfo;
			}
		}

		protected bool SuppressErrors { get; set; }

		public string Text
		{
			get
			{
				if (this._Text == null)
				{
					this._Text = new string(this._Chars);
				}
				return this._Text;
			}
			set
			{
				this._Text = value ?? "";
				this._Chars = null;
				this.Length = this._Text.Length;
				this.Reset();
			}
		}

		public char[] Chars
		{
			get
			{
				if (this._Chars == null)
				{
					this._Chars = this._Text.ToCharArray();
				}
				return this._Chars;
			}
			set
			{
				this._Chars = value;
				this._Text = null;
				this.Length = value.Length;
				this.Reset();
			}
		}

		public bool AllowQuoting()
		{
			if (this.IgnoreWhitespace)
			{
				this.NextNonWhitespace();
			}
			if (CharacterData.IsType(this.Peek(), CharacterType.Whitespace))
			{
				this.Next();
				this.QuotingActive = true;
				this.QuoteChar = this.Current;
			}
			return this.QuotingActive;
		}

		public bool IgnoreWhitespace { get; set; }

		public int Length { get; protected set; }

		public int Index { get; protected set; }

		public int LastIndex { get; protected set; }

		public char Current
		{
			get
			{
				return this.Text[this.Index];
			}
		}

		public string CurrentOrEmpty
		{
			get
			{
				if (!this.Finished)
				{
					char c = this.Current;
					return c.ToString();
				}
				return null;
			}
		}

		public string Match
		{
			get
			{
				return this._CurrentMatch;
			}
			protected set
			{
				this._LastMatch = this._CurrentMatch;
				this._CurrentMatch = value;
			}
		}

		public string LastMatch
		{
			get
			{
				return this._LastMatch;
			}
			protected set
			{
				this._LastMatch = value;
			}
		}

		public bool Finished
		{
			get
			{
				return this.Index >= this.Length || this.Length == 0;
			}
		}

		public bool AtEnd
		{
			get
			{
				return this.Index == this.Length - 1;
			}
		}

		public string LastError { get; protected set; }

		public bool Success { get; protected set; }

		public ICharacterInfo Info
		{
			get
			{
				this.characterInfo.Target = (this.Finished ? '\0' : this.Current);
				return this.characterInfo;
			}
		}

		public IStringScanner ToNewScanner()
		{
			if (!this.Success)
			{
				throw new InvalidOperationException("The last operation was not successful; a new string scanner cannot be created.");
			}
			return Scanner.Create(this.Match);
		}

		public IStringScanner ToNewScanner(string template)
		{
			if (!this.Success)
			{
				throw new InvalidOperationException("The last operation was not successful; a new string scanner cannot be created.");
			}
			return Scanner.Create(string.Format(template, this.Match));
		}

		public bool Matches(string text)
		{
			return text.Length + this.Index <= this.Length && this.Text.Substring(this.Index, text.Length) == text;
		}

		public bool MatchesAny(IEnumerable<string> text)
		{
			foreach (string text2 in text)
			{
				if (this.Matches(text2))
				{
					return true;
				}
			}
			return false;
		}

		public IStringScanner Seek(char character, bool orEnd)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			while (!this.Finished && this.Current != character)
			{
				this.Next();
			}
			if (!orEnd)
			{
				this.AssertNotFinished(null);
			}
			this.Match = this.Text.Substring(this.cachedPos, this.Index - this.cachedPos);
			this.NewPos();
			return this;
		}

		protected void SkipWhitespaceImpl()
		{
			if (this.Finished)
			{
				return;
			}
			if (CharacterData.IsType(this.Current, CharacterType.Whitespace))
			{
				while (!this.Finished && CharacterData.IsType(this.Current, CharacterType.Whitespace))
				{
					this.Next();
				}
			}
		}

		public void SkipWhitespace()
		{
			this.CachePos();
			this.AutoSkipWhitespace();
			this.NewPos();
		}

		public void NextNonWhitespace()
		{
			this.CachePos();
			this.NextNonWhitespaceImpl();
			this.NewPos();
		}

		public char Peek()
		{
			if (this.Index < this.Length - 1)
			{
				return this.Text[this.Index + 1];
			}
			return '\0';
		}

		public bool Next()
		{
			return this.Move(1);
		}

		public bool Previous()
		{
			return this.Move(-1);
		}

		public bool Move(int offset)
		{
			if (this.Index + offset > this.Length)
			{
				this.ThrowException("Cannot advance beyond end of string.");
			}
			else if (this.Index + offset < 0)
			{
				this.ThrowException("Cannot reverse beyond beginning of string");
			}
			this.Index += offset;
			return this.Index < this.Length && this.Index > 0;
		}

		public void Undo()
		{
			if (this.LastIndex < 0)
			{
				this.ThrowException("Can't undo - there's nothing to undo");
			}
			this.Index = this.LastIndex;
			this.Match = this.LastMatch;
			this.LastMatch = "";
			this.LastIndex = -1;
			this.NewPos();
		}

		public void AssertFinished(string errorMessage = null)
		{
			if (!this.Finished)
			{
				if (string.IsNullOrEmpty(errorMessage))
				{
					this.ThrowUnexpectedCharacterException();
					return;
				}
				this.ThrowException(errorMessage);
			}
		}

		public void AssertNotFinished(string errorMessage = null)
		{
			if (this.Finished)
			{
				if (string.IsNullOrEmpty(errorMessage))
				{
					this.ThrowUnexpectedCharacterException();
					return;
				}
				this.ThrowException(errorMessage);
			}
		}

		public void Reset()
		{
			this.Index = 0;
			this.LastIndex = -1;
			this.Match = "";
			this.LastError = "";
			this.Success = true;
		}

		public void End()
		{
			this.CachePos();
			this.NewPos(this.Length);
		}

		public IStringScanner Expect(string text)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			this.AutoSkipWhitespace();
			if (this.Matches(text))
			{
				this.Match = this.Text.Substring(this.Index, text.Length);
				this.NewPos(this.Index + text.Length);
			}
			else
			{
				this.ThrowUnexpectedCharacterException();
			}
			return this;
		}

		public string Get(params string[] values)
		{
			this.Expect(values);
			return this.Match;
		}

		public void Expect(params string[] values)
		{
			this.Expect((IEnumerable<string>)values);
		}

		public string Get(IEnumerable<string> stringList)
		{
			this.Expect(stringList);
			return this.Match;
		}

		public bool TryGet(IEnumerable<string> stringList, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.Expect(stringList);
			}, out result);
		}

		public void Expect(IEnumerable<string> stringList)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			this.AutoSkipWhitespace();
			string text = "";
			foreach (string text2 in stringList)
			{
				text += text2[0];
			}
			if (this.ExpectCharImpl(text))
			{
				foreach (string text3 in stringList)
				{
					if (this.Matches(text3))
					{
						this.Match = this.Text.Substring(this.Index, text3.Length);
						this.NewPos(this.Index + text3.Length);
						return;
					}
				}
			}
			this.ThrowUnexpectedCharacterException();
		}

		public char GetChar(char character)
		{
			this.ExpectChar(character);
			return this.Match[0];
		}

		public bool TryGetChar(char character, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.ExpectChar(character);
			}, out result);
		}

		public IStringScanner ExpectChar(char character)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			this.AutoSkipWhitespace();
			if (this.Current == character)
			{
				char c = this.Current;
				this.Match = c.ToString();
				this.Next();
				this.NewPos();
			}
			else
			{
				this.ThrowUnexpectedCharacterException();
			}
			return this;
		}

		public char GetChar(string characters)
		{
			return this.GetChar(characters.ToCharArray());
		}

		public bool TryGetChar(string characters, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.Expect(characters);
			}, out result);
		}

		public IStringScanner ExpectChar(params char[] characters)
		{
			return this.ExpectChar((IEnumerable<char>)characters);
		}

		public char GetChar(IEnumerable<char> characters)
		{
			this.ExpectChar(characters);
			return this.Match[0];
		}

		public bool TryGetChar(IEnumerable<char> characters, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.ExpectChar(characters);
			}, out result);
		}

		public IStringScanner ExpectChar(IEnumerable<char> characters)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			this.AutoSkipWhitespace();
			if (this.ExpectCharImpl(characters))
			{
				char c = this.Current;
				this.Match = c.ToString();
				this.Next();
				this.NewPos();
			}
			else
			{
				this.ThrowUnexpectedCharacterException();
			}
			return this;
		}

		protected bool ExpectCharImpl(IEnumerable<char> characters)
		{
			foreach (char c in characters)
			{
				if (c == this.Current)
				{
					return true;
				}
			}
			return false;
		}

		public string GetNumber()
		{
			this.ExpectNumber(false);
			return this.Match;
		}

		public bool TryGetNumber(out string result)
		{
			return this.TryWrapper(delegate
			{
				this.ExpectNumber(false);
			}, out result);
		}

		public bool TryGetNumber<T>(out T result) where T : IConvertible
		{
			string value;
			bool flag = this.TryWrapper(delegate
			{
				this.ExpectNumber(false);
			}, out value);
			if (flag)
			{
				result = (T)((object)Convert.ChangeType(value, typeof(T)));
				return true;
			}
			result = default(T);
			return false;
		}

		public bool TryGetNumber(out int result)
		{
			double value;
			if (this.TryGetNumber<double>(out value))
			{
				result = Convert.ToInt32(value);
				return true;
			}
			result = 0;
			return false;
		}

		public IStringScanner ExpectNumber(bool requireWhitespaceTerminator = false)
		{
			return this.Expect(MatchFunctions.Number(requireWhitespaceTerminator));
		}

		public bool TryGetAlpha(out string result)
		{
			return this.TryWrapper(delegate
			{
				this.GetAlpha();
			}, out result);
		}

		public string GetAlpha()
		{
			this.ExpectAlpha();
			return this.Match;
		}

		public IStringScanner ExpectAlpha()
		{
			return this.Expect(new Func<int, char, bool>(MatchFunctions.Alpha));
		}

		public string Get(IExpectPattern pattern)
		{
			this.ExpectImpl(pattern);
			return this.Match;
		}

		public bool TryGet(IExpectPattern pattern, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.Expect(pattern);
			}, out result);
		}

		public IStringScanner Expect(IExpectPattern pattern)
		{
			this.ExpectImpl(pattern);
			return this;
		}

		public string Get(Func<int, char, bool> validate)
		{
			this.Expect(validate);
			return this.Match;
		}

		public bool TryGet(Func<int, char, bool> validate, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.Expect(validate);
			}, out result);
		}

		public IStringScanner Expect(Func<int, char, bool> validate)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			this.AutoSkipWhitespace();
			int index = this.Index;
			int num = 0;
			while (!this.Finished && validate(num, this.Current))
			{
				this.Index++;
				num++;
			}
			if (this.Index > index)
			{
				this.Match = this.Text.SubstringBetween(index, this.Index);
				this.NewPos();
			}
			else
			{
				this.ThrowUnexpectedCharacterException();
			}
			return this;
		}

		public string GetBoundedBy(string start, string end, bool allowQuoting = false)
		{
			this.ExpectBoundedBy(start, end, false);
			return this.Match;
		}

		public bool TryGetBoundedBy(string start, string end, bool allowQuoting, out string result)
		{
			return this.TryWrapper(delegate
			{
				this.ExpectBoundedBy(start, end, allowQuoting);
			}, out result);
		}

		public IStringScanner ExpectBoundedBy(string start, string end, bool allowQuoting = false)
		{
			return this.Expect(new Bounded
			{
				BoundStart = start,
				BoundEnd = end,
				HonorInnerQuotes = allowQuoting
			});
		}

		public string GetBoundedBy(char bound, bool allowQuoting = false)
		{
			this.ExpectBoundedBy(bound, allowQuoting);
			return this.Match;
		}

		public IStringScanner ExpectBoundedBy(char bound, bool allowQuoting = false)
		{
			return this.Expect(new Bounded
			{
				BoundStart = bound.ToString(),
				HonorInnerQuotes = allowQuoting
			});
		}

		public override string ToString()
		{
			return this.Text;
		}

		StringScannerEngine ExpectImpl(IExpectPattern pattern)
		{
			this.AssertNotFinished(null);
			this.CachePos();
			this.AutoSkipWhitespace();
			pattern.Initialize(this.Index, this.Chars);
			if (pattern.Validate())
			{
				this.Match = pattern.Result;
				this.NewPos(pattern.EndIndex);
			}
			else
			{
				this.Index = pattern.EndIndex;
				this.ThrowUnexpectedCharacterException();
			}
			return this;
		}

		protected bool TryWrapper(Action action, out string result)
		{
			this.SuppressErrors = true;
			action();
			this.SuppressErrors = false;
			if (this.Success)
			{
				result = this.Match;
			}
			else
			{
				result = "";
			}
			return this.Success;
		}

		[DebuggerStepThrough]
		protected void ThrowUnexpectedCharacterException()
		{
			if (this.Index >= this.Length)
			{
				this.ThrowUnexpectedEndOfStringException();
				return;
			}
			this.ThrowException("Unexpected character found", this.Index);
		}

		[DebuggerStepThrough]
		protected void ThrowUnexpectedEndOfStringException()
		{
			this.ThrowException("The string unexpectedly ended", this.Index);
		}

		[DebuggerStepThrough]
		protected void ThrowException(string message)
		{
			this.ThrowException(message, -1);
		}

		protected void ThrowException(string message, int errPos)
		{
			string text = message;
			int num = -1;
			if (string.IsNullOrEmpty(this.Text))
			{
				text = " -- the string is empty.";
			}
			else
			{
				num = System.Math.Min(errPos + 1, this.Length - 1);
			}
			this.RestorePos();
			if (num >= 0)
			{
				object obj = text;
				text = string.Concat(new object[] { obj, " at position ", num, ": \"" });
				if (this.Index != num)
				{
					if (this.Index > 0 && this.Index < this.Length)
					{
						text += ".. ";
					}
					object obj2 = text;
					text = string.Concat(new object[]
					{
						obj2,
						this.Text.SubstringBetween(Math.Max(this.Index - 10, 0), num),
						">>",
						this.Text[num],
						"<<"
					});
					if (num < this.Length - 1)
					{
						text += this.Text.SubstringBetween(num + 1, Math.Min(this.Length, num + 30));
					}
					text += "\"";
				}
			}
			this.LastError = text;
			if (this.SuppressErrors)
			{
				this.Success = false;
				return;
			}
			throw new ArgumentException(text);
		}

		protected void AutoSkipWhitespace()
		{
			if (this.IgnoreWhitespace)
			{
				this.SkipWhitespaceImpl();
			}
		}

		protected void NextNonWhitespaceImpl()
		{
			this.Next();
			this.SkipWhitespaceImpl();
		}

		protected void CachePos()
		{
			this.LastError = "";
			this.Success = true;
			if (this.cached)
			{
				throw new InvalidOperationException("Internal error: already cached");
			}
			this.cached = true;
			this.cachedPos = this.Index;
			this.cachedMatch = this.Match;
		}

		protected void NewPos(int pos)
		{
			this.Index = pos;
			this.NewPos();
		}

		protected void NewPos()
		{
			if (this.Index != this.cachedPos)
			{
				this.LastIndex = this.cachedPos;
				this.LastMatch = this.cachedMatch;
			}
			this.cached = false;
		}

		protected void RestorePos()
		{
			if (this.cached)
			{
				this.Index = this.cachedPos;
				this.Match = this.cachedMatch;
				this.cached = false;
			}
		}

		protected string LookupsToString(HashSet<char> list)
		{
			string text = string.Empty;
			foreach (char c in list)
			{
				text += c;
			}
			return text;
		}

		string _Text;

		string _CurrentMatch;

		string _LastMatch;

		int cachedPos;

		string cachedMatch;

		bool cached;

		CharacterInfo _characterInfo;

		protected char[] _Chars;
	}
}
