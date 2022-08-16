using System;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	sealed class MergedRunInfo
	{
		public MergedRunInfo()
		{
			this.ParagraphInsertIndex = -1;
		}

		public Run Run { get; set; }

		public int ParagraphInsertIndex { get; set; }

		public string Text { get; set; }

		public bool IsInsertIndexInitialized
		{
			get
			{
				return this.ParagraphInsertIndex != -1;
			}
		}

		public bool CanInsertMergedRun
		{
			get
			{
				return this.Run != null;
			}
		}

		public void AppendText(string text)
		{
			this.Text += text;
		}

		const int NonInitializedIndex = -1;
	}
}
