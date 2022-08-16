using System;
using System.IO;

namespace CsQuery.Output
{
	interface IHtmlEncoder
	{
		void Encode(string text, TextWriter output);
	}
}
