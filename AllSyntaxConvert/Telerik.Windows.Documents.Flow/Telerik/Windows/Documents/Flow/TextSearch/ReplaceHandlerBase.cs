using System;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	abstract class ReplaceHandlerBase
	{
		protected Regex Regex { get; set; }

		internal void BeginReplace(Regex regex, string replaceText)
		{
			Guard.ThrowExceptionIfNull<Regex>(regex, "regex");
			this.Regex = regex;
			this.BeginReplaceCore(replaceText);
		}

		internal void EndReplace()
		{
			this.EndReplaceCore();
		}

		protected internal abstract void ReplaceMatch(RunTextMatch runTextMatch);

		protected virtual void BeginReplaceCore(string replaceText)
		{
		}

		protected virtual void EndReplaceCore()
		{
		}
	}
}
