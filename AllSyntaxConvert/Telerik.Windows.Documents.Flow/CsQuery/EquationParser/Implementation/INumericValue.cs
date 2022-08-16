using System;

namespace CsQuery.EquationParser.Implementation
{
	interface INumericValue : IIntegerValue, IComparable, IConvertible, IEquatable<byte>, IEquatable<short>, IEquatable<ushort>, IEquatable<int>, IEquatable<uint>, IEquatable<long>, IEquatable<ulong>, IComparable<byte>, IComparable<short>, IComparable<ushort>, IComparable<int>, IComparable<uint>, IComparable<long>, IComparable<ulong>, IComparable<double>, IComparable<float>, IComparable<decimal>, IEquatable<double>, IEquatable<float>, IEquatable<decimal>
	{
	}
}
