using System;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	class CssStyle : ICSSStyle
	{
		public string Name { get; set; }

		public CSSStyleType Type { get; set; }

		public string Format { get; set; }

		public string Description { get; set; }

		public HashSet<string> Options { get; set; }
	}
}
