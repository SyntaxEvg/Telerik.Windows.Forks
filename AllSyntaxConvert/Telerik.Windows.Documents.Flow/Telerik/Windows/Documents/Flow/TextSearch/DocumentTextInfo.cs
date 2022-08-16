using System;
using System.Text;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.TextSearch.Core;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	class DocumentTextInfo
	{
		public DocumentTextInfo(RadFlowDocument document)
		{
			this.document = document;
			this.textIntervalToRunMap = new IntervalMap<int, Run>();
			this.documentAsText = this.BuildSearchableMappedText();
		}

		public string Text
		{
			get
			{
				return this.documentAsText;
			}
		}

		public bool TryGetRun(int textPositionIndex, out Run run)
		{
			run = this.textIntervalToRunMap.GetValueFromIntervalPoint(textPositionIndex);
			return run != null;
		}

		public Interval<int> GetLastRunInterval()
		{
			return this.textIntervalToRunMap.LastIntervalCached;
		}

		string BuildSearchableMappedText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Paragraph paragraph = null;
			int num = 0;
			int num2 = 0;
			foreach (InlineBase inlineBase in this.document.EnumerateChildrenOfType<InlineBase>())
			{
				Run run = inlineBase as Run;
				if (run != null)
				{
					if (paragraph != null && run.Paragraph != paragraph)
					{
						stringBuilder.Append('\u001f');
						num++;
					}
					stringBuilder.Append(run.Text);
					num2 = num + run.Text.Length;
					this.textIntervalToRunMap.Add(num, num2, run);
				}
				else
				{
					stringBuilder.Append('\u001f');
					num2++;
				}
				num = num2;
				paragraph = inlineBase.Paragraph;
			}
			return stringBuilder.ToString();
		}

		const char UnitSeparatorChar = '\u001f';

		readonly RadFlowDocument document;

		readonly IntervalMap<int, Run> textIntervalToRunMap;

		readonly string documentAsText;
	}
}
