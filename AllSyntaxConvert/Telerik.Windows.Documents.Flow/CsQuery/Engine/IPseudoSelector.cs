using System;

namespace CsQuery.Engine
{
	interface IPseudoSelector
	{
		string Arguments { get; set; }

		int MinimumParameterCount { get; }

		int MaximumParameterCount { get; }

		string Name { get; }
	}
}
