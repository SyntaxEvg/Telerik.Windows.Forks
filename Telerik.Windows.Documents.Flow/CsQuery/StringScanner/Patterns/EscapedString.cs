using System;
using System.Globalization;
using System.Text;
using CsQuery.StringScanner.Implementation;

namespace CsQuery.StringScanner.Patterns
{
	class EscapedString : ExpectPattern
	{
		public EscapedString()
			: this(new Func<int, char, bool>(EscapedString.AlwaysValid))
		{
		}

		public EscapedString(Func<int, char, bool> validCharacter)
		{
			this.ValidCharacter = validCharacter;
		}

		static bool AlwaysValid(int index, char character)
		{
			return true;
		}

		public override bool Validate()
		{
			int num = this.StartIndex;
			int num2 = 0;
			bool flag = false;
			this.Result = "";
			while (num < this.Source.Length && !flag)
			{
				char c = this.Source[num];
				if (!this.Escaped && c == '\\')
				{
					this.Escaped = true;
				}
				else
				{
					if (this.Escaped)
					{
						int num3 = num;
						StringBuilder stringBuilder = new StringBuilder();
						while (num3 < this.Source.Length && num3 - num < 6 && CharacterData.IsType(this.Source[num3], CharacterType.Hexadecimal))
						{
							stringBuilder.Append(this.Source[num3]);
							num3++;
						}
						if (stringBuilder.Length >= 1)
						{
							int num4 = 0;
							if (int.TryParse(stringBuilder.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num4))
							{
								c = (char)num4;
								num = num3;
							}
							if (stringBuilder.Length < 6 && num < this.Source.Length && CharacterData.IsType(this.Source[num], CharacterType.Whitespace))
							{
								num++;
								num2++;
							}
							num--;
							num2--;
						}
						else
						{
							stringBuilder.Append(this.Source[num3]);
						}
						this.Escaped = false;
					}
					else if (!this.ValidCharacter(num2, c))
					{
						flag = true;
						continue;
					}
					this.Result += c;
				}
				num++;
				num2++;
			}
			bool escaped = this.Escaped;
			this.EndIndex = num;
			if (this.EndIndex > this.Length || this.EndIndex == this.StartIndex || escaped)
			{
				this.Result = "";
				return false;
			}
			return true;
		}

		protected Func<int, char, bool> ValidCharacter;

		protected bool Escaped;

		protected bool failed;
	}
}
