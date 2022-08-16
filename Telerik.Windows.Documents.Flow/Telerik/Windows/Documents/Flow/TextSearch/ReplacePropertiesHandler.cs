using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Flow.TextSearch.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	class ReplacePropertiesHandler : ReplaceHandlerBase
	{
		public ReplacePropertiesHandler(Action<CharacterProperties> replacePropertiesAction)
		{
			Guard.ThrowExceptionIfNull<Action<CharacterProperties>>(replacePropertiesAction, "replacePropertiesAction");
			this.replacePropertiesAction = replacePropertiesAction;
			this.runsToRemove = new HashSet<Run>();
			this.runsToMerge = new IntervalMap<int, Run>();
			this.replaceTexts = new Queue<string>();
			this.lastMatchedRun = null;
		}

		bool HasReplaceTextsQueued
		{
			get
			{
				return this.replaceTexts.Count != 0;
			}
		}

		protected internal override void ReplaceMatch(RunTextMatch runTextMatch)
		{
			this.lastReplaceText = runTextMatch.FullMatchText;
			this.replaceTexts.Enqueue(runTextMatch.FullMatchText);
			ReplaceTextHandler.ReplaceTextInternal(runTextMatch, runTextMatch.FullMatchText);
			if (this.lastMatchedRun != runTextMatch.Run)
			{
				this.ReplaceProperties(this.lastMatchedRun);
			}
			this.lastMatchedRun = runTextMatch.Run;
		}

		protected override void EndReplaceCore()
		{
			base.EndReplaceCore();
			if (this.lastMatchedRun != this.runsToRemove.LastOrDefault<Run>())
			{
				this.ReplaceProperties(this.lastMatchedRun);
			}
			this.MergeProcessedRuns();
			this.Clear();
		}

		static List<Run> SplitRun(Regex regex, Run originalRun)
		{
			List<string> list = ReplacePropertiesHandler.SplitString(regex, originalRun.Text);
			if (string.IsNullOrEmpty(list.FirstOrDefault<string>()))
			{
				list.RemoveAt(0);
			}
			return ReplacePropertiesHandler.GetRuns(list, originalRun);
		}

		static List<string> SplitString(Regex regex, string input)
		{
			List<string> list = new List<string>();
			Match match = regex.Match(input, 0);
			if (!match.Success)
			{
				list.Add(input);
				return list;
			}
			int num = 0;
			do
			{
				list.Add(input.Substring(num, match.Index - num));
				num = match.Index + match.Length;
				match = match.NextMatch();
			}
			while (match.Success);
			list.Add(input.Substring(num, input.Length - num));
			return list;
		}

		static List<Run> GetRuns(IEnumerable<string> oldRunTextSplitted, Run originalRun)
		{
			List<Run> list = new List<Run>();
			foreach (string runText in oldRunTextSplitted)
			{
				list.Add(ReplacePropertiesHandler.CreateRunCopy(originalRun, runText));
			}
			return list;
		}

		static Run CreateRunCopy(Run runTemplate, string runText)
		{
			Run run = new Run(runTemplate.Document);
			run.Text = runText;
			run.Properties.CopyPropertiesFrom(runTemplate.Properties);
			return run;
		}

		static bool ArePropertiesEqual(CharacterProperties characterProperties, CharacterProperties otherCharacterProperties)
		{
			foreach (IStyleProperty styleProperty in characterProperties.StyleProperties)
			{
				IStyleProperty styleProperty2 = otherCharacterProperties.GetStyleProperty(styleProperty.PropertyDefinition);
				object actualValueAsObject = styleProperty.GetActualValueAsObject();
				object actualValueAsObject2 = styleProperty2.GetActualValueAsObject();
				if ((actualValueAsObject != null || actualValueAsObject2 != null) && actualValueAsObject != null && !actualValueAsObject.Equals(actualValueAsObject2))
				{
					return false;
				}
			}
			return true;
		}

		void ReplaceProperties(Run originalRun)
		{
			if (originalRun == null)
			{
				return;
			}
			Paragraph paragraph = originalRun.Paragraph;
			if (this.lastRunParent != paragraph)
			{
				this.paragraphTextLength = 0;
			}
			int num = paragraph.Inlines.IndexOf(originalRun);
			if (originalRun.Text == this.replaceTexts.Peek())
			{
				this.replacePropertiesAction(originalRun.Properties);
				this.replaceTexts.Dequeue();
				if (num - this.lastRunIndexInParagraph <= 1)
				{
					this.runsToMerge.Add(this.paragraphTextLength, this.paragraphTextLength + originalRun.Text.Length, originalRun);
				}
				this.paragraphTextLength += originalRun.Text.Length;
			}
			else
			{
				int num2 = originalRun.Text.IndexOf(this.replaceTexts.Peek());
				int num3 = 0;
				List<Run> list = ReplacePropertiesHandler.SplitRun(base.Regex, originalRun);
				foreach (Run run in list)
				{
					bool flag = string.IsNullOrEmpty(run.Text);
					if (num3 >= num2 || flag)
					{
						string replaceText = (this.HasReplaceTextsQueued ? this.replaceTexts.Dequeue() : this.lastReplaceText);
						Run run2 = this.InsertRunWithReplacedProperties(originalRun, replaceText, num);
						this.runsToMerge.Add(this.paragraphTextLength, this.paragraphTextLength + run2.Text.Length, run2);
						this.paragraphTextLength += run2.Text.Length;
						num3 += run2.Text.Length;
						num++;
					}
					if (!flag)
					{
						paragraph.Inlines.Insert(num, run);
						this.paragraphTextLength += run.Text.Length;
						num3 += run.Text.Length;
						num++;
					}
				}
				this.runsToRemove.Add(originalRun);
			}
			this.lastRunParent = paragraph;
			this.lastRunIndexInParagraph = num;
		}

		Run InsertRunWithReplacedProperties(Run runTemplate, string replaceText, int insertIndexInParagraph)
		{
			Run run = ReplacePropertiesHandler.CreateRunCopy(runTemplate, replaceText);
			this.replacePropertiesAction(run.Properties);
			runTemplate.Paragraph.Inlines.Insert(insertIndexInParagraph, run);
			return run;
		}

		void MergeProcessedRuns()
		{
			MergedRunInfo mergedRunInfo = new MergedRunInfo();
			KeyValuePair<Interval<int>, Run> keyValuePair = default(KeyValuePair<Interval<int>, Run>);
			foreach (object obj in this.runsToMerge)
			{
				KeyValuePair<Interval<int>, Run> keyValuePair2 = (KeyValuePair<Interval<int>, Run>)obj;
				if (keyValuePair.Value != null && keyValuePair.Key.End == keyValuePair2.Key.Start && (mergedRunInfo.Run == null || ReplacePropertiesHandler.ArePropertiesEqual(mergedRunInfo.Run.Properties, keyValuePair2.Value.Properties)))
				{
					mergedRunInfo.Run = keyValuePair.Value;
					if (!mergedRunInfo.IsInsertIndexInitialized)
					{
						mergedRunInfo.ParagraphInsertIndex = mergedRunInfo.Run.Paragraph.Inlines.IndexOf(mergedRunInfo.Run);
					}
					mergedRunInfo.AppendText(mergedRunInfo.Run.Text);
					this.runsToRemove.Add(keyValuePair.Value);
				}
				else if (mergedRunInfo.CanInsertMergedRun)
				{
					mergedRunInfo.AppendText(keyValuePair.Value.Text);
					this.InsertRunWithReplacedProperties(mergedRunInfo.Run, mergedRunInfo.Text, mergedRunInfo.ParagraphInsertIndex);
					this.runsToRemove.Add(keyValuePair.Value);
					mergedRunInfo = new MergedRunInfo();
				}
				keyValuePair = keyValuePair2;
			}
			if (mergedRunInfo.CanInsertMergedRun)
			{
				mergedRunInfo.AppendText(keyValuePair.Value.Text);
				this.InsertRunWithReplacedProperties(mergedRunInfo.Run, mergedRunInfo.Text, mergedRunInfo.ParagraphInsertIndex);
				this.runsToRemove.Add(keyValuePair.Value);
			}
		}

		void Clear()
		{
			foreach (Run run in this.runsToRemove)
			{
				run.Paragraph.Inlines.Remove(run);
			}
			this.runsToRemove.Clear();
			this.runsToMerge.Clear();
			this.replaceTexts.Clear();
			this.lastMatchedRun = null;
			this.paragraphTextLength = 0;
			this.lastRunIndexInParagraph = 0;
			this.lastReplaceText = string.Empty;
		}

		readonly Action<CharacterProperties> replacePropertiesAction;

		readonly HashSet<Run> runsToRemove;

		readonly IntervalMap<int, Run> runsToMerge;

		readonly Queue<string> replaceTexts;

		Run lastMatchedRun;

		Paragraph lastRunParent;

		int paragraphTextLength;

		string lastReplaceText = string.Empty;

		int lastRunIndexInParagraph;
	}
}
