using System;

namespace HtmlParserSharp.Core
{
	class TaintableLocator : Locator
	{
		public TaintableLocator(ILocator locator)
			: base(locator)
		{
			this.IsTainted = false;
		}

		public void MarkTainted()
		{
			this.IsTainted = true;
		}

		public bool IsTainted { get; set; }
	}
}
