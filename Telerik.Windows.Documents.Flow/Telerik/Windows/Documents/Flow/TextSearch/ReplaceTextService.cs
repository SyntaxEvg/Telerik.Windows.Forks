using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	class ReplaceTextService
	{
		public ReplaceTextService(ReplaceHandlerBase replaceHandler)
		{
			Guard.ThrowExceptionIfNull<ReplaceHandlerBase>(replaceHandler, "replaceHandler");
			this.replaceHandler = replaceHandler;
		}

		public void Replace(IEnumerable<RunTextMatch> matches, Regex regex, string replaceText)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<RunTextMatch>>(matches, "matches");
			this.replaceHandler.BeginReplace(regex, replaceText);
			foreach (RunTextMatch runTextMatch in matches)
			{
				this.replaceHandler.ReplaceMatch(runTextMatch);
			}
			this.replaceHandler.EndReplace();
		}

		readonly ReplaceHandlerBase replaceHandler;
	}
}
