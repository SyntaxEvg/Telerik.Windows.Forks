using System;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	class LongRange
	{
		public long Start
		{
			get
			{
				return this.start;
			}
			internal set
			{
				this.start = value;
			}
		}

		public long End
		{
			get
			{
				return this.end;
			}
			internal set
			{
				this.end = value;
			}
		}

		public long Length
		{
			get
			{
				return this.end - this.start + 1L;
			}
		}

		public LongRange()
		{
		}

		public LongRange(long start, long end)
		{
			this.start = start;
			this.end = end;
		}

		long start;

		long end;
	}
}
