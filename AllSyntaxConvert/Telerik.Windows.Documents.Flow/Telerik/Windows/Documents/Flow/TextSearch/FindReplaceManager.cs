using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	static class FindReplaceManager
	{
		public static IEnumerable<RunTextMatch> Find(Regex regex, RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			FindTextService findTextService = new FindTextService(document);
			return findTextService.Find(regex);
		}

		public static void Replace(IEnumerable<RunTextMatch> matches, Regex regex, string replaceText, ReplaceHandlerBase replaceHandler)
		{
			ReplaceTextService replaceTextService = new ReplaceTextService(replaceHandler);
			replaceTextService.Replace(matches, regex, replaceText);
		}
	}
}
