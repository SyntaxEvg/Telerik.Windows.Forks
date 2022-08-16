using System;
using System.IO;

namespace CsQuery.Output
{
	class HtmlEncoderNone : IHtmlEncoder
	{
		public void Encode(string text, TextWriter output)
		{
			output.Write(text);
		}
	}
}
