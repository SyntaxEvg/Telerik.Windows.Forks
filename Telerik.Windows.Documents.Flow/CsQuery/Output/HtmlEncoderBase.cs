using System;
using System.IO;

namespace CsQuery.Output
{
	abstract class HtmlEncoderBase : IHtmlEncoder
	{
		protected abstract bool TryEncode(char c, out string encoded);

		protected abstract bool TryEncodeAstralPlane(int c, out string encoded);

		public virtual void Encode(string html, TextWriter output)
		{
			int i = 0;
			int length = html.Length;
			while (i < length)
			{
				char c = html[i++];
				string value;
				if ((c & '\uf800') == '\ud800')
				{
					char c2 = html[i++];
					int c3 = (int)((c - '\ud800') * 'Ѐ' + (c2 - '\udc00')) + 65536;
					if (this.TryEncodeAstralPlane(c3, out value))
					{
						output.Write(value);
					}
					else
					{
						output.Write(c);
						output.Write(c2);
					}
				}
				else if (this.TryEncode(c, out value))
				{
					output.Write(value);
				}
				else
				{
					output.Write(c);
				}
			}
		}
	}
}
