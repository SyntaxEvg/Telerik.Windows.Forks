using System;

namespace CsQuery.StringScanner
{
	interface IValueInfo<T> : IValueInfo where T : IConvertible
	{
		T Target { get; set; }
	}
}
