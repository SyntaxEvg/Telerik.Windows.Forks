using System;
using System.Collections.Generic;

namespace CsQuery
{
	interface ICSSStyle
	{
		string Name { get; set; }

		CSSStyleType Type { get; set; }

		string Format { get; set; }

		HashSet<string> Options { get; set; }

		string Description { get; set; }
	}
}
