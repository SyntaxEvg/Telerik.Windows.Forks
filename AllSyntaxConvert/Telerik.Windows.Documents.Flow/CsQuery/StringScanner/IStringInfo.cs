using System;

namespace CsQuery.StringScanner
{
	interface IStringInfo : IValueInfo<string>, IValueInfo
	{
		bool HtmlAttributeName { get; }

		bool HasAlpha { get; }
	}
}
