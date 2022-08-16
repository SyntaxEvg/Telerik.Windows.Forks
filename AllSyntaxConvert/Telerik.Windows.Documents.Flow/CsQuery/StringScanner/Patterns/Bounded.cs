using System;
using CsQuery.StringScanner.Implementation;

namespace CsQuery.StringScanner.Patterns
{
	class Bounded : ExpectPattern
	{
		public bool HonorInnerQuotes { get; set; }

		public string BoundStart
		{
			get
			{
				return this._BoundStart;
			}
			set
			{
				this.boundAny = false;
				if (value.Length == 0)
				{
					this.boundAny = true;
				}
				else
				{
					this.BoundStartChar = value[0];
				}
				this._BoundStart = value;
			}
		}

		public string BoundEnd
		{
			get
			{
				return this._BoundEnd;
			}
			set
			{
				this.boundAny = false;
				this._BoundEnd = value;
				this._BoundEndChar = value[0];
			}
		}

		protected char BoundStartChar
		{
			get
			{
				return this._BoundStartChar;
			}
			set
			{
				this._BoundStartChar = value;
				this.BoundEnd = CharacterData.MatchingBound(value).ToString();
			}
		}

		protected char BoundEndChar
		{
			get
			{
				return this._BoundEndChar;
			}
			set
			{
				this._BoundEndChar = value;
			}
		}

		public override void Initialize(int startIndex, char[] sourceText)
		{
			base.Initialize(startIndex, sourceText);
			this.hasStartBound = this.BoundStartChar != '\0' || this.boundAny;
			this.nestedCount = 0;
			this.matched = false;
			this.quoting = false;
		}

		public override bool Validate()
		{
			int startIndex = this.StartIndex;
			while (startIndex < this.Source.Length && this.Expect(ref startIndex, this.Source[startIndex]))
			{
			}
			this.EndIndex = startIndex;
			if (this.EndIndex > this.Length || this.EndIndex == this.StartIndex || !this.matched)
			{
				this.Result = "";
				return false;
			}
			this.Result = base.GetOuput(this.StartIndex + this.BoundStart.Length, this.EndIndex - this.BoundEnd.Length, false);
			return true;
		}

		protected bool Expect(ref int index, char current)
		{
			this.info.Target = current;
			if (!this.quoting)
			{
				if (this.hasStartBound)
				{
					if (index == this.StartIndex)
					{
						if (this.boundAny && this.info.Bound)
						{
							this.BoundStart = current.ToString();
							this.BoundEnd = CharacterData.Closer(current).ToString();
							return this.info.Bound && index++ < this.Length;
						}
						if (base.MatchSubstring(index, this.BoundStart))
						{
							index += this.BoundStart.Length;
							return true;
						}
					}
					else if (current == this.BoundStartChar && base.MatchSubstring(index, this.BoundStart))
					{
						index += this.BoundStart.Length;
						this.nestedCount++;
						return true;
					}
				}
				if (current == this.BoundEndChar)
				{
					if (this.boundAny)
					{
						if (this.nestedCount == 0)
						{
							this.matched = true;
							index++;
							return false;
						}
						this.nestedCount--;
					}
					else if (base.MatchSubstring(index, this.BoundEnd))
					{
						if (this.nestedCount == 0)
						{
							index += this.BoundEnd.Length;
							this.matched = true;
							return false;
						}
						this.nestedCount--;
					}
				}
			}
			if (this.HonorInnerQuotes)
			{
				if (!this.quoting)
				{
					if (this.info.Quote)
					{
						this.quoting = true;
						this.quoteChar = current;
					}
				}
				else
				{
					bool flag = this.Source[index - 1] == '\\';
					if (current == this.quoteChar && !flag)
					{
						this.quoting = false;
					}
				}
			}
			index++;
			return true;
		}

		string _BoundStart = "";

		string _BoundEnd = "";

		char _BoundStartChar;

		char _BoundEndChar;

		bool hasStartBound;

		protected bool boundAny = true;

		bool quoting;

		char quoteChar;

		bool matched;

		int nestedCount;
	}
}
