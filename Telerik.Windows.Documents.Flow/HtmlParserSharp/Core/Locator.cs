using System;

namespace HtmlParserSharp.Core
{
	class Locator : ILocator
	{
		public int ColumnNumber { get; set; }

		public int LineNumber { get; set; }

		public Locator(ILocator locator)
		{
			this.ColumnNumber = locator.ColumnNumber;
			this.LineNumber = locator.LineNumber;
		}
	}
}
