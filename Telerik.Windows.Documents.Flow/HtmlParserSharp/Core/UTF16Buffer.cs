using System;

namespace HtmlParserSharp.Core
{
	sealed class UTF16Buffer
	{
		public char[] Buffer { get; set; }

		public int Start { get; set; }

		public int End { get; set; }

		public UTF16Buffer(char[] buffer, int start, int end)
		{
			this.Buffer = buffer;
			this.Start = start;
			this.End = end;
		}

		public bool HasMore
		{
			get
			{
				return this.Start < this.End;
			}
		}

		public void Adjust(bool lastWasCR)
		{
			if (lastWasCR && this.Buffer[this.Start] == '\n')
			{
				this.Start++;
			}
		}
	}
}
