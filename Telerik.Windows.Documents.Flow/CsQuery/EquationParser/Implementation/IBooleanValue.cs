using System;

namespace CsQuery.EquationParser.Implementation
{
	interface IBooleanValue : IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
	{
	}
}
