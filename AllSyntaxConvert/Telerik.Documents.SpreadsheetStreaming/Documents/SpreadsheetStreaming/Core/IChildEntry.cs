using System;

namespace Telerik.Documents.SpreadsheetStreaming.Core
{
	interface IChildEntry
	{
		bool IsUsageBegan { get; }

		bool IsUsageCompleted { get; }
	}
}
