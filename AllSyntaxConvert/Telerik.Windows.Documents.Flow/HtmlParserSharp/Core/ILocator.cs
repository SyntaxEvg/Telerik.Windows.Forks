using System;

namespace HtmlParserSharp.Core
{
	interface ILocator
	{
		int LineNumber { get; }

		int ColumnNumber { get; }
	}
}
