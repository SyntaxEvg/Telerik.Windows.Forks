using System;
using System.Diagnostics;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch
{
	[DebuggerDisplay("Text = {Run.Text}, StartIndex = {StartIndex}, Length = {Length}, FullMatchText = {FullMatchText}")]
	class RunTextMatch
	{
		internal RunTextMatch(Run run, int startIndex, int length, string fullMatchText)
		{
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			Guard.ThrowExceptionIfLessThan<int>(0, startIndex, "startIndex");
			this.run = run;
			this.startIndex = startIndex;
			this.length = length;
			this.fullMatchText = fullMatchText;
		}

		public Run Run
		{
			get
			{
				return this.run;
			}
		}

		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		public string FullMatchText
		{
			get
			{
				return this.fullMatchText;
			}
		}

		readonly Run run;

		readonly int startIndex;

		readonly int length;

		readonly string fullMatchText;
	}
}
