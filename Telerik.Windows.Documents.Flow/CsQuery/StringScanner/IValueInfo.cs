using System;

namespace CsQuery.StringScanner
{
	interface IValueInfo
	{
		bool Alpha { get; }

		bool Numeric { get; }

		bool NumericExtended { get; }

		bool Lower { get; }

		bool Upper { get; }

		bool Whitespace { get; }

		bool Alphanumeric { get; }

		bool Operator { get; }

		bool AlphaISO10646 { get; }

		IConvertible Target { get; set; }
	}
}
