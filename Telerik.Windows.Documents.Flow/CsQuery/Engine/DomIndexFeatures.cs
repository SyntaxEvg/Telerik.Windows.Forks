using System;

namespace CsQuery.Engine
{
	[Flags]
	enum DomIndexFeatures
	{
		Lookup = 1,
		Range = 2,
		Queue = 4
	}
}
