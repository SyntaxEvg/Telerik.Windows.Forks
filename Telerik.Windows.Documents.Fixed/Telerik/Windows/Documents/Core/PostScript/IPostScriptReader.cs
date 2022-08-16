using System;

namespace Telerik.Windows.Documents.Core.PostScript
{
	interface IPostScriptReader
	{
		bool EndOfFile { get; }

		byte Peek(int skip = 0);

		byte Read(bool appendLineFeed = true);
	}
}
