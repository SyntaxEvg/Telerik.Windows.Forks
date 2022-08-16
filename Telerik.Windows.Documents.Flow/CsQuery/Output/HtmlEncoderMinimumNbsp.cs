using System;

namespace CsQuery.Output
{
	class HtmlEncoderMinimumNbsp : HtmlEncoderMinimum
	{
		protected override bool TryEncode(char c, out string encoded)
		{
			if (c == '\u00a0')
			{
				encoded = "&nbsp;";
				return true;
			}
			return base.TryEncode(c, out encoded);
		}
	}
}
