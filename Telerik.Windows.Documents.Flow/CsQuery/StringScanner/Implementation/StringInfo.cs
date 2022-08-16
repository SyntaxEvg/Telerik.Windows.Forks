using System;
using System.Linq;

namespace CsQuery.StringScanner.Implementation
{
	class StringInfo : IStringInfo, IValueInfo<string>, IValueInfo
	{
		public StringInfo()
		{
		}

		public StringInfo(string text)
		{
			this.Target = text;
		}

		public static implicit operator StringInfo(string text)
		{
			return new StringInfo(text);
		}

		public static StringInfo Create(string text)
		{
			return new StringInfo(text);
		}

		protected bool CheckFor(Func<CharacterInfo, bool> function)
		{
			foreach (char target2 in this.Target)
			{
				this.charInfo.Target = target2;
				if (!function(this.charInfo))
				{
					return false;
				}
			}
			return true;
		}

		public string Target { get; set; }

		IConvertible IValueInfo.Target
		{
			get
			{
				return this.Target;
			}
			set
			{
				this.Target = (string)value;
			}
		}

		public bool Alpha
		{
			get
			{
				return this.Exists && this.CheckFor(this.isAlpha);
			}
		}

		public bool Numeric
		{
			get
			{
				return this.Exists && this.CheckFor(StringInfo.isNumeric);
			}
		}

		public bool NumericExtended
		{
			get
			{
				return this.Exists && this.CheckFor(StringInfo.isNumericExtended);
			}
		}

		public bool Lower
		{
			get
			{
				return this.Exists && this.HasAlpha && this.CheckFor(StringInfo.isLower);
			}
		}

		public bool Upper
		{
			get
			{
				return this.Exists && this.HasAlpha && this.CheckFor(StringInfo.isUpper);
			}
		}

		public bool Whitespace
		{
			get
			{
				return this.Exists && this.CheckFor(StringInfo.isWhitespace);
			}
		}

		public bool Alphanumeric
		{
			get
			{
				return this.Exists && this.CheckFor(StringInfo.isAlphanumeric);
			}
		}

		public bool Operator
		{
			get
			{
				return this.Exists && this.CheckFor(this.isOperator);
			}
		}

		public bool HasAlpha
		{
			get
			{
				foreach (char target2 in this.Target)
				{
					this.charInfo.Target = target2;
					if (this.charInfo.Alpha)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool HtmlAttributeName
		{
			get
			{
				if (!this.Exists)
				{
					return false;
				}
				this.charInfo.Target = this.Target[0];
				if (!this.charInfo.Alpha && this.charInfo.Target != ':' && this.charInfo.Target != '_')
				{
					return false;
				}
				for (int i = 1; i < this.Target.Length; i++)
				{
					this.charInfo.Target = this.Target[i];
					if (!this.charInfo.Alphanumeric && !"_:.-".Contains(this.charInfo.Target))
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool AlphaISO10646
		{
			get
			{
				return this.Exists && this.CheckFor(this.isAlphaISO10646);
			}
		}

		public override string ToString()
		{
			return this.Target;
		}

		protected bool Exists
		{
			get
			{
				return !string.IsNullOrEmpty(this.Target);
			}
		}

		protected CharacterInfo charInfo = new CharacterInfo();

		protected Func<CharacterInfo, bool> isAlpha = (CharacterInfo item) => item.Alpha;

		static Func<CharacterInfo, bool> isNumeric = (CharacterInfo item) => item.Numeric;

		static Func<CharacterInfo, bool> isNumericExtended = (CharacterInfo item) => item.NumericExtended;

		static Func<CharacterInfo, bool> isLower = (CharacterInfo item) => !item.Alpha || item.Lower;

		static Func<CharacterInfo, bool> isUpper = (CharacterInfo item) => !item.Alpha || item.Upper;

		static Func<CharacterInfo, bool> isWhitespace = (CharacterInfo item) => item.Whitespace;

		static Func<CharacterInfo, bool> isAlphanumeric = (CharacterInfo item) => item.Alpha || item.Numeric;

		protected Func<CharacterInfo, bool> isOperator = (CharacterInfo item) => item.Operator;

		protected Func<CharacterInfo, bool> isAlphaISO10646 = (CharacterInfo item) => item.AlphaISO10646;
	}
}
