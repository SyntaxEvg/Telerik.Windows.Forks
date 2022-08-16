using System;

namespace CsQuery.EquationParser.Implementation
{
	interface IIntegerValue : IComparable, IConvertible, IEquatable<byte>, IEquatable<short>, IEquatable<ushort>, IEquatable<int>, IEquatable<uint>, IEquatable<long>, IEquatable<ulong>, IEquatable<double>, IEquatable<float>, IComparable<byte>, IComparable<short>, IComparable<ushort>, IComparable<int>, IComparable<uint>, IComparable<long>, IComparable<ulong>
	{
	}
}
