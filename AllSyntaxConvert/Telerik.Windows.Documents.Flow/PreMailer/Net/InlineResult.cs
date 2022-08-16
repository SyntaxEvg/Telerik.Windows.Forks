using System;
using System.Collections.Generic;
using System.Linq;

namespace PreMailer.Net
{
	class InlineResult
	{
		public IEnumerable<string> Warnings { get; protected set; }

		public InlineResult(IEnumerable<string> warnings = null)
		{
			this.Warnings = warnings ?? Enumerable.Empty<string>();
		}
	}
}
