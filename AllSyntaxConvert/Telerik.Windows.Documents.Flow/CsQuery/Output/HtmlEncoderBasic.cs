using System;
using System.Globalization;

namespace CsQuery.Output
{
	class HtmlEncoderBasic : HtmlEncoderBase
	{
		protected override bool TryEncode(char c, out string encoded)
		{
			if (c <= '&')
			{
				if (c == '"')
				{
					encoded = "&quot;";
					return true;
				}
				if (c == '&')
				{
					encoded = "&amp;";
					return true;
				}
			}
			else
			{
				switch (c)
				{
				case '<':
					encoded = "&lt;";
					return true;
				case '=':
					break;
				case '>':
					encoded = "&gt;";
					return true;
				default:
					if (c == '\u00a0')
					{
						encoded = "&nbsp;";
						return true;
					}
					break;
				}
			}
			if (c > '\u00a0')
			{
				encoded = this.EncodeNumeric((int)c);
				return true;
			}
			encoded = null;
			return false;
		}

		protected override bool TryEncodeAstralPlane(int c, out string encoded)
		{
			encoded = this.EncodeNumeric(c);
			return true;
		}

		protected string EncodeNumeric(int value)
		{
			return "&#" + value.ToString(CultureInfo.InvariantCulture) + ";";
		}
	}
}
