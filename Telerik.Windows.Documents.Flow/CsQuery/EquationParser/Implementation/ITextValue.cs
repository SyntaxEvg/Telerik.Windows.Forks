using System;

namespace CsQuery.EquationParser.Implementation
{
	interface ITextValue : IComparable, IConvertible, IComparable<string>, IComparable<char>, IEquatable<string>, IEquatable<char>
	{
	}
}
