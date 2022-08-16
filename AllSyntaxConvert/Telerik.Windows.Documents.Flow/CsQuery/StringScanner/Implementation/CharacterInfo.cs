using System;

namespace CsQuery.StringScanner.Implementation
{
	class CharacterInfo : ICharacterInfo, IValueInfo<char>, IValueInfo
	{
		public CharacterInfo()
		{
		}

		public CharacterInfo(char character)
		{
			this.Target = character;
		}

		public static implicit operator CharacterInfo(char character)
		{
			return new CharacterInfo(character);
		}

		public static ICharacterInfo Create(char character)
		{
			return new CharacterInfo(character);
		}

		public char Target { get; set; }

		IConvertible IValueInfo.Target
		{
			get
			{
				return this.Target;
			}
			set
			{
				this.Target = (char)value;
			}
		}

		public CharacterType Type
		{
			get
			{
				return CharacterData.GetType(this.Target);
			}
		}

		public bool Alpha
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Alpha);
			}
		}

		public bool Numeric
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Number);
			}
		}

		public bool NumericExtended
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.NumberPart);
			}
		}

		public bool Lower
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Lower);
			}
		}

		public bool Upper
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Upper);
			}
		}

		public bool Whitespace
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Whitespace);
			}
		}

		public bool Alphanumeric
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Alpha | CharacterType.Number);
			}
		}

		public bool Operator
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Operator);
			}
		}

		public bool Bound
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Enclosing | CharacterType.Quote);
			}
		}

		public bool Enclosing
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Enclosing);
			}
		}

		public bool Quote
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Quote);
			}
		}

		public bool Parenthesis
		{
			get
			{
				return this.Target == '(' || this.Target == ')';
			}
		}

		public bool Separator
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.Separator);
			}
		}

		public bool AlphaISO10646
		{
			get
			{
				return CharacterData.IsType(this.Target, CharacterType.AlphaISO10646);
			}
		}

		public override string ToString()
		{
			return this.Target.ToString();
		}
	}
}
