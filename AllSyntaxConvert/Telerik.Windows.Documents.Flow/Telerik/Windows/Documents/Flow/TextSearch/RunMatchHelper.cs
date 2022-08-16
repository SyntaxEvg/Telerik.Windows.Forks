using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	static class RunMatchHelper
	{
		public static int GetMatchedIndexInRun(Run matchedRun, string searchedText, int startIndex = 0)
		{
			startIndex = Math.Max(0, startIndex);
			int num = matchedRun.Text.IndexOf(searchedText, startIndex);
			if (num == -1)
			{
				string text = searchedText.Substring(0, searchedText.Length - 1);
				while (num == -1 && !string.IsNullOrEmpty(text))
				{
					num = matchedRun.Text.IndexOf(text, startIndex);
					text = text.Substring(0, text.Length - 1);
				}
			}
			if (num == -1)
			{
				return RunMatchHelper.GetMatchedIndexInRun(matchedRun, searchedText, 0);
			}
			return num;
		}

		public static int GetMatchedTextLengthInRun(Run matchedRun, string searchedText, int matchIndex)
		{
			return Math.Min(searchedText.Length, matchedRun.Text.Length - matchIndex);
		}
	}
}
