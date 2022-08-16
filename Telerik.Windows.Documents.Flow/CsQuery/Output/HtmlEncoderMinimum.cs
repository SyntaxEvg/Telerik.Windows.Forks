using System;

namespace CsQuery.Output
{
	class HtmlEncoderMinimum : HtmlEncoderBase
	{
		protected override bool TryEncode(char c, out string encoded)
		{
			if (c != '&')
			{
				switch (c)
				{
				case '<':
					encoded = "&lt;";
					return true;
				case '>':
					encoded = "&gt;";
					return true;
				}
				encoded = null;
				return false;
			}
			encoded = "&amp;";
			return true;
		}

		protected override bool TryEncodeAstralPlane(int c, out string encoded)
		{
			encoded = null;
			return false;
		}
	}
}
