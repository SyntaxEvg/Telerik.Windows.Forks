using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	class FindTextService
	{
		public FindTextService(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.documentTextInfo = new DocumentTextInfo(document);
		}

		public IEnumerable<RunTextMatch> Find(Regex regex)
		{
			Guard.ThrowExceptionIfNull<Regex>(regex, "regex");
			int startIndex = 0;
			int initialRunTextLength = 0;
			int actualRunTextLength = 0;
			foreach (object obj in regex.Matches(this.documentTextInfo.Text))
			{
				Match match = (Match)obj;
				Run matchedRun;
				if (this.documentTextInfo.TryGetRun(match.Index, out matchedRun))
				{
					initialRunTextLength = this.documentTextInfo.GetLastRunInterval().End - this.documentTextInfo.GetLastRunInterval().Start;
					actualRunTextLength = matchedRun.Text.Length;
					startIndex = match.Index - this.documentTextInfo.GetLastRunInterval().Start + (actualRunTextLength - initialRunTextLength);
					int matchIndex = RunMatchHelper.GetMatchedIndexInRun(matchedRun, match.Value, startIndex);
					int matchLength = RunMatchHelper.GetMatchedTextLengthInRun(matchedRun, match.Value, matchIndex);
					yield return new RunTextMatch(matchedRun, matchIndex, matchLength, match.Value);
				}
			}
			yield break;
		}

		internal static Regex BuildRegex(string pattern, bool matchCase, bool matchWholeWord)
		{
			pattern = Regex.Escape(pattern);
			RegexOptions options = RegexOptions.None;
			if (!matchCase)
			{
				options = RegexOptions.IgnoreCase;
			}
			if (matchWholeWord)
			{
				pattern = "\\b" + pattern + "\\b";
			}
			return new Regex(pattern, options);
		}

		const string WholeWordRegexPattern = "\\b";

		readonly DocumentTextInfo documentTextInfo;
	}
}
