using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	class ReplaceTextHandler : ReplaceHandlerBase
	{
		protected string ReplaceText { get; set; }

		internal static void ReplaceTextInternal(RunTextMatch runTextMatch, string replaceText)
		{
			Run run = runTextMatch.Run;
			string text = run.Text.Substring(runTextMatch.StartIndex, runTextMatch.Length);
			string fullMatchText = runTextMatch.FullMatchText;
			run.Text = run.Text.Remove(runTextMatch.StartIndex, runTextMatch.Length);
			run.Text = run.Text.Insert(runTextMatch.StartIndex, replaceText);
			if (text != fullMatchText)
			{
				List<Run> list = new List<Run>();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(text);
				Paragraph paragraph = run.Paragraph;
				string text2 = fullMatchText.Substring(text.Length, fullMatchText.Length - runTextMatch.Length);
				int num = paragraph.Inlines.IndexOf(run) + 1;
				for (int i = num; i < paragraph.Inlines.Count; i++)
				{
					Run run2 = (Run)paragraph.Inlines[i];
					int num2 = 0;
					while (num2 < Math.Min(run2.Text.Length, text2.Length) && run2.Text[num2] == text2[num2])
					{
						stringBuilder.Append(text2[num2]);
						num2++;
					}
					if (run2.Text.Length == num2)
					{
						list.Add(run2);
					}
					else if (run2.Text.Length > num2)
					{
						run2.Text = run2.Text.Substring(num2, run2.Text.Length - num2);
					}
					if (string.Equals(stringBuilder.ToString(), fullMatchText))
					{
						break;
					}
					text2 = text2.Remove(0, num2);
				}
				foreach (Run run3 in list)
				{
					run3.Paragraph.Inlines.Remove(run3);
				}
				list.Clear();
				stringBuilder.Clear();
			}
		}

		protected internal override void ReplaceMatch(RunTextMatch runTextMatch)
		{
			ReplaceTextHandler.ReplaceTextInternal(runTextMatch, this.ReplaceText);
		}

		protected override void BeginReplaceCore(string replaceText)
		{
			base.BeginReplaceCore(replaceText);
			this.ReplaceText = replaceText;
		}
	}
}
