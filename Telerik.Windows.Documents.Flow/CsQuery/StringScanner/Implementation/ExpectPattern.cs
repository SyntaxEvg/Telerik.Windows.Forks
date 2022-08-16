using System;
using System.Text;

namespace CsQuery.StringScanner.Implementation
{
	abstract class ExpectPattern : IExpectPattern
	{
		public virtual void Initialize(int startIndex, char[] sourceText)
		{
			this.Source = sourceText;
			this.StartIndex = startIndex;
			this.Length = this.Source.Length;
		}

		public virtual bool Validate()
		{
			if (this.EndIndex > this.StartIndex)
			{
				this.Result = this.GetOuput(this.StartIndex, this.EndIndex, false);
				return true;
			}
			this.Result = "";
			return false;
		}

		public virtual int EndIndex { get; protected set; }

		public virtual string Result { get; protected set; }

		protected bool MatchSubstring(int startIndex, string substring)
		{
			if (startIndex + substring.Length <= this.Source.Length)
			{
				for (int i = 0; i < substring.Length; i++)
				{
					if (this.Source[startIndex + i] != substring[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		protected string GetOuput(int startIndex, int endIndex, bool honorQuotes)
		{
			return this.GetOuput(startIndex, endIndex, honorQuotes, false);
		}

		protected string GetOuput(int startIndex, int endIndex, bool honorQuotes, bool stripQuotes)
		{
			bool flag = false;
			char c = '\0';
			StringBuilder stringBuilder = new StringBuilder();
			int i = startIndex;
			if (endIndex <= i)
			{
				return "";
			}
			if (stripQuotes && CharacterData.IsType(this.Source[i], CharacterType.Quote))
			{
				flag = true;
				c = this.Source[i];
				i++;
				endIndex--;
			}
			while (i < endIndex)
			{
				char c2 = this.Source[i];
				this.info.Target = c2;
				if (honorQuotes)
				{
					if (!flag)
					{
						if (this.info.Quote)
						{
							flag = true;
							c = c2;
						}
					}
					else if (c2 == c)
					{
						flag = false;
					}
				}
				stringBuilder.Append(c2);
				i++;
			}
			return stringBuilder.ToString();
		}

		protected bool TryParseEscapeChar(char character, out char newValue)
		{
			if (character <= '\'')
			{
				if (character != '"' && character != '\'')
				{
					goto IL_2A;
				}
			}
			else if (character != '\\')
			{
				if (character != 'n')
				{
					goto IL_2A;
				}
				newValue = '\n';
				return true;
			}
			newValue = character;
			return true;
			IL_2A:
			newValue = ' ';
			return false;
		}

		protected ICharacterInfo info = CharacterData.CreateCharacterInfo();

		protected char[] Source;

		protected int StartIndex;

		protected int Length;
	}
}
